using Application.Abstractions.Data;
using Application.Abstractions.Events;
using Application.Orders.Dtos;
using Domain.Orders;
using Domain.Orders.ValueObjects;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernel;

namespace Application.Orders.Commands;

internal sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IApplicationDbContext context,
    IDomainEventsDispatcher domainEventsDispatcher)
    : IRequestHandler<CreateOrderCommand, Result<OrderDto>>
{
    public async Task<Result<OrderDto>> Handle(
        CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        await using IDbContextTransaction transaction =
            await context.BeginTransactionAsync(
                cancellationToken);

        try
        {
            bool exists =
                await orderRepository.ExistsByNameAsync(
                    command.Name,
                    cancellationToken);

            if (exists)
            {
                return Result.Failure<OrderDto>(
                    Error.Conflict(
                        "Order.AlreadyExists",
                        "Order with same name already exists"));
            }

            Product? product =
                await productRepository.GetByIdAsync(
                    command.ProductId,
                    cancellationToken);

            if (product is null)
            {
                return Result.Failure<OrderDto>(
                    Error.NotFound(
                        "Product.NotFound",
                        "Product not found"));
            }

            Result stockResult =
                product.ReduceStock(1);

            if (stockResult.IsFailure)
            {
                return Result.Failure<OrderDto>(
                    stockResult.Error);
            }

            var order = Order.Create(
                new OrderName(command.Name),
                command.TotalAmount);

            await orderRepository.AddAsync(order);

            await productRepository.UpdateAsync(
                product,
                cancellationToken);

            await context.SaveChangesAsync(
                cancellationToken);

            await transaction.CommitAsync(
                cancellationToken);

            await domainEventsDispatcher.DispatchAsync(
                order.DomainEvents,
                cancellationToken);

            return Result.Success(
                new OrderDto
                {
                    Id = order.Id,
                    Name = order.Name.Value,
                    TotalAmount = order.TotalAmount
                });
        }
        catch
        {
            await transaction.RollbackAsync(
                cancellationToken);

            throw;
        }
    }
}
