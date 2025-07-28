﻿using Auth.Domain.Enums;

namespace Auth.Service.DTOs.Tests.MockTestsDto;

public class MockTestForUpdateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public CourseLevel Level { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime ScheduledAt { get; set; }
}
