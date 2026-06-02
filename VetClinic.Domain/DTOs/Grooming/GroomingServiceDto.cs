namespace VetClinic.Domain.DTOs.Grooming;

public class GroomingServiceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal BasePrice { get; set; }
    public string Species { get; set; } = null!;
    public string PetSize { get; set; } = null!;
    public string? CoatType { get; set; }
}