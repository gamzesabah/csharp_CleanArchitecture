using SharedKernel;

namespace Application.Abstractions.Events;

public interface IDomainEventsDispatcher
{
    Task DispatchAsync(
        IReadOnlyCollection<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default);
}
