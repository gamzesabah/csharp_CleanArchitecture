using Application.Orders.Dtos;
using Application.Orders.Queries;
using MediatR;
using SharedKernel;

namespace Web.Api.Endpoints.Orders;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("orders", async (
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            GetOrderListQuery query = new();

            Result<List<OrderDto>> result =
                await mediator.Send(
                    query,
                    cancellationToken);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok(result.Value);
        })
        .WithTags("Orders");
    }
}
