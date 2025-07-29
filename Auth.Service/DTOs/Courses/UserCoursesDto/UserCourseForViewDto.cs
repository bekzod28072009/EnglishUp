namespace Auth.Service.DTOs.Courses.UserCoursesDto;

public class UserCourseForViewDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public long CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public string? CommentText { get; set; }
    public int? Rating { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}
