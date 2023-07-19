using AutoMapper;
using MicroserviceProject.Services.User.Application.Dtos.Requests;
using MicroserviceProject.Services.User.Application.Dtos.Responses;
using MicroserviceProject.Services.User.Domain.Entities;
using MicroserviceProject.Services.User.Domain.ValueObjects;

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