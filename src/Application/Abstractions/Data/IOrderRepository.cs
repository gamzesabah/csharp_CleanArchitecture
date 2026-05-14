using System;
using System.Collections.Generic;
using System.Text;
using Domain.Orders;

namespace Application.Abstractions.Data;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task<Order?> GetByIdAsync(Guid id);
    Task<bool> ExistsByNameAsync(
    string name,
    CancellationToken cancellationToken);
}
