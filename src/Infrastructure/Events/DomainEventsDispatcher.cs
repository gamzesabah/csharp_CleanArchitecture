using Application.Abstractions.Events;
using MediatR;
using SharedKernel;

namespace Infrastructure.Events;

public sealed class DomainEventsDispatcher(
    IMediator mediator)
    : IDomainEventsDispatcher
{
    public async Task DispatchAsync(
        IReadOnlyCollection<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await mediator.Publish(
                domainEvent,
                cancellationToken);
        }
    }
}
