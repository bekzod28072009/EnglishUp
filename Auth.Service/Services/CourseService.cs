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
    private readonly IGenericRepository<Course> repository;
    private readonly IMapper mapper;

    public CourseService(IGenericRepository<Course> repository,
        IMapper mapper)
    {
        this.mapper = mapper;
        this.repository = repository;   
    }

    public async Task<IEnumerable<CourseForViewDto>> GetAllAsync(Expression<Func<Course, bool>> filter = null, string[] includes = null)
    {        
        var courses = await repository.GetAll(filter, includes).ToListAsync();
        return mapper.Map<IEnumerable<CourseForViewDto>>(courses);
    }

    public async Task<CourseForViewDto> GetAsync(Expression<Func<Course, bool>> filter, string[] includes = null)
    {
        var course = await repository.GetAsync(filter, includes);
        return course is null? null : mapper.Map<CourseForViewDto>(course);
    }

    public async Task<CourseForViewDto> CreateAsync(CourseForCreationDto dto)
    {
        var course = mapper.Map<Course>(dto);
        var created = await repository.CreateAsync(course);
        await repository.SaveChangesAsync(); // Ensure changes are saved asynchronously
        return mapper.Map<CourseForViewDto>(created);
    }

    public async Task<bool> DeleteAsync(Expression<Func<Course, bool>> filter)
    {
        var course = await repository.GetAsync(filter);
        if (course is null)
            return false;


        
        
        await repository.DeleteAsync(course);
        await repository.SaveChangesAsync(); 
        return true; 

    }

    public async Task<CourseForViewDto> UpdateAsync(long id, CourseForUpdateDto dto)
    {
        var existingCourse = await repository.GetAsync(c => c.Id == id);
        if (existingCourse is null)
            return null;

        mapper.Map(dto, existingCourse);
        var updated = repository.Update(existingCourse); // Fixed: Use the synchronous Update method instead of UpdateAsync  
        await repository.SaveChangesAsync(); // Ensure changes are saved asynchronously  
        return mapper.Map<CourseForViewDto>(updated);
    }
}
