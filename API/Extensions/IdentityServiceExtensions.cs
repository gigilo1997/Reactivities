using API.Services;
using Domain;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Persistence.Database;
using System.Text;

namespace API.Extensions;

public static class IdentityServiceExtensions
{
  public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddIdentityCore<AppUser>(options => {
      options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<ReactivityContext>()
    .AddSignInManager<SignInManager<AppUser>>();

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = key,
          ValidateIssuer = false,
          ValidateAudience = false
        };
        options.Events = new JwtBearerEvents {
          OnMessageReceived = context => {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat")) {
              context.Token = accessToken;
            }
            return Task.CompletedTask;
          }
        };
      });

    services.AddAuthorization(options => {
      options.AddPolicy("IsActivityHost", policy => {
        policy.AddRequirements(new IsHostRequirement());
      });
    });
    services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();
    services.AddScoped<TokenService>();

    return services;
  }
}
