using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.Homeworks;
using Auth.Service.DTOs.Homeworks.HomeworksDto;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class HomeworkService : IHomeworkService
{
    private readonly IGenericRepository<Homework> repository;
    private readonly IMapper mapper;

    public HomeworkService(IGenericRepository<Homework> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<HomeworkForViewDto> CreateAsync(HomeworkForCreationDto dto)
    {
        var homework = mapper.Map<Homework>(dto);
        await repository.CreateAsync(homework);
        await repository.SaveChangesAsync();
        return mapper.Map<HomeworkForViewDto>(homework);
    }

    public async Task<bool> DeleteAsync(Expression<Func<Homework, bool>> filter)
    {
        var homework = await repository.GetAsync(filter);
        if (homework is null)
            return false;

        await repository.DeleteAsync(homework);
        await repository.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<HomeworkForViewDto>> GetAllAsync(Expression<Func<Homework, bool>> filter = null, string[] includes = null)
    {
        var homeworks = repository.GetAll(filter, includes).ToListAsync();
        return mapper.Map<IEnumerable<HomeworkForViewDto>>(homeworks);
    }

    public async Task<HomeworkForViewDto> GetAsync(Expression<Func<Homework, bool>> filter, string[] includes = null)
    {
        var homework = await repository.GetAsync(filter, includes);
        return mapper.Map<HomeworkForViewDto>(homework);
    }

    public async Task<HomeworkForViewDto> UpdateAsync(long id, HomeworkForUpdateDto dto)
    {
        var homework = await repository.GetAsync(h => h.Id == id);
        if (homework is null)
            throw new HttpStatusCodeException(404, $"DailyChallenge with Id {id} not found");

        homework = mapper.Map(dto, homework);
        repository.Update(homework);
        await repository.SaveChangesAsync();
        return mapper.Map<HomeworkForViewDto>(homework);
    }
}
