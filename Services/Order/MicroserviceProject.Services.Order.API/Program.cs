using System.Reflection;
using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Orders.Commands.CreateOrder;
using MicroserviceProject.Services.Order.Infrastructure.Persistence;
using MicroserviceProject.Services.Order.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<OrderDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), configure =>
    {
        configure.MigrationsAssembly(typeof(OrderDbContext).Assembly.FullName);
    });
});

// Add MediatR
builder.Services.AddMediatR(typeof(CreateOrderCommandHandler).Assembly);

builder.Services.AddScoped<IDomainEventService, DomainEventService>();
builder.Services.AddScoped<IOrderDbContext, OrderDbContext>();

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


//Password12*