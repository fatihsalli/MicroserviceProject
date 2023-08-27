using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using MicroserviceProject.Services.Order.API.Middlewares;
using MicroserviceProject.Services.Order.Container;
using MicroserviceProject.Services.Order.Container.Modules;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace MicroserviceProject.Services.Order.API;

public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var environmentNamePath = string.IsNullOrEmpty(environmentName) ? "" : environmentName + ".";
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environmentNamePath}json", optional: false)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RepositoryModule.AddDbContext(services, Configuration);
            
            services.AddControllers();
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //add service for httpContext
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
        {
            var container = app.ApplicationServices.GetAutofacRoot();
            Bootstrapper.SetContainer(container);

            applicationLifetime.ApplicationStarted.Register(OnStart);
            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            #if DEBUG
            app.UseCors("local");
            #endif

            app.UseExceptionHandler("/error");
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackOffice.Api v1"));
            app.UseRouting();
            app.UseAuthorization();
            
            app.UseMiddleware<CorrelationIdMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        
        public void ConfigureContainer(ContainerBuilder builder)
        {
            Bootstrapper.RegisterModules(builder);
        }

        private void OnShutdown()
        {
            var busControl = Bootstrapper.Container.Resolve<IBusControl>();
            busControl.StopAsync();
        }

        private void OnStart()
        {
            var busControl = Bootstrapper.Container.Resolve<IBusControl>();
            busControl.StartAsync();
        }
    }