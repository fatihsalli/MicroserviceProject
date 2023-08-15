using Autofac;
using MicroserviceProject.Services.Product.Container.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Product.Container
{
    public class Bootstrapper
    {
        public static ILifetimeScope Container { get; private set; }

        public static void RegisterModules(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new MediatRModule());
            containerBuilder.RegisterModule(new RepositoryModule());

        }

        public static void SetContainer(ILifetimeScope autofacContainer) 
        {
            Container= autofacContainer;
        }




    }
}
