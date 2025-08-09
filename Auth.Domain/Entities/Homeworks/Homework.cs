using Auth.Domain.Common;
using Auth.Domain.Entities.Courses;

namespace Auth.Domain.Entities.Homeworks;

public class Homework : Auditable
{
    public string ImagePath { get; set; }
    public string Question { get; set; } = string.Empty;
    public string Colour { get; set; }
    public long LessonId { get; set; }
    public Lesson Lesson { get; set; } = default!;
}
