namespace MicroserviceProject.Services.Order.Application.Common.Helpers;

public static class Helpers
{
    public static bool BeValidGuid(string uuid)
    {
        Guid guid;
        return Guid.TryParse(uuid, out guid);
    }
}