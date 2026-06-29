using System.Text.Json;
using System.Text.Json.Nodes;

namespace Veterinaria.Middleware;

public class ParameterValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ParameterValidationMiddleware> _logger;

    // Campos obligatorios por endpoint
    private static readonly Dictionary<string, string[]> RequiredFields = new()
    {
        // Appointments
        ["/api/appointments"]                  = ["petId", "specialistId", "serviceAreaId"],
        
        // Owner
        ["/api/owner"]                         = ["fullName", "dni"],
        
        // MedicalRecord
        ["/api/medicalrecord"]                 = ["petId", "veterinarianId", "reason", "diagnosis"],
        
        // Auth
        ["/api/auth/register"]                 = ["fullName", "email", "password"],
        
        // Grooming
        ["/api/grooming/package"]              = ["name", "specialPrice"],
        
        // Vaccination
        ["/api/vaccination/catalog"]           = ["name", "species"],
    };

    public ParameterValidationMiddleware(RequestDelegate next, 
        ILogger<ParameterValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Solo validar POST y PUT con JSON
        if ((context.Request.Method == HttpMethods.Post || 
             context.Request.Method == HttpMethods.Put) &&
            context.Request.ContentType?.Contains("application/json") == true)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";
            
            // Buscar si este endpoint tiene campos requeridos
            var matchedKey = RequiredFields.Keys
                .FirstOrDefault(k => path.StartsWith(k.ToLower()));

            if (matchedKey != null)
            {
                // Leer el body
                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, 
                    leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; // resetear para el siguiente middleware

                var errors = ValidateRequiredFields(body, RequiredFields[matchedKey]);

                if (errors.Any())
                {
                    _logger.LogWarning(
                        "Validación fallida en {Path}: {Errors}", 
                        path, string.Join(", ", errors));

                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "application/json";

                    var response = new
                    {
                        status = 400,
                        message = "Parámetros inválidos o faltantes",
                        errors,
                        timestamp = DateTime.UtcNow
                    };

                    await context.Response.WriteAsync(
                        JsonSerializer.Serialize(response));
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

                // Campo ausente
                if (value is null)
                {
                    errors.Add($"El campo '{field}' es obligatorio.");
                    continue;
                }

                // Campo presente pero vacío (string vacío o 0)
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