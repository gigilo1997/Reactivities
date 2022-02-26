using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Application.Activities;

public class Details
{
  public record Query(Guid Id) : IRequest<Result<ActivityDto>>;

  public class Handler : IRequestHandler<Query, Result<ActivityDto>>
  {
    private readonly ReactivityContext context;
    private readonly IMapper mapper;

    public Handler(ReactivityContext context, IMapper mapper)
    {
      this.context = context;
      this.mapper = mapper;
    }

    public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
    {
      var activity = await context.Activities
        .ProjectTo<ActivityDto>(mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(e => e.Id == request.Id);

      return Result<ActivityDto>.Success(activity);
    }
  }
}
