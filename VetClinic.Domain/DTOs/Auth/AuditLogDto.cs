namespace VetClinic.Domain.DTOs.Auth;
public class AuditLogDto
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string Action { get; set; } = null!;
    public string ResourceName { get; set; } = null!;
    public string? ResourceId { get; set; }
    public DateTime? Timestamp { get; set; }
    public string? Details { get; set; }
}