namespace VetClinic.Domain.DTOs.Vaccine;

public class AddVaccineCatalogDto
{
    public string Name { get; set; } = null!;
    public string Species { get; set; } = null!;
    public int MinAgeDays { get; set; }
    public int BoosterIntervalDays { get; set; }
}