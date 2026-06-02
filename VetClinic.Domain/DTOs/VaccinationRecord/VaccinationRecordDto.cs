namespace VetClinic.Domain.DTOs.VaccinationRecord;

public class VaccinationRecordDto
{
    public int Id { get; set; }
    public string PetName { get; set; } = null!;
    public string VaccineName { get; set; } = null!;
    public string VeterinarianName { get; set; } = null!;
    public string? BatchNumber { get; set; }
    public DateOnly? ApplicationDate { get; set; }
    public DateOnly? NextBoosterDate { get; set; }
    public int DaysOverdue { get; set; }
}