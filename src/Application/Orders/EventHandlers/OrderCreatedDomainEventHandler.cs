using Domain.Orders.Events;
using Microsoft.Extensions.Logging;
using MediatR;

namespace Application.Orders.EventHandlers;

internal sealed class OrderCreatedDomainEventHandler(
    ILogger<OrderCreatedDomainEventHandler> logger)
    : INotificationHandler<OrderCreatedDomainEvent>
{
    public Task Handle(
        OrderCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Order created event received. OrderId: {OrderId}",
            notification.OrderId);

        return Task.CompletedTask;
    }
}
