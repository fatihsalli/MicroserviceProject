using Autofac;
using MicroserviceProject.Services.Product.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Module = Autofac.Module;

namespace MicroserviceProject.Services.Product.Container.Modules
{
    public class RepositoryModule : Module
    {
        private static string _connectionString;

        public static void AddDbContext(IServiceCollection services,IConfiguration configuration)
        {
            _connectionString = configuration["DbConnString"];

            services.AddEntityFrameworkSqlServer().AddDbContext<ProductDbContext>
                (options => options.UseSqlServer(_connectionString));
        }

        protected override void Load(ContainerBuilder builder)
        {
            var assemblyType = typeof(ProductRepository).GetTypeInfo();

            builder.RegisterAssemblyTypes(assemblyType.Assembly)
                .Where(x => x != typeof(ProductDbContext))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }

    }
}
