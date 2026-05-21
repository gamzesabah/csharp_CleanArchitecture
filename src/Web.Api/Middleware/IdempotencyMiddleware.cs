using Application.Abstractions.Data;
using Domain.Idempotency;
using Microsoft.Extensions.Primitives;

namespace Web.Api.Middleware;

public sealed class IdempotencyMiddleware
{
    private readonly RequestDelegate _next;

    public IdempotencyMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        IIdempotencyRepository repository,
        IApplicationDbContext dbContext)
    {
        if (!context.Request.Headers
    .TryGetValue(
        "Idempotency-Key",
        out StringValues key))
        {
            await _next(context);

            return;
        }

        IdempotencyRecord? existingRecord =
            await repository
                .GetByKeyAsync(
                    key!,
                    CancellationToken.None);

        if (existingRecord is not null)
        {
            await context.Response
                .WriteAsync(
                    existingRecord.Response);

            return;
        }

        Stream originalBody =
            context.Response.Body;

        using MemoryStream stream = new();

        context.Response.Body =
            stream;

        await _next(context);

        stream.Position = 0;

        using StreamReader reader =
            new(stream);

        string response =
            await reader.ReadToEndAsync();

        var record =
            IdempotencyRecord.Create(
                key!,
                response);

        await repository
            .AddAsync(record);

        await dbContext
            .SaveChangesAsync(
                CancellationToken.None);

        stream.Position = 0;

        await stream.CopyToAsync(
            originalBody);

        context.Response.Body =
            originalBody;
    }
}
