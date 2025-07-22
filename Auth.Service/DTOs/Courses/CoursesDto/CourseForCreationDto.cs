using Auth.Domain.Enums;

namespace Auth.Service.DTOs.Courses.CoursesDto;

public class CourseForCreationDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public CourseLevel Level { get; set; }
}
