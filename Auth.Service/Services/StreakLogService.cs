﻿using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Gamification;
using Auth.Service.DTOs.Gamification.StreakLogDto;
using Auth.Service.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class StreakLogService
{
    private readonly IGenericRepository<StreakLog> repository;
    private readonly IMapper mapper;

    public StreakLogService(IGenericRepository<StreakLog> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<StreakLogForViewDto>> GetAllAsync(Expression<Func<StreakLog, bool>> filter = null, string[] includes = null)
    {
        var logs = await repository.GetAll(filter, includes).ToListAsync();
        return mapper.Map<IEnumerable<StreakLogForViewDto>>(logs);
    }

    public async Task<StreakLogForViewDto> GetAsync(Expression<Func<StreakLog, bool>> filter, string[] includes = null)
    {
        var log = await repository.GetAsync(filter, includes);
        if (log is null)
            throw new HttpStatusCodeException(404, "Streak log not found");

        return mapper.Map<StreakLogForViewDto>(log);
    }

    public async Task<StreakLogForViewDto> CreateAsync(StreakLogForCreationDto dto)
    {
        var log = mapper.Map<StreakLog>(dto);
        await repository.CreateAsync(log);
        await repository.SaveChangesAsync();

        return mapper.Map<StreakLogForViewDto>(log);
    }

    public async Task<StreakLogForViewDto> UpdateAsync(long id, StreakLogForUpdateDto dto)
    {
        var existing = await repository.GetAsync(x => x.Id == id);
        if (existing is null)
            throw new HttpStatusCodeException(404, "Streak log not found");

        mapper.Map(dto, existing);
        await repository.SaveChangesAsync();

        return mapper.Map<StreakLogForViewDto>(existing);
    }

    public async Task<bool> DeleteAsync(Expression<Func<StreakLog, bool>> filter)
    {
        var entity = await repository.GetAsync(filter);
        if (entity is null)
            return false;

        await repository.DeleteAsync(entity);
        await repository.SaveChangesAsync(); // SaveChangesAsync returns void, so remove the comparison.  
        return true;
    }
}
