using Application.Activities;
using Application.Core;
using FluentValidation.AspNetCore;
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

  public static IMvcBuilder AddMvcServices(this IMvcBuilder builder)
  {
    builder.AddFluentValidation(config => {
      config.RegisterValidatorsFromAssemblyContaining<Create>();
    });

    return builder;
  }
}
