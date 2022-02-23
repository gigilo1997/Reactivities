using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence.Database;

namespace Application.Activities;

public class Create
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

    public Handler(ReactivityContext context)
    {
      this.context = context;
    }

    public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
    {
      context.Activities.Add(request.Activity);
      var result = await context.SaveChangesAsync() > 0;
      if (!result) return Result<Unit>.Failure("Failed to create activity");
      return Result<Unit>.Success(Unit.Value);
    }
  }
}
