using Autofac.Extensions.DependencyInjection;
using MicroserviceProject.Services.Product.Container.Modules;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


// Autofac ile yükledik repository leri ve context'i
RepositoryModule.AddDbContext(builder.Services, builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Autofac kütüphanesini yükledikten sonra kullanmak için yazıyoruz.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
