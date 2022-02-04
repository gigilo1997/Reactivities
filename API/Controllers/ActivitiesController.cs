using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace API.Controllers;

public class ActivitiesController : ApiBaseController
{
  private readonly ReactivityContext context;

  public ActivitiesController(ReactivityContext context)
  {
    this.context = context;
  }

  [HttpGet]
  public async Task<ActionResult<List<Activity>>> GetActivities()
  {
    return await context.Activities.ToListAsync();
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Activity>> GetActivity(Guid id)
  {
    return await context.Activities.SingleOrDefaultAsync(x => x.Id == id);
  }
}
