using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles;
public class Edit {
  public record Command(string DisplayName, string Bio) : IRequest<Result<Unit>>;

  public class CommandValidator : AbstractValidator<Command>
  {
    public CommandValidator()
    {
      RuleFor(x => x.DisplayName).NotEmpty();
    }
  }

  public class Handler : IRequestHandler<Command, Result<Unit>>
  {
    private readonly ReactivityContext context;
    private readonly IUserAccessor userAccessor;

    public Handler(ReactivityContext context, IUserAccessor userAccessor)
    {
      this.context = context;
      this.userAccessor = userAccessor;
    }

    public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
    {
      var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == userAccessor.GetUsername());

      user.Bio = request.Bio ?? user.Bio;
      user.DisplayName = request.DisplayName ?? user.DisplayName;
      var success = await context.SaveChangesAsync() > 0;
      if (success) return Result<Unit>.Success(Unit.Value);
      return Result<Unit>.Failure("Problem updating profile");
    }
  }
}
