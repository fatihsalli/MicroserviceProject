﻿using MediatR;
using MicroserviceProject.Shared.Models;
using MicroserviceProject.Shared.Models.Requests;

namespace MicroserviceProject.Services.Order.Application.Orders.Commands.UpdateOrder;

/// <summary>
///     Kullanıcı siparişi oluşturduktan sonra address bilgisini değiştirmek istediğini farzedelim. Siparişi ipatl etmek
///     yerine update ile değiştiriyoruz.
/// </summary>
public class UpdateOrderCommand : IRequest<CustomResponse<bool>>
{
    public string Id { get; set; }
    public AddressRequest Address { get; set; }
}