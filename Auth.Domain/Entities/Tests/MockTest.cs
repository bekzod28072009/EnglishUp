using Auth.Domain.Common;

namespace Auth.Domain.Entities.Tests;

public class MockTest : Auditable
{
    public string Title { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public ICollection<TestResult> Results { get; set; } = new List<TestResult>();
}
