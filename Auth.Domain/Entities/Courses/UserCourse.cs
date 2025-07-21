using Auth.Domain.Common;
using Auth.Domain.Entities.UserManagement;

namespace Auth.Domain.Entities.Courses;

public class UserCourse : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; } = default!;
    public long CourseId { get; set; }
    public Course Course { get; set; } = default!;
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}
