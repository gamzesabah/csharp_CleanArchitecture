using System.Threading.Channels;
using Domain.Outbox;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Messaging;

public sealed class EventConsumer(
    Channel<OutboxMessage> channel)
    : BackgroundService
{
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            OutboxMessage message =
                await channel.Reader.ReadAsync(
                    stoppingToken);

            Console.WriteLine(
                $"Consumer aldı: {message.Type}");
        }
    }
}
