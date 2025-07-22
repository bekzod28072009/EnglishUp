namespace Auth.Service.DTOs.Courses.LessonsDto;

public class LessonForViewDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public long CourseId { get; set; }
    public string CourseTitle { get; set; }
}
