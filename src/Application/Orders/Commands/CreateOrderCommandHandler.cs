using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Orders.Dtos;
using Domain.Orders;
using Domain.Orders.ValueObjects;
using MediatR;
using SharedKernel;

namespace Application.Orders.Commands;

internal sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IMediator mediator)
    : ICommandHandler<CreateOrderCommand, OrderDto>
{
    public async Task<Result<OrderDto>> Handle(
        CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var order = Order.Create(
            new OrderName(command.Name),
            command.TotalAmount);
        if(command.Name is null)
        {
            return Result.Failure<OrderDto>(Error.NullValue);  
        }
        //early return : Araştır

        await orderRepository.AddAsync(order);

        foreach (IDomainEvent domainEvent in order.DomainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }

        var response = new OrderDto
        {
            Id = order.Id,
            Name = $"{order.Name.Value} {order.Name.Value}",
            TotalAmount = order.TotalAmount
        };

        return Result.Success(response); 
        /*sadece neden success döndürüyoruz? 
         * çünkü hata durumunu da Result ile döndürebiliriz. 
         * eğer hata durumunu da döndürmek istiyorsak Result.Failure() kullanabiliriz.
         * */
    }
}
