using AutoMapper;
using MicroserviceProject.Services.Order.Application.Dtos.Requests;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Services.Order.Application.Orders.Commands.CreateOrder;
using MicroserviceProject.Services.Order.Domain.Entities;
using MicroserviceProject.Services.Order.Domain.ValueObjects;

namespace MicroserviceProject.Services.Order.Application.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CreateOrderRequest, CreateOrderCommand>().ReverseMap();
        CreateMap<OrderResponse, Domain.Entities.Order>().ReverseMap();
        CreateMap<OrderItemResponse, OrderItem>().ReverseMap();
        CreateMap<OrderItemRequest, OrderItem>().ReverseMap();
        CreateMap<AddressResponse, Address>().ReverseMap();
        CreateMap<AddressRequest, Address>().ReverseMap();
    }
}