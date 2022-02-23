using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
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
  public record Command(Activity Activity) : IRequest<Result<Unit>>;

  public class CommandValidator : AbstractValidator<Command>
  {
    public CommandValidator()
    {
      RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
    }
  }

  public class Handler : IRequestHandler<Command, Result<Unit>>
  {
    private readonly ReactivityContext context;
    private readonly IMapper mapper;

    public Handler(ReactivityContext context, IMapper mapper)
    {
      this.context = context;
      this.mapper = mapper;
    }

    public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
    {
      var activity = await context.Activities.FindAsync(request.Activity.Id);

      if (activity == null) return null;

      mapper.Map(request.Activity, activity);

      var result = await context.SaveChangesAsync() > 0;

      if (!result) return Result<Unit>.Failure("Failed to update activity");

      return Result<Unit>.Success(Unit.Value);
    }
  }
}
