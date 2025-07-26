namespace Auth.Service.DTOs.Users;

public class UserForUpdateDto
{
    public string? FullName { get; set; }
    public string? Email { get; set; }

    // Passwords
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmPassword { get; set; }
}
