using AutoMapper;
using MicroserviceProject.Services.User.Domain.ValueObjects;
using MicroserviceProject.Shared.Models.Requests;
using MicroserviceProject.Shared.Models.Responses;

namespace MicroserviceProject.Services.User.Application.Mapping;

public class MapperProfile:Profile
{
    public MapperProfile()
    {
        CreateMap<UserResponse, Domain.Entities.User>().ReverseMap();
        CreateMap<AddressResponse, Address>().ReverseMap();
        CreateMap<AddressRequest, Address>().ReverseMap();
    }
}