using Application.Abstractions.Data;
using Application.Orders.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Orders.Queries;

internal sealed class GetOrderListQueryHandler(
    IApplicationDbContext context)
    : IRequestHandler<
        GetOrderListQuery,
        Result<List<OrderDto>>>
{
    public async Task<Result<List<OrderDto>>> Handle(
        GetOrderListQuery request,
        CancellationToken cancellationToken)
    {
        List<OrderDto> orders =
            await context.Orders
                .Select(x => new OrderDto
                {
                    Id = x.Id,
                    Name = x.Name.Value,
                    TotalAmount = x.TotalAmount
                })
                .ToListAsync(cancellationToken);

        return Result.Success(orders);
    }
}
