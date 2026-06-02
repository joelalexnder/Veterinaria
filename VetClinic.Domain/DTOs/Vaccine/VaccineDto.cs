namespace VetClinic.Domain.DTOs.Vaccine;

public class VaccineDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Species { get; set; } = null!;
    public int MinAgeDays { get; set; }
    public int BoosterIntervalDays { get; set; }
}