using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiBaseController : ControllerBase
{
  private IMediator mediator;

  protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetService<IMediator>();
}
