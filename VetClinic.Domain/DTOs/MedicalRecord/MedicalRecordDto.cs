namespace VetClinic.Domain.DTOs.MedicalRecord;

public class MedicalRecordDto
{
    public int Id { get; set; }
    public int PetId { get; set; }
    public string PetName { get; set; } = null!;
    public string VeterinarianName { get; set; } = null!;
    public DateTime? ConsultDate { get; set; }
    public string Reason { get; set; } = null!;
    public string Diagnosis { get; set; } = null!;
    public string? Treatment { get; set; }
    public string? Observations { get; set; }
    public decimal? RegisteredWeight { get; set; }
}