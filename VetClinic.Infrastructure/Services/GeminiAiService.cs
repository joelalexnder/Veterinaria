using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using VetClinic.Domain.Ports.Services;

namespace VetClinic.Infrastructure.Services;

public class GeminiAiService : IAiRecommendationService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiAiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["GeminiAi:ApiKey"] ?? throw new ArgumentNullException("Falta configurar la API Key de Gemini");
    }

    public async Task<string> GenerateHealthSummaryAsync(string medicalHistoryText)
    {
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.1-flash-lite-preview:generateContent?key={_apiKey}";

        var prompt = $"Actua como un veterinario experto. Analiza los siguientes datos de la mascota y genera un breve resumen de salud (2 líneas) y 3 recomendaciones preventivas en viñetas. Datos: {medicalHistoryText}";

        var payload = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = prompt } } }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
            throw new Exception("Error al conectar con el servicio de IA.");

        var responseData = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(responseData);
        
        var resultText = document.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return resultText ?? "No se pudo generar el análisis.";
    }
    
}