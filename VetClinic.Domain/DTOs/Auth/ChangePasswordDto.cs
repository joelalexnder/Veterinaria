namespace VetClinic.Domain.DTOs.Auth;

public class ChangePasswordDto
{
    public int UserId { get; set; }
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}