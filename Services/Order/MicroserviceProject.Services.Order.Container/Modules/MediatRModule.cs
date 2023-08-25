using System.Reflection;
using Autofac;
using MediatR;
using Module = Autofac.Module;

namespace MicroserviceProject.Services.Order.Container.Modules;

public class MediatRModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces(); 
        
        builder.Register<ServiceFactory>(ctx =>
        {
            var c = ctx.Resolve<IComponentContext>();
            return t => c.Resolve(t);
        });
        
        base.Load(builder);
    }
}