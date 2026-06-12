using Application.Orders.Commands;
using Application.Orders.Dtos;
using MediatR;
using SharedKernel;

namespace Web.Api.Endpoints.Orders;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("orders", async (
            CreateOrderCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            Result<OrderDto> result =
                await sender.Send(
                    command,
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
