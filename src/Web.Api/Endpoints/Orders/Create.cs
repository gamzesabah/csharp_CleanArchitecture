using Application.Abstractions.Messaging;
using Application.Orders.Commands;
using Application.Orders.Dtos;
using SharedKernel;

namespace Web.Api.Endpoints.Orders;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("orders", async (
            CreateOrderCommand command,
            ICommandHandler<CreateOrderCommand, OrderDto> handler,
            CancellationToken cancellationToken) =>
        {
            Result<OrderDto> result =
                await handler.Handle(command, cancellationToken);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok(result.Value);
        })
        .WithTags("Orders");
    }
}
