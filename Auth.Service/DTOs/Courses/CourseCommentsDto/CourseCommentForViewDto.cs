namespace Auth.Service.DTOs.Courses.CourseCommentsDto;

public class CourseCommentForViewDto
{
    public long Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Rating { get; set; }
    public long UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
