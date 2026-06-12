using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Web.Api.Endpoints.Users;

internal sealed class Me : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "users/me",
            [Authorize] (
                ClaimsPrincipal user) =>
            {
                string? userId =
                    user.FindFirstValue(
                        ClaimTypes.NameIdentifier);

                return Results.Ok(new
                {
                    Message = "Authorized",
                    UserId = userId
                });
            })
            .WithTags("Users");
    }
}
