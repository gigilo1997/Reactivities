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
  public record Command(Guid Id) : IRequest;

  public class Handler : IRequestHandler<Command>
  {
    private readonly ReactivityContext context;

    public Handler(ReactivityContext context)
    {
      this.context = context;
    }

    public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
    {
      var activity = await context.Activities.FindAsync(request.Id);

      context.Remove(activity);

      await context.SaveChangesAsync();

      return Unit.Value;
    }
  }
}
