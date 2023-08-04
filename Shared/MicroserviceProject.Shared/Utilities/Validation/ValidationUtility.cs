namespace MicroserviceProject.Shared.Utilities.Validation;

public static class ValidationUtility
{
    /// <summary>
    /// FluentValidation kullanırken "uuid" değerini kontrol etmek için kullanılır.
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
    public static bool BeValidGuid(string uuid)
    {
        Guid guid;
        return Guid.TryParse(uuid, out guid);
    }
}