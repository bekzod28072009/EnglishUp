namespace Auth.Service.DTOs.Roles;

public class RoleForViewDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public long? UserCount { get; set; }
    public object? RolePermissions { get; set; }
}
