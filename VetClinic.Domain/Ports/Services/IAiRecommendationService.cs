namespace VetClinic.Domain.Ports.Services;

public interface IAiRecommendationService
{
    Task<string> GenerateHealthSummaryAsync(string medicalHistoryText);
}