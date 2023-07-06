using MicroserviceProject.Order.Extensions;
using MicroserviceProject.Order.Service;
using MicroserviceProject.Shared.Configs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.Configure<Config>(builder.Configuration.GetSection("Config"));

builder.Services.AddRepositoryExtension(builder.Configuration.GetSection("Config").Get<Config>());
builder.Services.AddServiceExtension();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();