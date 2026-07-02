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
    [RegularExpression(@"^(?=.*[a-zA-ZÀ-ÿ]).+$", ErrorMessage = "El nombre debe contener al menos una letra")]
    public string FullName { get; set; } = "";

    [Required(ErrorMessage = "El DNI es obligatorio")]
    public string Dni { get; set; } = "";

    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "Email inválido")]
    public string? Email { get; set; }

    public string? Address { get; set; }
}