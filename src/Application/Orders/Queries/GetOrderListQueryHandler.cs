using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Messaging;
using Application.Orders.Dtos;
using SharedKernel;

namespace Application.Orders.Queries;

internal sealed class GetOrderListQueryHandler
    : IQueryHandler<GetOrderListQuery, List<OrderDto>>
{
    public async Task<Result<List<OrderDto>>> Handle(
        GetOrderListQuery request,
        CancellationToken cancellationToken)
    {
        var orders = new List<OrderDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Laptop",
                TotalAmount = 1000
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Mouse",
                TotalAmount = 200
            }
        };
        return await Task.FromResult(Result.Success(orders));
    }
}
