using Application.Products.Commands;
using MediatR;
using SharedKernel;

namespace Web.Api.Endpoints.Products;

public sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("products", async (
            CreateProductCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            Result<Guid> result =
                await sender.Send(
                    command,
                    cancellationToken);

            if (result.IsFailure)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok(result.Value);
        })
        .WithTags("Products");
    }
}
