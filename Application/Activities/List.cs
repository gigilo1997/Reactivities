using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Application.Activities;

public class List
{
  public record Query : IRequest<List<Activity>>;

  public class Handler : IRequestHandler<Query, List<Activity>>
  {
    private readonly ReactivityContext context;

    public Handler(ReactivityContext context)
    {
      this.context = context;
    }
    public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
    {
      return await context.Activities.ToListAsync();
    }
  }
}
