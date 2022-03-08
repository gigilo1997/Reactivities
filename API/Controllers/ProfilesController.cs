using Application.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfilesController : ApiBaseController
{
  [HttpGet("{username}")]
  public async Task<IActionResult> GetProfile(string username)
  {
    return HandleResult(await Mediator.Send(new Details.Query(username)));
  }

  [HttpPut]
  public async Task<IActionResult> Edit(Edit.Command command)
  {
    return HandleResult(await Mediator.Send(command));
  }
}
