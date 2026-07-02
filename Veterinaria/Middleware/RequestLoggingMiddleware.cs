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

        _logger.LogInformation("[VetClinic] {Method} {Path} | Inicio: {Time}", method, path, DateTime.UtcNow);

        await _next(context);

        stopwatch.Stop();
        var user = context.User?.Identity?.Name ?? "Anónimo"; // ← recalculado DESPUÉS de auth

        _logger.LogInformation(
            "[VetClinic] {Method} {Path} | Usuario: {User} | Status: {StatusCode} | Duración: {Ms}ms",
            method, path, user, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
    }
}