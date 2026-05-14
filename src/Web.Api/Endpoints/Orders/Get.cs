using Application.Abstractions.Messaging;
using Application.Orders.Dtos;
using Application.Orders.Queries;

namespace Web.Api.Endpoints.Orders;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders", async (
            IQueryHandler<GetOrderListQuery, List<OrderDto>> handler,
            CancellationToken cancellationToken) =>
        {
            GetOrderListQuery query = new();

            return Results.Ok(await handler.Handle(query, cancellationToken));
        })
        .WithTags("Orders");
    }
}
