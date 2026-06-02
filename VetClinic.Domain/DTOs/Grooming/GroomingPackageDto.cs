namespace VetClinic.Domain.DTOs.Grooming;

public class GroomingPackageDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal SpecialPrice { get; set; }
}