using Serilog;

namespace MicroserviceProject.Services.Order.Infrastructure.Persistence.Repositories.Autofac;

public class AutofacRepoTest2 : IAutofacRepoTest2
{
    public AutofacRepoTest2()
    {
        Log.Information("AutofacRepoTest2 Constructor çalıştı!");
    }

    public void Initialize()
    {
        Log.Information("Initialize çalıştı!");
    }

    public void GetRequest()
    {
        Log.Information("AutofacRepoTest2-GetRequest çalıştı!");
    }
}