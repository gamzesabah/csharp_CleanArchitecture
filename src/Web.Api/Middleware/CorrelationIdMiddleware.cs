using Serilog.Context;

namespace Web.Api.Middleware;

public sealed class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        const string CorrelationIdHeader =
            "X-Correlation-ID";

        string correlationId =
            context.Request.Headers[
                CorrelationIdHeader]
            .FirstOrDefault()
            ??
            Guid.NewGuid().ToString();

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[
                CorrelationIdHeader] =
                    correlationId;

            return Task.CompletedTask;
        });

        using (
            LogContext.PushProperty(
                "CorrelationId",
                correlationId))
        {
            await _next(context);
        }
    }
}
