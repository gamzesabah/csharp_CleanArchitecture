using Domain.Users;
using MediatR;

namespace Application.Users.Register;

internal sealed class UserRegisteredDomainEventHandler
    : INotificationHandler<UserRegisteredDomainEvent>
{
    public Task Handle(
        UserRegisteredDomainEvent notification,
        CancellationToken cancellationToken)
    {
        // TODO: Send an email verification link, etc.
        return Task.CompletedTask;
    }
}

internal sealed class UserRegisteredDomainEventHandler1
    : INotificationHandler<UserRegisteredDomainEvent>
{
    public Task Handle(
        UserRegisteredDomainEvent notification,
        CancellationToken cancellationToken)
    {
        // TODO: Send an email verification link, etc.
        return Task.CompletedTask;
    }
}
