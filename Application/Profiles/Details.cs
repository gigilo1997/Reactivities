using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles;

public class Details
{
  public record Query(string Username) : IRequest<Result<Profile>>;

  public class Handler : IRequestHandler<Query, Result<Profile>>
  {
    private readonly ReactivityContext context;
    private readonly IMapper mapper;

    public Handler(ReactivityContext context, IMapper mapper)
    {
      this.context = context;
      this.mapper = mapper;
    }

    public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
    {
      var user = await context.Users
        .ProjectTo<Profile>(mapper.ConfigurationProvider)
        .SingleOrDefaultAsync(x => x.Username == request.Username);

      if (user == null) return null;

      return Result<Profile>.Success(user);
    }
  }
}
