using Domain.Outbox;

namespace Infrastructure.Messaging;

public interface IEventBus
{
    ValueTask PublishAsync(
        OutboxMessage message,
        CancellationToken cancellationToken);
}
