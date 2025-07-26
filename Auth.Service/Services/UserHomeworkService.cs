using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Homeworks;
using Auth.Service.DTOs.Homeworks.UserHomeworksDto;
using Auth.Service.Exceptions;
using AutoMapper;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class UserHomeworkService
{
    private readonly IGenericRepository<UserHomework> repository;
    private readonly IMapper mapper;

    public UserHomeworkService(IGenericRepository<UserHomework> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<UserHomeworkForViewDto>> GetAllAsync(Expression<Func<UserHomework, bool>> filter = null, string[] includes = null)
    {
        var userHomeworks = repository.GetAll(filter, includes);
        return mapper.Map<IEnumerable<UserHomeworkForViewDto>>(userHomeworks);
    }

    public async Task<UserHomeworkForViewDto> GetAsync(Expression<Func<UserHomework, bool>> filter, string[] includes = null)
    {
        var userHomework = await repository.GetAsync(filter, includes);
        if (userHomework is null)
            throw new HttpStatusCodeException(404, "UserHomework not found");

        return mapper.Map<UserHomeworkForViewDto>(userHomework);
    }

    public async Task<UserHomeworkForViewDto> CreateAsync(UserHomeworkForCreationDto dto)
    {
        var userHomework = mapper.Map<UserHomework>(dto);
        await repository.CreateAsync(userHomework);
        await repository.SaveChangesAsync();

        return mapper.Map<UserHomeworkForViewDto>(userHomework);
    }

    public async Task<UserHomeworkForViewDto> UpdateAsync(long id, UserHomeworkForUpdateDto dto)
    {
        var userHomework = await repository.GetAsync(x => x.Id == id);
        if (userHomework is null)
            throw new HttpStatusCodeException(404, "UserHomework not found");

        mapper.Map(dto, userHomework);
        repository.Update(userHomework);
        await repository.SaveChangesAsync();

        return mapper.Map<UserHomeworkForViewDto>(userHomework);
    }

    public async Task<bool> DeleteAsync(Expression<Func<UserHomework, bool>> filter)
    {
        var userHomework = await repository.GetAsync(filter);
        if (userHomework is null)
            return false;

        await repository.DeleteAsync(userHomework);
        await repository.SaveChangesAsync();
        return true;
    }
}
