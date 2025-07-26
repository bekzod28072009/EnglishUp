namespace Auth.Service.DTOs.Gamification.StreakLogDto;

public class StreakLogForViewDto
{
    public long Id { get; set; }
    public long StreakId { get; set; }
    public DateOnly ActivityDate { get; set; }
    public string Description { get; set; }
}
