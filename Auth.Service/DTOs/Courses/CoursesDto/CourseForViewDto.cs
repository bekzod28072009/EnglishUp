﻿using Auth.Service.DTOs.Courses.CourseCommentsDto;

namespace Auth.Service.DTOs.Courses.CoursesDto;

public class CourseForViewDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public long LevelId { get; set; }
    public string LevelName { get; set; } = string.Empty;
    public List<CourseCommentForViewDto> Comments { get; set; } = new();
}
