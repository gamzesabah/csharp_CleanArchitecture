using Application.Abstractions.Data;
using Application.Abstractions.Events;
using Application.Orders.Dtos;
using Domain.Orders;
using Domain.Orders.ValueObjects;
using Domain.Products;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using SharedKernel;
using System.Text.Json;
using Domain.Outbox;

namespace Application.Orders.Commands;

internal sealed class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IApplicationDbContext context)
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
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure<OrderDto>(
                    Error.NotFound(
                        "Product.NotFound",
                        "Product not found"));
            }

            Result stockResult =
                product.ReduceStock(1);

            if (stockResult.IsFailure)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Failure<OrderDto>(
                    stockResult.Error);
            }

            var order = Order.Create(
                new OrderName(command.Name),
                command.TotalAmount);

            var outboxMessage =
                new OutboxMessage(
                    Guid.NewGuid(),
                    "OrderCreatedEvent",
                    JsonSerializer.Serialize(
                        new
                        {
                            OrderId = order.Id,
                            OrderName = order.Name.Value,
                            Amount = order.TotalAmount
                        }),
                    DateTime.UtcNow);

            await orderRepository.AddAsync(order);

            await context.OutboxMessages.AddAsync(
                outboxMessage,
                cancellationToken);

            await productRepository.UpdateAsync(
                product,
                cancellationToken);

            Console.WriteLine(
                $"Outbox Count Local: {context.OutboxMessages.Local.Count}");

            await context.SaveChangesAsync(
                cancellationToken);

            await transaction.CommitAsync(
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
