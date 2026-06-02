namespace VetClinic.Domain.DTOs.Pet;

public class RegisterOwnerDto
{
    public string FullName { get; set; } = null!;
    public string Dni { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}