namespace Auth.Service.DTOs.Courses.UserCoursesDto;

public class UserCourseForUpdateDto
{
    public string? UserComment { get; set; }
    public bool? IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}
