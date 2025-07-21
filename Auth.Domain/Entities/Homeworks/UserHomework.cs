using Auth.Domain.Common;
using Auth.Domain.Entities.UserManagement;

namespace Auth.Domain.Entities.Homeworks;

public class UserHomework : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; } = default!;
    public long HomeworkId { get; set; }
    public Homework Homework { get; set; } = default!;
    public string Answer { get; set; } = string.Empty;
    public int Score { get; set; }
    public bool IsCompleted { get; set; }
}
