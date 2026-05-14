using Domain.Orders.Events;
using Domain.Orders.ValueObjects;
using SharedKernel;

namespace Domain.Orders;

public class Order
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public Guid Id { get; private set; }
    public OrderName Name { get; private set; }
    public decimal TotalAmount { get; private set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents
        => _domainEvents.AsReadOnly();

    private Order()
    {
    }

    private Order(Guid id, OrderName name, decimal totalAmount)
    {
        Id = id;
        Name = name;
        TotalAmount = totalAmount;
    }

    public static Order Create(OrderName name, decimal totalAmount)
    {
        if (totalAmount < 0)
        {
            throw new Exception("Total amount negatif olamaz");
        }

        var order = new Order(Guid.NewGuid(), name, totalAmount);

        order.AddDomainEvent(
            new OrderCreatedDomainEvent(order.Id));

        return order;
    }

    public void UpdateAmount(decimal amount)
    {
        if (amount < 0)
        {
            throw new Exception("Negatif amount olamaz");
        }

        TotalAmount = amount;
    }

    private void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
