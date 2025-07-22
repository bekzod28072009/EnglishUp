namespace Auth.Service.DTOs.Users;

public class UserForUpdateDto
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public int? Age { get; set; }
    public long? RoleId { get; set; }
}
