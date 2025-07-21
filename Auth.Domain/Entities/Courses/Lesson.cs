using Auth.Domain.Common;
using Auth.Domain.Entities.Homeworks;

namespace Auth.Domain.Entities.Courses;

public class Lesson : Auditable
{
    public string Title { get; set; } = string.Empty;
    public long CourseId { get; set; }
    public Course Course { get; set; } = default!;
    public ICollection<LessonPart> Parts { get; set; }
    public ICollection<Homework> Homeworks { get; set; }
}
