namespace Auth.Service.DTOs.Courses.CourseCommentsDto;

public class CourseCommentForCreationDto
{
    public long CourseId { get; set; }
    public long UserId { get; set; }
    public string Content { get; set; } = string.Empty;
}
