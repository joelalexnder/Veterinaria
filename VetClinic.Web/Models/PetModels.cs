namespace VetClinic.Web.Models;

public class PetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Species { get; set; } = "";
    public string? Breed { get; set; }
    public DateOnly? BirthDate { get; set; }
    public decimal? Weight { get; set; }
    public string? Sex { get; set; }
    public string? Color { get; set; }
    public string OwnerName { get; set; } = "";
}

public class RegisterPetRequest
{
    public int OwnerId { get; set; }
    public string Name { get; set; } = "";
    public string Species { get; set; } = "";
    public string? Breed { get; set; }
    public DateOnly? BirthDate { get; set; }
    public decimal? Weight { get; set; }
    public string? Sex { get; set; }
    public string? Color { get; set; }
    public string? GeneralHealthStatus { get; set; }
}