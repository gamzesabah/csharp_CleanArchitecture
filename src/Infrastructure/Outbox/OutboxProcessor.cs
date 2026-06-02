using Domain.Outbox;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Outbox;

public sealed class OutboxProcessor(
    IServiceScopeFactory serviceScopeFactory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        { 

            using IServiceScope scope =
                serviceScopeFactory.CreateScope();

            ApplicationDbContext context =
                scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

            List<OutboxMessage> messages =
                await context.OutboxMessages
                    .Where(x => x.ProcessedOnUtc == null)
                    .ToListAsync(stoppingToken);

            Console.WriteLine(
                $"Bulunan mesaj sayısı: {messages.Count}");

            foreach (OutboxMessage message in messages)
            {
                Console.WriteLine(
                    $"İşleniyor: {message.Type}");

                message.MarkAsProcessed();
            }

            await context.SaveChangesAsync(
                stoppingToken);

            Console.WriteLine(
                $"Bulunan mesaj sayısı: {messages.Count}");

            await Task.Delay(
                TimeSpan.FromSeconds(10),
                stoppingToken);
        }
    }
}
