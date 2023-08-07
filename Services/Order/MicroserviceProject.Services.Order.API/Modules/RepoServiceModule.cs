using System.Reflection;
using Autofac;
using MicroserviceProject.Services.Order.Application.Common.Behaviours;
using MicroserviceProject.Services.Order.Application.Common.Interfaces.Repositories;
using MicroserviceProject.Services.Order.Application.Orders.Commands.CreateOrder;
using MicroserviceProject.Services.Order.Infrastructure.Persistence;
using MicroserviceProject.Services.Order.Infrastructure.Persistence.Repositories;
using Module = Autofac.Module;

namespace MicroserviceProject.Services.Order.API.Modules;

//Autofac=> Module'den miras alıyor.
public class RepoServiceModule : Module
{
    protected override void Load(ContainerBuilder builder)
        {
            //Generic olduğu için bu şekilde belirttik.
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerLifetimeScope();
            // builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();
            // Generic değil o yüzden type olarak ekledik.
            // builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            //Bunun anlamı da bulunduğun klasörde ara demek
            var apiAssembly = Assembly.GetExecutingAssembly();
            // Application katmanındaki herhangi bir classı typeof içine verebiliriz. Bu şekilde diğerlerini kendisi bulacaktır.
            var applicationAssembly = Assembly.GetAssembly(typeof(CreateOrderCommand));
            //Service katmanındaki herhangi bir classı typeof içine verebiliriz. Bu şekilde diğerlerini kendisi bulacaktır.
            var infrastructureAssembly = Assembly.GetAssembly(typeof(OrderDbContext));

            //InstancePerLifetimeScope => Scope a karşılık gelir.
            //InstancePerDependency => Transient a karşılık gelir.
            //Burada şunu demek istiyoruz, bu verilen Assembly'lere git ve sonu Repository ile bitenlerin Scope olarak instance'ını al. builder.Services.AddScoped<IProductRepository, ProductRepository>(); => bu yazdığımızı tüm repository yerine tek tek yazmak yerine bu şekilde bütün hepsi için Scope olarak instance alıyoruz.
            builder.RegisterAssemblyTypes(apiAssembly, applicationAssembly, infrastructureAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(apiAssembly, applicationAssembly, infrastructureAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            
        }
}