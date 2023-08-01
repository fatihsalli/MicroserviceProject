using MediatR;
using MicroserviceProject.Services.Order.Application.Common.Interfaces;
using MicroserviceProject.Services.Order.Application.Orders.Commands.UpdateOrder;
using MicroserviceProject.Services.Order.Domain.Events;
using MicroserviceProject.Services.Order.Domain.ValueObjects;
using MicroserviceProject.Shared.Configs;
using MicroserviceProject.Shared.Exceptions;
using MicroserviceProject.Shared.Kafka;
using MicroserviceProject.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace MicroserviceProject.Services.Order.Application.Orders.Handlers.CommandHandlers;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, CustomResponse<bool>>
{
    private readonly IOrderDbContext _context;
    private readonly KafkaProducer _kafkaProducer;
    private readonly Config _config;

    public UpdateOrderCommandHandler(IOrderDbContext context,KafkaProducer kafkaProducer,IOptions<Config> config)
    {
        _context = context;
        _kafkaProducer = kafkaProducer;
        _config = config.Value;
    }
    
    public async Task<CustomResponse<bool>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _context.Orders
                .Include(x => x.OrderItems)
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (order == null)
                throw new NotFoundException("order", request.Id);

            var newAddress = new Address(
                request.Address.Province,
                request.Address.District,
                request.Address.Street,
                request.Address.Zip,
                request.Address.Line);
            
            order.AddDomainEvent(new OrderUpdatedEvent(order));

            order.UpdateAddress(newAddress);

            await _context.SaveChangesAsync(cancellationToken);
            


            return CustomResponse<bool>.Success(200, true);
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case NotFoundException:
                    Log.Information(ex, "UpdateOrderCommandHandler exception. Not Found Error");
                    throw new NotFoundException($"Not Found Error. Error message:{ex.Message}");
                default:
                    Log.Error(ex, "UpdateOrderCommandHandler exception. Internal Server Error");
                    throw new Exception("Something went wrong.");
            }
        }
    }
}