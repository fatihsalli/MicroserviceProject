using AutoMapper;
using MicroserviceProject.Services.Order.Application.Dtos;
using MicroserviceProject.Services.Order.Domain.OrderAggregate;

namespace MicroserviceProject.Services.Order.Application.Mapping;

public class CustomMapping:Profile
{
    public CustomMapping()
    {
        CreateMap<Domain.OrderAggregate.Order, OrderDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<Address, AddressDto>().ReverseMap();
    }
}