using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database;

public class ReactivityContext : IdentityDbContext<AppUser>
{
  public ReactivityContext(DbContextOptions options) : base(options)
  {

  }

  public DbSet<Activity> Activities { get; set; }
}
