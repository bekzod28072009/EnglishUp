using Auth.Domain.Common;
using Auth.Domain.Entities.UserManagement;

namespace Auth.Domain.Entities.Gamification;

public class Streak : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; } = default!;
    public int DaysInRow { get; set; }
    public DateTime LastActive { get; set; }
}
