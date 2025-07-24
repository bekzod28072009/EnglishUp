namespace Auth.Service.DTOs.Roles;

public class RoleForViewGetDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public List<string> Users { get; set; }
    public Object? Permissions { get; set; }
    public string? CreatedBy { get; set; }
}
