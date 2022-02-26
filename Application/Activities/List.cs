using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Application.Activities;

public class List
{
  public record Query : IRequest<Result<List<ActivityDto>>>;

  public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
  {
    private readonly ReactivityContext context;
    private readonly IMapper mapper;

    public Handler(ReactivityContext context, IMapper mapper)
    {
      this.context = context;
      this.mapper = mapper;
    }
    public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
    {
      var activities = await context.Activities
        .ProjectTo<ActivityDto>(mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);

      return Result<List<ActivityDto>>.Success(activities);
    }
  }
}
