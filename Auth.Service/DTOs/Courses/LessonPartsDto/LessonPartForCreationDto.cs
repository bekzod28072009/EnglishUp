namespace Auth.Service.DTOs.Courses.LessonPartsDto;

public class LessonPartForCreationDto
{
    public string Type { get; set; }
    public string Content { get; set; }
    public long LessonId { get; set; }
}
