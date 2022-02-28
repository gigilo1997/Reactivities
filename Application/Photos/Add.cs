using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Photos;

public class Add
{
  public record Command(IFormFile File) : IRequest<Result<Photo>>;

  public class Handler : IRequestHandler<Command, Result<Photo>>
  {
    private readonly ReactivityContext context;
    private readonly IPhotoAccessor photoAccessor;
    private readonly IUserAccessor userAccessor;

    public Handler(ReactivityContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
    {
      this.context = context;
      this.photoAccessor = photoAccessor;
      this.userAccessor = userAccessor;
    }

    public async Task<Result<Photo>> Handle(Command request, CancellationToken cancellationToken)
    {
      var user = await context.Users.Include(p => p.Photos)
        .FirstOrDefaultAsync(x => x.UserName == userAccessor.GetUsername());

      if (user == null) return null;

      var photoUploadResult = await photoAccessor.AddPhoto(request.File);

      var photo = new Photo {
        Url = photoUploadResult.Url,
        Id = photoUploadResult.PublicId
      };

      if (!user.Photos.Any()) photo.IsMain = true;

      user.Photos.Add(photo);

      var result = await context.SaveChangesAsync() > 0;

      if (result) return Result<Photo>.Success(photo);

      return Result<Photo>.Failure("Problem adding photo");
    }
  }
}
