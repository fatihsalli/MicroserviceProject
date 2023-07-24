using MicroserviceProject.Services.User.Application;
using MicroserviceProject.Services.User.Infrastructure;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
Log.Information("Application is starting...");
Log.Information("Now listening on: http://localhost:5012");

// Create Config Class to use with options pattern
builder.Services.Configure<Config>(builder.Configuration.GetSection("Config"));

// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serilog
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Custom middleware
app.UseCustomException();

app.UseAuthorization();

app.MapControllers();

app.Run();