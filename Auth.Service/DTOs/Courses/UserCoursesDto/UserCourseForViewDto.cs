namespace Auth.Service.DTOs.Courses.UserCoursesDto;

public class UserCourseForViewDto
{
    public long Id { get; set; }
    public string UserFullName { get; set; }
    public string CourseTitle { get; set; }
    public string? CommentText { get; set; }
    public int? Rating { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}
