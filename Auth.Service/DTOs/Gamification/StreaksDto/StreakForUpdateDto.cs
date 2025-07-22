namespace Auth.Service.DTOs.Gamification.StreaksDto;

public class StreakForUpdateDto
{
    public int? DaysInRow { get; set; }
    public DateTime? LastActive { get; set; }
}
