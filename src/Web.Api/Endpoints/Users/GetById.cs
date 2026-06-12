using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Web.Api.Endpoints.Users;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "users/{id:guid}",
            [Authorize] (
                Guid id,
                ClaimsPrincipal user) =>
            {
                string? userId =
                    user.FindFirstValue(
                        ClaimTypes.NameIdentifier);

                if (userId != id.ToString())
                {
                    return Results.Forbid();
                }

                return Results.Ok(new
                {
                    Message = "You can access your own data",
                    UserId = userId
                });
            })
            .WithTags("Users");
    }
}
