namespace Auth.Service.DTOs.Courses.UserCoursesDto;

public class UserCourseForCreationDto
{
    public long UserId { get; set; }
    public string? UserComment { get; set; }
    public long CourseId { get; set; }
}
