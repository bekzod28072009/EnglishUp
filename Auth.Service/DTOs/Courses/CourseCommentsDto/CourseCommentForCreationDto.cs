namespace Auth.Service.DTOs.Courses.CourseCommentsDto;

public class CourseCommentForCreationDto
{
    public long UserId { get; set; }
    public long CourseId { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
}
