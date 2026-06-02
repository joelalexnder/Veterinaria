namespace VetClinic.Domain.DTOs.Specialist;

public class SpecialistDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string? SpecialtyName { get; set; }
    public string ServiceAreaName { get; set; } = null!;
}