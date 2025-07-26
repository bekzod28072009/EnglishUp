using Auth.Domain.Common;

namespace Auth.Domain.Entities.Gamification;

public class StreakLog : Auditable
{
    public long StreakId { get; set; }
    public Streak Streak { get; set; }

    public DateOnly ActivityDate { get; set; }
    public string Description { get; set; }
}
