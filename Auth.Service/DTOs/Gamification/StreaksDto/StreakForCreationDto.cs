namespace Auth.Service.DTOs.Gamification.StreaksDto;

public class StreakForCreationDto
{
    public long UserId { get; set; }
    public int DaysInRow { get; set; }
    public DateTime LastActive { get; set; }
}
