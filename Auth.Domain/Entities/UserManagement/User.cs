using Auth.Domain.Common;
using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.Homeworks;
using Auth.Domain.Entities.Roles;
using Auth.Domain.Entities.Tests;

namespace Auth.Domain.Entities.UserManagement;

public class User : Auditable
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int Age { get; set; }
    public long RoleId { get; set; }
    public Role Role { get; set; } = default!;
    public ICollection<UserCourse> UserCourses { get; set; }
    public ICollection<UserHomework> UserHomeworks { get; set; }
    public ICollection<TestResult> TestResult { get; set; }
    public ICollection<PointTransaction> PointTransactions { get; set; }
}
