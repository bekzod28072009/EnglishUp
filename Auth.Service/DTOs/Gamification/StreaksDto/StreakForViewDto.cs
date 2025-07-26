namespace Auth.Service.DTOs.Gamification.StreaksDto;

public class StreakForViewDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateOnly LastActivityDate { get; set; }
}
