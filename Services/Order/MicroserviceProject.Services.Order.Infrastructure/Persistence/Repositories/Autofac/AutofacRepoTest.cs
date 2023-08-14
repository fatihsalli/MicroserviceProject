using Serilog;

namespace MicroserviceProject.Services.Order.Infrastructure.Persistence.Repositories.Autofac;

public class AutofacRepoTest : IAutofacRepoTest
{
    public AutofacRepoTest()
    {
        Log.Information("AutofacRepoTest Constructor çalıştı!");
    }

    public void StartListening()
    {
        Log.Information("Starting Listening çalıştı!");
    }

    public void GetRequest()
    {
        Log.Information("AutofacRepoTest-GetRequest çalıştı!");
    }
}