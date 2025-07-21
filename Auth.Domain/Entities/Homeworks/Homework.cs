using Auth.Domain.Common;
using Auth.Domain.Entities.Courses;

namespace Auth.Domain.Entities.Homeworks;

public class Homework : Auditable
{
    public string Question { get; set; } = string.Empty;
    public long LessonId { get; set; }
    public Lesson Lesson { get; set; } = default!;
}
