using System.Threading.Channels;
using Domain.Outbox;

namespace Infrastructure.Messaging;

public sealed class InMemoryEventBus
    : IEventBus
{
    private readonly Channel<OutboxMessage> _channel;

    public InMemoryEventBus(
        Channel<OutboxMessage> channel)
    {
        _channel = channel;
    }

    public async ValueTask PublishAsync(
        OutboxMessage message,
        CancellationToken cancellationToken)
    {
        await _channel.Writer.WriteAsync(
            message,
            cancellationToken);

        Console.WriteLine(
            $"Queue'ya gönderildi: {message.Type}");
    }
}
