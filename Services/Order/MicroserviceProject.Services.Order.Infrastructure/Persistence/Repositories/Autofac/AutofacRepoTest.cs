using Serilog;

namespace MicroserviceProject.Services.Order.Infrastructure.Persistence.Repositories.Autofac;

public class AutofacTestRepo : IAutofacTestRepo
{
    public AutofacTestRepo()
    {
        Log.Information("AutofacTestRepo Constructor çalıştı!");
    }

    public void StartListening()
    {
        Log.Information("Starting Listening çalıştı!");
    }

    public void GetRequest()
    {
        Log.Information("AutofacTestRepo-GetRequest çalıştı!");
    }


    
}