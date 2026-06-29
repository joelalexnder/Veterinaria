using System.Diagnostics;

namespace Veterinaria.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        var method = context.Request.Method;
        var path = context.Request.Path;
        var user = context.User?.Identity?.Name ?? "Anónimo";

        _logger.LogInformation(
            "[VetClinic] {Method} {Path} | Usuario: {User} | Inicio: {Time}",
            method, path, user, DateTime.UtcNow);

        await _next(context);

        stopwatch.Stop();

        _logger.LogInformation(
            "[VetClinic] {Method} {Path} | Status: {StatusCode} | Duración: {Ms}ms",
            method, path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
    }
}