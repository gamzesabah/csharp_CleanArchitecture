using MediatR;
using Application.Orders.Dtos;
using SharedKernel;

namespace Application.Orders.Commands;

public sealed record CreateOrderCommand(
    string Name,
    decimal TotalAmount)
    : IRequest<Result<OrderDto>>;
