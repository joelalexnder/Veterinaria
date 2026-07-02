using System.Text.Json;
using System.Text.Json.Nodes;

namespace Veterinaria.Middleware;

public class ParameterValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ParameterValidationMiddleware> _logger;

    private static readonly Dictionary<(string Method, string Path), string[]> RequiredFields = new()
    {
        // ── Appointment ─────────────────────────────
        [(HttpMethods.Post, "/api/appointment")]            = ["petId", "specialistId", "serviceAreaId"],
        [(HttpMethods.Put,  "/api/appointment/cancel")]     = ["id", "cancellationReason"],
        [(HttpMethods.Put,  "/api/appointment/reschedule")] = ["id", "newDate", "newStartTime", "newEndTime"],
        [(HttpMethods.Put,  "/api/appointment/status")]     = ["id", "status"],

        // ── Auth ────────────────────────────────────
        [(HttpMethods.Post, "/api/auth/login")]             = ["email", "password"],
        [(HttpMethods.Post, "/api/auth/register")]          = ["fullName", "email", "password", "roleId"],
        [(HttpMethods.Put,  "/api/auth/update-role")]       = ["userId", "roleId"],
        [(HttpMethods.Put,  "/api/auth/change-password")]  = ["userId", "currentPassword", "newPassword"],

        // ── Grooming ────────────────────────────────
        [(HttpMethods.Post, "/api/grooming")]                = ["appointmentId"],
        [(HttpMethods.Put,  "/api/grooming/complete")]      = ["id", "finalPrice"],
        [(HttpMethods.Post, "/api/grooming/package")]       = ["name", "specialPrice"],

        // ── Owner ───────────────────────────────────
        [(HttpMethods.Post, "/api/owner")]                  = ["fullName", "dni"],

        // ── MedicalRecord ───────────────────────────
        [(HttpMethods.Post, "/api/medicalrecord")]          = ["petId", "veterinarianId", "reason", "diagnosis"],

        // ── Pet ─────────────────────────────────────
        [(HttpMethods.Post, "/api/pet")]                    = ["ownerId", "name", "species"],

        // ── ServiceArea ─────────────────────────────
        [(HttpMethods.Post, "/api/servicearea")]            = ["name"],

        // ── Specialist ──────────────────────────────
        [(HttpMethods.Post, "/api/specialist")]             = ["userId", "serviceAreaId"],

        // ── Vaccination ─────────────────────────────
        [(HttpMethods.Post, "/api/vaccination")]             = ["petId", "vaccineId", "veterinarianId", "applicationDate", "nextBoosterDate"],
        [(HttpMethods.Post, "/api/vaccination/catalog")]    = ["name", "species"],
        [(HttpMethods.Put,  "/api/vaccination/schedule")]   = ["id", "nextBoosterDate"],
    };

    public ParameterValidationMiddleware(RequestDelegate next, ILogger<ParameterValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if ((context.Request.Method == HttpMethods.Post ||
             context.Request.Method == HttpMethods.Put) &&
            context.Request.ContentType?.Contains("application/json") == true)
        {
            var path = context.Request.Path.Value?.ToLower().TrimEnd('/') ?? "";
            var method = context.Request.Method;

            var matchedKey = RequiredFields.Keys
                .FirstOrDefault(k => k.Method == method && k.Path == path);

            if (matchedKey != default)
            {
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                var errors = ValidateRequiredFields(body, RequiredFields[matchedKey]);

                if (errors.Any())
                {
                    _logger.LogWarning("Validación fallida en {Method} {Path}: {Errors}",
                        method, path, string.Join(", ", errors));

                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";

                    var response = new
                    {
                        status = 400,
                        message = "Parámetros inválidos o faltantes",
                        errors,
                        timestamp = DateTime.UtcNow
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    return;
                }
            }
        }

        await _next(context);
    }

    private static List<string> ValidateRequiredFields(string body, string[] requiredFields)
    {
        var errors = new List<string>();

        try
        {
            var json = JsonNode.Parse(body);
            if (json is null)
            {
                errors.Add("El cuerpo de la solicitud está vacío o no es JSON válido.");
                return errors;
            }

            foreach (var field in requiredFields)
            {
                var value = json[field];

                if (value is null)
                {
                    errors.Add($"El campo '{field}' es obligatorio.");
                    continue;
                }

                var strValue = value.ToString();
                if (string.IsNullOrWhiteSpace(strValue) || strValue == "0")
                    errors.Add($"El campo '{field}' no puede estar vacío.");
            }
        }
        catch (JsonException)
        {
            errors.Add("El cuerpo de la solicitud no es un JSON válido.");
        }

        return errors;
    }
}