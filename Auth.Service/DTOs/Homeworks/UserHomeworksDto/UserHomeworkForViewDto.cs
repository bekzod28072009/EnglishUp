namespace Auth.Service.DTOs.Homeworks.UserHomeworksDto;

public class UserHomeworkForViewDto
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string Answer { get; set; }
    public int Score { get; set; }
    public bool IsCompleted { get; set; }
}
