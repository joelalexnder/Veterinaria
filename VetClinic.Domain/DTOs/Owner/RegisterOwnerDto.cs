using System.ComponentModel.DataAnnotations;

namespace VetClinic.Domain.DTOs.Pet;

public class RegisterOwnerDto
{
    [Required(ErrorMessage = "El nombre completo es obligatorio")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "El DNI es obligatorio")]
    public string Dni { get; set; } = null!;

    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    public string? Email { get; set; }

    public string? Address { get; set; }
}