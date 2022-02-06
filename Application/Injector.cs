using Application.Activities;
using Application.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Application;
public static class Injector
{
  public static IServiceCollection AddReactivityServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDatabase(configuration);
    services.AddMediatR(typeof(List.Handler).Assembly);
    services.AddAutoMapper(typeof(MappingProfiles).Assembly);

    return services;
  }
}
