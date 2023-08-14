using Serilog;

namespace MicroserviceProject.Services.Order.Infrastructure.Persistence.Repositories.Autofac;

public class AutofacTestRepo2
{
    public AutofacTestRepo2()
    {
        Log.Information("AutofacTestRepo2 Constructor çalıştı!");
    }
    
    public void Initialize()
    {
        Log.Information("Initialize çalıştı!");
    }
    
    public void GetRequest()
    {
        Log.Information("AutofacTestRepo2-GetRequest çalıştı!");
    }
}