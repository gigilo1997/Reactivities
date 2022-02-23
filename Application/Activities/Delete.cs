using Application.Core;
using MediatR;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities;

public class Delete
{
  public record Command(Guid Id) : IRequest<Result<Unit>>;

  public class Handler : IRequestHandler<Command, Result<Unit>>
  {
    private readonly ReactivityContext context;

    public Handler(ReactivityContext context)
    {
      this.context = context;
    }

    public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
    {
      var activity = await context.Activities.FindAsync(request.Id);

      if (activity == null) return null;

      context.Remove(activity); 

      var result = await context.SaveChangesAsync() > 0;

      if (!result) return Result<Unit>.Failure("Failed to delete the activity");

      return Result<Unit>.Success(Unit.Value);
    }
  }
}
