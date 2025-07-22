namespace Auth.Service.DTOs.Homeworks.UserHomeworksDto;

public class UserHomeworkForCreationDto
{
    public long UserId { get; set; }
    public long HomeworkId { get; set; }
    public string Answer { get; set; }
}
