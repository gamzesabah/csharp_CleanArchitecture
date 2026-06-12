using System.Text;
using Domain.Outbox;
using RabbitMQ.Client;

namespace Infrastructure.Messaging;

public sealed class RabbitMqEventBus
    : IEventBus
{
    public async ValueTask PublishAsync(
        OutboxMessage message,
        CancellationToken cancellationToken)
    {
        ConnectionFactory factory = new()
        {
            HostName = "rabbitmq"
        };

        await using IConnection connection =
            await factory.CreateConnectionAsync(
                cancellationToken);

        await using IChannel channel =
            await connection.CreateChannelAsync(
                cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: "order-created",
            durable: false,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        byte[] body =
            Encoding.UTF8.GetBytes(
                message.Content);

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: "order-created",
            body: body,
            cancellationToken: cancellationToken);

        Console.WriteLine(
            $"RabbitMQ'ya gönderildi: {message.Type}");
    }
}
