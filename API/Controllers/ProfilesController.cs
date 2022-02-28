﻿using Application.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProfilesController : ApiBaseController
{
  [HttpGet("{username}")]
  public async Task<IActionResult> GetProfile(string username)
  {
    return HandleResult(await Mediator.Send(new Details.Query(username)));
  }
}
