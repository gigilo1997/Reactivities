using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Application.Activities;

public class List
{
  public record Query : IRequest<Result<List<Activity>>>;

  public class Handler : IRequestHandler<Query, Result<List<Activity>>>
  {
    private readonly ReactivityContext context;

    public Handler(ReactivityContext context)
    {
      this.context = context;
    }
    public async Task<Result<List<Activity>>> Handle(Query request, CancellationToken cancellationToken)
    {
      return Result<List<Activity>>.Success(await context.Activities.ToListAsync(cancellationToken));
    }
  }
}
