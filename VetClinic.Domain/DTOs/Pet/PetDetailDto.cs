using VetClinic.Domain.DTOs.MedicalRecord;

namespace VetClinic.Domain.DTOs.Pet;

public class PetDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Species { get; set; } = null!;
    public string? Breed { get; set; }
    public DateOnly? BirthDate { get; set; }
    public decimal? Weight { get; set; }
    public string? Sex { get; set; }
    public string? Color { get; set; }
    public string? GeneralHealthStatus { get; set; }
    public string OwnerName { get; set; } = null!;
    public MedicalRecordDto? LastMedicalRecord { get; set; }
}