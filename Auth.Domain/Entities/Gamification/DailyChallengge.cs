using Auth.Domain.Common;

namespace Auth.Domain.Entities.Gamification;

public class DailyChallengge : Auditable
{
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}
