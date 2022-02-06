using AutoMapper;
using Domain;
using MediatR;
using Persistence.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities;

public class Edit
{
  public record Command(Activity Activity) : IRequest;

  public class Handler : IRequestHandler<Command>
  {
    private readonly ReactivityContext context;
    private readonly IMapper mapper;

    public Handler(ReactivityContext context, IMapper mapper)
    {
      this.context = context;
      this.mapper = mapper;
    }

    public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
    {
      var activity = await context.Activities.FindAsync(request.Activity.Id);

      mapper.Map(request.Activity, activity);

      await context.SaveChangesAsync();

      return Unit.Value;
    }
  }
}
