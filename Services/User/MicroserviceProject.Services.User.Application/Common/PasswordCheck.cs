namespace MicroserviceProject.Services.User.Application.Common;

/// <summary>
/// "IdentityUser" kütüphanesi kullanmadığımız için "BCrypt" kütüphanesi ile kullanıcının şifresini sisteme hashleyerek kaydetmek için kullanılır. Gerektiğinde de bu hashlenmiş veri ile şifre değerini kontrol edebiliyoruz. "IdentityUser" kütüphanesini kullanmış olsaydık "SignInManager,UserManager" gibi yardımcı servisleri kullanabilirdik.
/// </summary>
public static class PasswordCheck
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}