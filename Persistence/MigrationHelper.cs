using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence.Database;

namespace Persistence;

public static class MigrationHelper
{
  public static async Task MigrateDatabase(this IHost host)
  {
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;
    try {
      var context = services.GetRequiredService<ReactivityContext>();
      await context.Database.MigrateAsync();
      await context.SeedDataAsync();
    }
    catch (Exception ex) {
      var logger = services.GetRequiredService<ILogger>();
      logger.LogError(ex, "An error occured during migration");
    }
  }
}
