using System.Text;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infrastructure.Messaging;

public sealed class RabbitMqConsumer
    : BackgroundService
{
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        ConnectionFactory factory = new()
        {
            HostName = "rabbitmq"
        };

        IConnection connection =
            await factory.CreateConnectionAsync(
                stoppingToken);

        IChannel channel =
            await connection.CreateChannelAsync(
                cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
            queue: "order-created",
            durable: false,
            exclusive: false,
            autoDelete: false,
            cancellationToken: stoppingToken);

        AsyncEventingBasicConsumer consumer =
            new(channel);

        consumer.ReceivedAsync += async (
            sender,
            eventArgs) =>
        {
            string message =
                Encoding.UTF8.GetString(
                    eventArgs.Body.ToArray());

            Console.WriteLine(
                $"RabbitMQ Consumer aldı: {message}");

            await channel.BasicAckAsync(
                eventArgs.DeliveryTag,
                false);
        };

        await channel.BasicConsumeAsync(
            queue: "order-created",
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        await Task.Delay(
            Timeout.Infinite,
            stoppingToken);
    }
}
