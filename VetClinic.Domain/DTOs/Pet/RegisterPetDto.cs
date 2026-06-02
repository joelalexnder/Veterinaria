namespace VetClinic.Domain.DTOs.Pet;

public class RegisterPetDto
{
    public int OwnerId { get; set; }
    public string Name { get; set; } = null!;
    public string Species { get; set; } = null!;
    public string? Breed { get; set; }
    public DateOnly? BirthDate { get; set; }
    public decimal? Weight { get; set; }
    public string? Sex { get; set; }
    public string? Color { get; set; }
    public string? GeneralHealthStatus { get; set; }
}