using Domain;
using MediatR;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities;

public class Create
{
  public record Command(Activity Activity) : IRequest;

  public class Handler : IRequestHandler<Command>
  {
    private readonly ReactivityContext context;

    public Handler(ReactivityContext context)
    {
      this.context = context;
    }

    public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
    {
      context.Activities.Add(request.Activity);
      await context.SaveChangesAsync();
      return Unit.Value;
    }
  }
}
