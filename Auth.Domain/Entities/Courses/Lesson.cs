using Auth.Domain.Common;
using Auth.Domain.Entities.Homeworks;

namespace Auth.Domain.Entities.Courses;

public class Lesson : Auditable
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; }
    public string Content { get; set; } = string.Empty; // video URL
    public long CourseId { get; set; }
    public Course Course { get; set; } = default!;
    public ICollection<Homework> Homeworks { get; set; } = new List<Homework>();
}
