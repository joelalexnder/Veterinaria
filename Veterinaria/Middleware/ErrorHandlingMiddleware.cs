using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using VetClinic.Domain.Exceptions;

namespace Veterinaria.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning("Recurso no encontrado: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.NotFound, "Recurso no encontrado", ex.Message);
        }
        catch (ConflictException ex)
        {
            _logger.LogWarning("Conflicto de negocio: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.Conflict, "Conflicto", ex.Message);
        }
        catch (UnauthorizedException ex)
        {
            _logger.LogWarning("No autorizado: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.Unauthorized, "No autorizado", ex.Message);
        }
        catch (ForbiddenException ex)
        {
            _logger.LogWarning("Acceso prohibido: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.Forbidden, "Acceso prohibido", ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Acceso no autorizado: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.Unauthorized, "No autorizado", ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Recurso no encontrado: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.NotFound, "Recurso no encontrado", ex.Message);
        }
        catch (DbUpdateException ex)
        {
            var pgMessage = ex.InnerException?.Message ?? ex.Message;
            _logger.LogWarning("Conflicto de base de datos: {Message}", pgMessage);

            if (pgMessage.Contains("duplicate key value violates unique constraint", StringComparison.OrdinalIgnoreCase))
            {
                var friendlyField = ExtractDuplicateField(pgMessage);
                await WriteResponse(context, HttpStatusCode.Conflict,
                    "Conflicto",
                    $"Ya existe un registro con ese {friendlyField}. Verifica los datos ingresados.");
            }
            else if (pgMessage.Contains("violates foreign key constraint", StringComparison.OrdinalIgnoreCase))
            {
                await WriteResponse(context, HttpStatusCode.Conflict,
                    "Conflicto",
                    "No se puede eliminar este registro porque tiene datos relacionados (citas, historial médico, vacunaciones, etc.). Elimina o reasigna esos datos primero.");
            }
            else
            {
                await WriteResponse(context, HttpStatusCode.Conflict,
                    "Conflicto",
                    "No se pudo completar la operación debido a un conflicto con los datos existentes.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error interno del servidor");
            await WriteResponse(context, HttpStatusCode.InternalServerError,
                "Error interno del servidor", ex.Message);
        }
    }

    private static string ExtractDuplicateField(string pgMessage)
    {
        if (pgMessage.Contains("dni", StringComparison.OrdinalIgnoreCase)) return "DNI";
        if (pgMessage.Contains("email", StringComparison.OrdinalIgnoreCase)) return "email";
        if (pgMessage.Contains("user_id", StringComparison.OrdinalIgnoreCase)) return "usuario (ya es especialista)";
        return "dato";
    }

    private static async Task WriteResponse(HttpContext context, HttpStatusCode statusCode,
        string message, string details)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = (int)statusCode,
            message,
            details,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}