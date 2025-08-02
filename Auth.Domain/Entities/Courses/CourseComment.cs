using Auth.Domain.Common;
using Auth.Domain.Entities.UserManagement;

namespace Auth.Domain.Entities.Courses;

public class CourseComment : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; } = default!;

    public long CourseId { get; set; }
    public Course Course { get; set; } = default!;

    public string Content { get; set; } = string.Empty;

}
