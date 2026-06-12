using MediatR;
using SharedKernel;
using Application.Orders.Dtos;

namespace Application.Orders.Commands;

public sealed record CreateOrderCommand(
    string Name,
    decimal TotalAmount,
    Guid ProductId)
    : IRequest<Result<OrderDto>>;
