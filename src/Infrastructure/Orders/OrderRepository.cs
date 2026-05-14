using Application.Abstractions.Data;
using Domain.Orders;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Orders;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;
    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _context.Orders
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<bool> ExistsByNameAsync(
    string name,
    CancellationToken cancellationToken)
    {
        return await _context.Orders
            .AnyAsync(x => x.Name.Value == name, cancellationToken);
    }
}
