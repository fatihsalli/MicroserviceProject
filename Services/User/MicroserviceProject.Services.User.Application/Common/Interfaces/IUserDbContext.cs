using MongoDB.Driver;

namespace MicroserviceProject.Services.User.Application.Common.Interfaces;

public interface IUserDbContext
{
    IMongoCollection<Domain.Entities.User> Users { get; }
}