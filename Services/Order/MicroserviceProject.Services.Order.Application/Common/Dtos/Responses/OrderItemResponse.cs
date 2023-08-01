﻿namespace MicroserviceProject.Services.Order.Application.Common.Dtos.Responses;

public class OrderItemResponse
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}