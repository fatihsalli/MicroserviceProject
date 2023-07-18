using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Dtos.Requests;
using MicroserviceProject.Services.Order.Application.Dtos.Responses;
using MicroserviceProject.Services.Order.Domain.ValueObjects;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Commands.UpdateOrder;

/// <summary>
/// Kullanıcı siparişi oluşturduktan sonra address bilgisini değiştirmek istediğini farzedelim. Siparişi ipatl etmek yerine update ile değiştiriyoruz.
/// </summary>
public class UpdateOrderCommand : IRequest<CustomResponse<bool>>
{
    public string Id { get; set; }
    public AddressRequest Address { get; set; }
}

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, CustomResponse<bool>>
{
    private readonly IOrderDbContext _context;

    public UpdateOrderCommandHandler(IOrderDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<CustomResponse<bool>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _context.Orders
                .Include(x => x.OrderItems)
                .Where(x=>x.Id==request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (order == null)
                throw new NotFoundException("order",request.Id);

            var newAddress = new Address(
                request.Address.Province,
                request.Address.District,
                request.Address.Street,
                request.Address.Zip,
                request.Address.Line);
            
            order.UpdateAddress(newAddress);

            await _context.SaveChangesAsync(cancellationToken);

            return CustomResponse<bool>.Success(200, true);
        }
        catch (Exception ex)
        {
            if (ex is NotFoundException)
            {
                Log.Information(ex, "UpdateOrderCommandHandler exception. Not Found Error");
                throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
            }
            
            Log.Error(ex, "UpdateOrderCommandHandler exception. Internal Server Error");
            throw new Exception("Something went wrong.");
        }
    }
}