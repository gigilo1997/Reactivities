using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Database;

public class ReactivityContext : DbContext
{
  public ReactivityContext(DbContextOptions options) : base(options)
  {

  }

  public DbSet<Activity> Activities { get; set; }
}
