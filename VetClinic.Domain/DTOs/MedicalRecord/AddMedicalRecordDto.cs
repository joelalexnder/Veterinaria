namespace VetClinic.Domain.DTOs.MedicalRecord;

public class AddMedicalRecordDto
{
    public int PetId { get; set; }
    public int VeterinarianId { get; set; }
    public string Reason { get; set; } = null!;
    public string Diagnosis { get; set; } = null!;
    public string? Treatment { get; set; }
    public string? Observations { get; set; }
    public decimal? RegisteredWeight { get; set; }
}