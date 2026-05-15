using System.Collections.Generic;
using Application.Orders.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Orders.Queries;

public sealed record GetOrderListQuery
    : IRequest<Result<List<OrderDto>>>;
