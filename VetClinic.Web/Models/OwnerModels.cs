using System.ComponentModel.DataAnnotations;

namespace VetClinic.Web.Models;

public class OwnerDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Dni { get; set; } = "";
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public List<string> PetNames { get; set; } = new();
}

public class RegisterOwnerRequest
{
    [Required(ErrorMessage = "El nombre completo es obligatorio")]
    [RegularExpression(@"^(?=.*[a-zA-ZÀ-ÿ]).+$",
        ErrorMessage = "El nombre debe contener al menos una letra")]
    public string FullName { get; set; } = "";

    [Required(ErrorMessage = "El DNI es obligatorio")]
    [StringLength(8,
        MinimumLength = 8,
        ErrorMessage = "El DNI debe tener exactamente 8 dígitos")]
    [RegularExpression(@"^\d{8}$",
        ErrorMessage = "El DNI solo debe contener números")]
    public string Dni { get; set; } = "";

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [StringLength(9,
        MinimumLength = 9,
        ErrorMessage = "El teléfono debe tener 9 dígitos")]
    [RegularExpression(@"^\d{9}$",
        ErrorMessage = "El teléfono solo debe contener números")]
    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "Email inválido")]
    public string? Email { get; set; }

    public string? Address { get; set; }
}