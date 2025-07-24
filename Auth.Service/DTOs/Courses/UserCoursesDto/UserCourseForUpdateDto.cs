namespace Auth.Service.DTOs.Courses.UserCoursesDto;

public class UserCourseForUpdateDto
{
    public long UserId { get; set; }
    public long CourseId { get; set; }
    public bool? IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}
