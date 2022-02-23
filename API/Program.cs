using API.Middleware;
using Application;
using Persistence.Database;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;
services.AddControllers().AddMvcServices();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddCors(opt => {
  opt.AddPolicy("CorsPolicy", policy => {
    policy.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:3000");
  });
});

services.AddReactivityServices(configuration);

var app = builder.Build();

await app.MigrateDatabase();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
