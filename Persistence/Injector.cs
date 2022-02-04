﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Database;

namespace Persistence;

public static class Injector
{
  public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDbContext<ReactivityContext>(options => {
      options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    });

    return services;
  }
}
