namespace VetClinic.Domain.DTOs.Auth;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public bool? IsActive { get; set; }
}