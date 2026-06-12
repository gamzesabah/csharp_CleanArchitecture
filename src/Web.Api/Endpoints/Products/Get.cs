using Application.Products.Dtos;
using Application.Products.Queries;
using MediatR;
using SharedKernel;
using Web.Api.Endpoints;

namespace Web.Api.Endpoints.Products;

public sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products", async (
            [AsParameters] GetProductListQuery query,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            Result<PagedResult<ProductDto>> result =
                await sender.Send(
                    query,
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
