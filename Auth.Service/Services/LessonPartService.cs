using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Courses;
using Auth.Service.DTOs.Courses.LessonPartsDto;
using Auth.Service.Interfaces;
using AutoMapper;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class LessonPartService : ILessonPartService
{
    private readonly IGenericRepository<LessonPart> repository;
    private readonly IMapper mapper;

    public LessonPartService(IGenericRepository<LessonPart> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<LessonPartForViewDto>> GetAllAsync(string lang, string[] includes = null)
    {
        var lessonParts = repository.GetAll(null, includes);
        // Optional: filter by lang if needed
        return mapper.Map<IEnumerable<LessonPartForViewDto>>(lessonParts);
    }

    public async Task<LessonPartForViewDto> GetAsync(Expression<Func<LessonPart, bool>> filter, string[] includes = null)
    {
        var lessonPart = await repository.GetAsync(filter, includes);
        return lessonPart is null ? null : mapper.Map<LessonPartForViewDto>(lessonPart);
    }

    public async Task<LessonPartForViewDto> CreateAsync(LessonPartForCreationDto dto)
    {
        var entity = mapper.Map<LessonPart>(dto);
        var created = await repository.CreateAsync(entity);
        await repository.SaveChangesAsync(); // Ensure changes are saved asynchronously
        return mapper.Map<LessonPartForViewDto>(created);
    }

    public async Task<bool> DeleteAsync(Expression<Func<LessonPart, bool>> filter)
    {
        var lessonPart = await repository.GetAsync(filter);
        if (lessonPart is null)
            return false;

        await repository.SaveChangesAsync(); // Ensure any pending changes are saved before deletion

        return await repository.DeleteAsync(filter);
    }

    public async Task<LessonPartForViewDto> UpdateAsync(long id, LessonPartForUpdateDto dto)
    {
        var existing = await repository.GetAsync(x => x.Id == id);
        if (existing is null)
            return null;

        mapper.Map(dto, existing);
        var updated = repository.Update(existing);
        await repository.SaveChangesAsync(); // Ensure changes are saved asynchronously
        return mapper.Map<LessonPartForViewDto>(updated);
    }
}
