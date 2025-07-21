using Auth.Domain.Common;

namespace Auth.Domain.Entities.Courses;

public class LessonPart : Auditable
{
    public string Type { get; set; } = string.Empty; // Vocabulary, Grammar, etc.
    public string Content { get; set; } = string.Empty;
    public long LessonId { get; set; }
    public Lesson Lesson { get; set; } = default!;
}
