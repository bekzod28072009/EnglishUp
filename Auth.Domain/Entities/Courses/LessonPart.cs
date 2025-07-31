using Auth.Domain.Common;

namespace Auth.Domain.Entities.Courses;

public class LessonPart : Auditable
{
    public string Content { get; set; } = string.Empty; // video URL
    public long LessonId { get; set; }
    public Lesson Lesson { get; set; } = default!;
}
