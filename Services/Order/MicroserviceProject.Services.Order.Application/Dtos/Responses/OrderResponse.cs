﻿namespace MicroserviceProject.Services.Order.Application.Dtos.Responses;

public class OrderResponse
{
    public OrderResponse()
    {
        OrderItems = new List<OrderItemResponse>();
    }
    
    public string Id { get; set; }
    public decimal TotalPrice { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdadetAt { get; set; }
    public AddressResponse Address { get; set; }
    public List<OrderItemResponse> OrderItems { get; set; }
}