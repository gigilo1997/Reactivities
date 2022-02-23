using Application.Core;
using Domain;
using MediatR;
using Persistence.Database;

namespace Application.Activities;

public class Details
{
  public record Query(Guid Id) : IRequest<Result<Activity>>;

  public class Handler : IRequestHandler<Query, Result<Activity>>
  {
    private readonly ReactivityContext context;

    public Handler(ReactivityContext context)
    {
      this.context = context;
    }

    public async Task<Result<Activity>> Handle(Query request, CancellationToken cancellationToken)
    {
      var activity = await context.Activities.FindAsync(request.Id);

      return Result<Activity>.Success(activity);
    }
  }
}
