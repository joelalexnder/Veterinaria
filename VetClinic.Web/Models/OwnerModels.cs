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
    public string FullName { get; set; } = "";
    public string Dni { get; set; } = "";
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}