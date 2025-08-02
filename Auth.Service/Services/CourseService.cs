using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.Gamification;
using Auth.Service.DTOs.Courses.CoursesDto;
using Auth.Service.Exceptions;
using Auth.Service.Helpers;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class CourseService : ICourseService
{
    private readonly IGenericRepository<Course> courseRepository;
    private readonly IGenericRepository<CourseLevel> courseLevelRepository;
    private readonly IMapper mapper;

    public CourseService(IGenericRepository<Course> repository,
        IMapper mapper, IGenericRepository<CourseLevel> repository1)
    {
        this.mapper = mapper;
        this.courseRepository = repository;
        this.courseLevelRepository = repository1;
    }

    public async Task<IEnumerable<CourseForViewDto>> GetAllAsync(
        Expression<Func<Course, bool>> filter = null,
        string[] includes = null)
    {
        var courses = courseRepository.GetAll(filter, includes);
        var courseViews = mapper.Map<IEnumerable<CourseForViewDto>>(courses);

        foreach (var courseView in courseViews)
        {
            var level = await courseLevelRepository.GetAsync(l => l.Id == courseView.LevelId);
            courseView.LevelName = level?.Name ?? "Unknown";
        }

        return courseViews;
    }

    public async Task<CourseForViewDto> GetAsync(
        Expression<Func<Course, bool>> filter,
        string[] includes = null)
    {
        var course = await courseRepository.GetAsync(filter, includes)
            ?? throw new HttpStatusCodeException(404, "Course not found");

        var dto = mapper.Map<CourseForViewDto>(course);
        var level = await courseLevelRepository.GetAsync(l => l.Id == course.LevelId);
        dto.LevelName = level?.Name ?? "Unknown";

        return dto;
    }

    public async Task<CourseForViewDto> CreateAsync(CourseForCreationDto dto)
    {
        var level = await courseLevelRepository.GetAsync(l => l.Id == dto.LevelId)
            ?? throw new HttpStatusCodeException(404,"Course level not found");

        var course = mapper.Map<Course>(dto);
        await courseRepository.CreateAsync(course);
        await courseRepository.SaveChangesAsync();

        var result = mapper.Map<CourseForViewDto>(course);
        result.LevelName = level.Name;
        return result;
    }

    public async Task<CourseForViewDto> UpdateAsync(long id, CourseForUpdateDto dto)
    {
        var course = await courseRepository.GetAsync(c => c.Id == id)
        ?? throw new HttpStatusCodeException(404, "Course not found");

        if (dto.LevelId.HasValue)
        {
            var level = await courseLevelRepository.GetAsync(l => l.Id == dto.LevelId.Value)
                ?? throw new HttpStatusCodeException(404, "Course level not found");
            course.LevelId = dto.LevelId.Value;
        }

        if (!string.IsNullOrEmpty(dto.Title))
            course.Title = dto.Title;

        if (!string.IsNullOrEmpty(dto.Description))
            course.Description = dto.Description;

        courseRepository.Update(course);
        await courseRepository.SaveChangesAsync();

        var result = mapper.Map<CourseForViewDto>(course);
        result.LevelName = (await courseLevelRepository.GetAsync(l => l.Id == course.LevelId))?.Name ?? "Unknown";
        return result;
    }

    public async Task<bool> DeleteAsync(Expression<Func<Course, bool>> filter)
    {
        var course = await courseRepository.GetAsync(filter)
            ?? throw new HttpStatusCodeException(404, "Course not found");

        var isDeleted = await courseRepository.DeleteAsync(course);
        if (isDeleted)
            await courseRepository.SaveChangesAsync();

        return isDeleted;
    }
}
