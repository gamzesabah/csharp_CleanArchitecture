using System;
using System.Collections.Generic;
using System.Text;
using SharedKernel;

namespace Domain.Orders.Events;

public sealed record OrderCreatedDomainEvent(Guid OrderId)
    : IDomainEvent;
