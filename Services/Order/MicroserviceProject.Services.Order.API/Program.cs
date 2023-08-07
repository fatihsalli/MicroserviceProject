using Autofac;
using Autofac.Extensions.DependencyInjection;
using MicroserviceProject.Services.Order.API.Modules;
using MicroserviceProject.Services.Order.Application;
using MicroserviceProject.Services.Order.Infrastructure;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
Log.Information("Application is starting...");
Log.Information("Now listening on: http://localhost:5011");

// Create Config Class to use with options pattern
builder.Services.Configure<Config>(builder.Configuration.GetSection("Config"));

// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serilog
builder.Host.UseSerilog();

//Autofac kütüphanesini yükledikten sonra kullanmak için yazıyoruz.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//Buradan Autofac kullanarak yazdığımız RepoServiceModule'ü dahil ediyoruz.
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

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