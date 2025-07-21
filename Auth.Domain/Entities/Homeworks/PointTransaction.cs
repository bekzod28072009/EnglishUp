using Auth.Domain.Common;
using Auth.Domain.Entities.UserManagement;

namespace Auth.Domain.Entities.Homeworks;

public class PointTransaction : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; } = default!;
    public int Points { get; set; }
    public string Reason { get; set; } = string.Empty;
}
