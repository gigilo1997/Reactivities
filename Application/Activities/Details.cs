using Domain;
using MediatR;
using Persistence.Database;

namespace Application.Activities;

public class Details
{
  public record Query(Guid Id) : IRequest<Activity>;

  public class Handler : IRequestHandler<Query, Activity>
  {
    private readonly ReactivityContext context;

    public Handler(ReactivityContext context)
    {
      this.context = context;
    }

    public async Task<Activity> Handle(Query request, CancellationToken cancellationToken)
    {
      return await context.Activities.FindAsync(request.Id);
    }
  }
}
