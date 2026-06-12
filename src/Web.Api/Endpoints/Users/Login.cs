using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Web.Api.Endpoints.Users;

internal sealed class Login : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapPost(
            "users/login",
            (
                IConfiguration configuration) =>
            {
                var userId = Guid.NewGuid();

                Claim[] claims =
                [
                    new(
                        ClaimTypes.NameIdentifier,
                        userId.ToString())
                ];

                SymmetricSecurityKey key =
                    new(
                        Encoding.UTF8.GetBytes(
                            configuration["Jwt:Secret"]!));

                SigningCredentials credentials =
                    new(
                        key,
                        SecurityAlgorithms.HmacSha256);

                JwtSecurityToken token = 
                    new(
                        issuer: "clean-architecture", 
                        audience: "developers", 
                        claims: claims, 
                        expires: DateTime.UtcNow.AddHours(1), 
                        signingCredentials: credentials);

                string jwt =
                    new JwtSecurityTokenHandler()
                        .WriteToken(token);

                return Results.Ok(new 
                { 
                    Token = jwt, 
                    Issuer = configuration["Jwt:Issuer"], 
                    Audience = configuration["Jwt:Audience"] 
                });
            })
            .WithTags("Users");
    }
}
