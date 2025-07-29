namespace Auth.Service.DTOs.Homeworks.HomeworksDto;

public class HomeworkForViewDto
{
    public long Id { get; set; }
    public string Question { get; set; }
    public long LessonId { get; set; }           // Include this to identify the lesson
    public string LessonTitle { get; set; } = string.Empty;  // Optional, for display
}
