namespace Auth.Service.DTOs.Gamification.StreaksDto;

public class StreakForViewDto
{
    public long Id { get; set; }
    public string UserFullName { get; set; }
    public int DaysInRow { get; set; }
    public DateTime LastActive { get; set; }
}
