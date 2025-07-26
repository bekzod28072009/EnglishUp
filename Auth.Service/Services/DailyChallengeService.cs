using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Gamification;
using Auth.Service.DTOs.Gamification.DailyChallengesDto;
using AutoMapper;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class DailyChallengeService
{
    private readonly IGenericRepository<DailyChallengge> repository;
    private readonly IMapper mapper;

    public DailyChallengeService(IGenericRepository<DailyChallengge> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<DailyChallengeForViewDto> CreateAsync(DailyChallengeForCreateDto dto)
    {
        var challenge = mapper.Map<DailyChallengge>(dto);
        await repository.CreateAsync(challenge);
        await repository.SaveChangesAsync();
        return mapper.Map<DailyChallengeForViewDto>(challenge);
    }

    public async Task<bool> DeleteAsync(Expression<Func<DailyChallengge, bool>> filter)
    {
        var challenge = await repository.GetAsync(filter);
        if (challenge is null)
            return false;

        return await repository.DeleteAsync(challenge);
    }

    public async Task<IEnumerable<DailyChallengeForViewDto>> GetAllAsync(string[] includes = null)
    {
        var challenges = repository.GetAll(null, includes);
        return mapper.Map<IEnumerable<DailyChallengeForViewDto>>(challenges);
    }

    public async Task<DailyChallengeForViewDto> GetAsync(Expression<Func<DailyChallengge, bool>> filter, string[] includes = null)
    {
        var challenge = await repository.GetAsync(filter, includes);
        return mapper.Map<DailyChallengeForViewDto>(challenge);
    }

    public async Task<DailyChallengeForViewDto> UpdateAsync(long id, DailyChallengeForUpdateDto dto)
    {
        var challenge = await repository.GetAsync(c => c.Id == id);
        if (challenge is null)
            return null;

        mapper.Map(dto, challenge);
        await repository.SaveChangesAsync();
        return mapper.Map<DailyChallengeForViewDto>(challenge);
    }
}
