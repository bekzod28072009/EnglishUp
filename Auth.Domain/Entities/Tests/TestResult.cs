using Auth.Domain.Common;
using Auth.Domain.Entities.UserManagement;

namespace Auth.Domain.Entities.Tests;

public class TestResult : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; } = default!;
    public long MockTestId { get; set; }
    public MockTest MockTest { get; set; } = default!;
    public int ListeningScore { get; set; }
    public int ReadingScore { get; set; }
    public int WritingScore { get; set; }
    public int SpeakingScore { get; set; }
}
