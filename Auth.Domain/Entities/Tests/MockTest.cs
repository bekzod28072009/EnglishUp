using Auth.Domain.Common;
using Auth.Domain.Enums;

namespace Auth.Domain.Entities.Tests;

public class MockTest : Auditable
{
    public string Title { get; set; }
    public string Description { get; set; }
    public CourseLevel Level { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime ScheduledAt { get; set; }

    // Navigation property
    public ICollection<TestResult> TestResults { get; set; }
}
