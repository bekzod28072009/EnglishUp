using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Courses;
using Auth.Service.DTOs.Courses.CourseLevelsDto;
using Auth.Service.DTOs.Courses.LessonsDto;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class LessonService : ILessonService
{
    private readonly IGenericRepository<Lesson> repository;
    private readonly IMapper mapper;

    public LessonService(IGenericRepository<Lesson> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<LessonForViewDto>> GetAllAsync(Expression<Func<Lesson, bool>> filter = null, string[] includes = null)
    {
        var lessons = await repository.GetAll(filter, includes).ToListAsync();
        // You can add lang filtering here if needed
        return mapper.Map<IEnumerable<LessonForViewDto>>(lessons);
    }

    public async Task<LessonForViewDto> GetAsync(long id)
    {
        var lesson = await repository.GetAsync(c => c.Id == id)
            ?? throw new HttpStatusCodeException(404, "Course level not found");

        return mapper.Map<LessonForViewDto>(lesson);
    }

    public async Task<LessonForViewDto> CreateAsync(LessonForCreationDto dto)
    {
        var lesson = mapper.Map<Lesson>(dto);
        var created = await repository.CreateAsync(lesson);
        await repository.SaveChangesAsync(); // Ensure changes are saved asynchronously
        return mapper.Map<LessonForViewDto>(created);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var level = await repository.GetAsync(c => c.Id == id)
            ?? throw new HttpStatusCodeException(404, "Lesson not found");

        await repository.DeleteAsync(level);
        await repository.SaveChangesAsync();

        return true;
    }

    public async Task<LessonForViewDto> UpdateAsync(long id, LessonForUpdateDto dto)
    {
        var existing = await repository.GetAsync(l => l.Id == id);
        if (existing is null)
            return null;

        existing = mapper.Map(dto, existing);

        var updated = repository.Update(existing);
        await repository.SaveChangesAsync();
        return mapper.Map<LessonForViewDto>(updated);
    }
}
