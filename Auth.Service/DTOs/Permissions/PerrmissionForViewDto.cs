﻿using Auth.Domain.Entities.Roles;

namespace Auth.Service.DTOs.Permissions;

public class PerrmissionForViewDto
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ICollection<Role> Role { get; set; } = new List<Role>();
}
