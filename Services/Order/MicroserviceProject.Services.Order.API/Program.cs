using System.Reflection;
using MediatR;
using MicroserviceProject.Services.Order.Application;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Mapping;
using MicroserviceProject.Services.Order.Application.Orders.Commands.CreateOrder;
using MicroserviceProject.Services.Order.Infrastructure;
using MicroserviceProject.Shared.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
Log.Information("Application is starting...");

// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddControllers();
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
