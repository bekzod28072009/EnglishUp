using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Courses;
using Auth.Service.DTOs.Courses.LessonsDto;
using Auth.Service.Interfaces;
using AutoMapper;
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

    public async Task<IEnumerable<LessonForViewDto>> GetAllAsync(string lang, string[] includes = null)
    {
        var lessons =  repository.GetAll(null, includes);
        // You can add lang filtering here if needed
        return mapper.Map<IEnumerable<LessonForViewDto>>(lessons);
    }

    public async Task<LessonForViewDto> GetAsync(Expression<Func<Lesson, bool>> filter, string[] includes = null)
    {
        var lesson = await repository.GetAsync(filter, includes);
        return lesson is null
            ? null
            : mapper.Map<LessonForViewDto>(lesson);
    }

    public async Task<LessonForViewDto> CreateAsync(LessonForCreationDto dto)
    {
        var lesson = mapper.Map<Lesson>(dto);
        var created = await repository.CreateAsync(lesson);
        await repository.SaveChangesAsync(); // Ensure changes are saved asynchronously
        return mapper.Map<LessonForViewDto>(created);
    }

    public async Task<bool> DeleteAsync(Expression<Func<Lesson, bool>> filter)
    {
        var lesson = await repository.GetAsync(filter);
        if (lesson is null)
            return false;


        await repository.DeleteAsync(lesson);
        await repository.SaveChangesAsync();
        return true;
    }

    public async Task<LessonForViewDto> UpdateAsync(long id, LessonForUpdateDto dto)
    {
        var existing = await repository.GetAsync(l => l.Id == id);
        if (existing is null)
            return null;

        mapper.Map(dto, existing);
        var updated = repository.Update(existing);
        await repository.SaveChangesAsync();
        return mapper.Map<LessonForViewDto>(updated);
    }
}
