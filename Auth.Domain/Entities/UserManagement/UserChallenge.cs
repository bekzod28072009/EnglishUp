using Auth.Domain.Common;
using Auth.Domain.Entities.Gamification;

namespace Auth.Domain.Entities.UserManagement;

public class UserChallenge : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; } = default!;

    public long ChallengeId { get; set; }
    public DailyChallengge Challenge { get; set; } = default!;

    public DateTime CompletedAt { get; set; }
}
