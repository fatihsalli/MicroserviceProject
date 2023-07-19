namespace MicroserviceProject.Services.User.Application.Common.Interfaces;

/// <summary>
/// Options pattern ile doldurmak için kullanıyoruz.
/// </summary>
public interface IUserDatabaseSettings
{
    public string UserCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}