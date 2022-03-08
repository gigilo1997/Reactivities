using Application;
using Application.Activities;
using Application.Core;
using Application.Interfaces;
using FluentValidation.AspNetCore;
using Infrastructure.Photos;
using Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddControllers(options => {
      var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
      options.Filters.Add(new AuthorizeFilter(policy));
    }).AddFluentValidation(config => {
      config.RegisterValidatorsFromAssemblyContaining<Create>();
    });
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddCors(opt => {
      opt.AddPolicy("CorsPolicy", policy => {
        policy
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithOrigins("http://localhost:3000");
      });
    });

    services.AddDbContext<ReactivityContext>(options => {
      options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    });
    services.AddMediatR(typeof(List.Handler).Assembly);
    services.AddAutoMapper(typeof(MappingProfiles).Assembly);
    services.AddScoped<IUserAccessor, UserAccessor>();
    services.AddScoped<IPhotoAccessor, PhotoAccessor>();

    services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
    services.AddSignalR();

    return services;
  }
}
