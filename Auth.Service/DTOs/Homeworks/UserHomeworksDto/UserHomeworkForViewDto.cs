namespace Auth.Service.DTOs.Homeworks.UserHomeworksDto;

public class UserHomeworkForViewDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public long HomeworkId { get; set; }
    public string HomeworkQuestion { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public int Score { get; set; }
    public bool IsCompleted { get; set; }
}
