﻿namespace MicroserviceProject.Services.Order.Application.Dtos;

public class OrderItemDto
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}