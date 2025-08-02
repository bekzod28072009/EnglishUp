using Auth.Domain.Entities.Courses;
using Auth.Domain.Enums;
using Auth.Service.DTOs.Tests.MockTestResultsDto;

namespace Auth.Service.DTOs.Tests.MockTestsDto;

public class MockTestForViewDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public CourseLevel Level { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime ScheduledAt { get; set; }

    public ICollection<ResultForViewDto> TestResults { get; set; }
}
