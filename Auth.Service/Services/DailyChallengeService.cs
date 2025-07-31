using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Gamification;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Gamification.DailyChallengesDto;
using Auth.Service.DTOs.UserChallenges;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class DailyChallengeService : IDaliyChallengeService
{
    private readonly IGenericRepository<UserChallenge> userChallengeRepository;
    private readonly IGenericRepository<DailyChallengge> challengeRepository;
    private readonly IUserService userService;
    private readonly IMapper mapper;

    public DailyChallengeService(IGenericRepository<UserChallenge> userChallengeRepository,
    IGenericRepository<DailyChallengge> challengeRepository,
    IUserService userService,
        IMapper mapper)
    {
        this.userChallengeRepository = userChallengeRepository;
        this.challengeRepository = challengeRepository;
        this.userService = userService;
        this.mapper = mapper;
    }

    public async Task<DailyChallengeForViewDto> CreateAsync(DailyChallengeForCreateDto dto)
    {
        var challenge = mapper.Map<DailyChallengge>(dto);
        await challengeRepository.CreateAsync(challenge);
        await challengeRepository.SaveChangesAsync();
        return mapper.Map<DailyChallengeForViewDto>(challenge);
    }

    public async Task<bool> DeleteAsync(Expression<Func<DailyChallengge, bool>> filter)
    {
        var challenge = await challengeRepository.GetAsync(filter);
        if (challenge is null)
            return false;

        return await challengeRepository.DeleteAsync(challenge);
    }

    public async Task<IEnumerable<DailyChallengeForViewDto>> GetAllAsync(Expression<Func<DailyChallengge, bool>> filter = null, string[] includes = null)
    {
        var challenges = await challengeRepository.GetAll(filter, includes).ToListAsync();
        return mapper.Map<IEnumerable<DailyChallengeForViewDto>>(challenges);
    }

    public async Task<DailyChallengeForViewDto> GetAsync(Expression<Func<DailyChallengge, bool>> filter, string[] includes = null)
    {
        var challenge = await challengeRepository.GetAsync(filter, includes);
        return mapper.Map<DailyChallengeForViewDto>(challenge);
    }

    public async Task<DailyChallengeForViewDto> UpdateAsync(long id, DailyChallengeForUpdateDto dto)
    {
        var challenge = await challengeRepository.GetAsync(c => c.Id == id);
        if (challenge is null)
            return null;

        mapper.Map(dto, challenge);
        await challengeRepository.SaveChangesAsync();
        return mapper.Map<DailyChallengeForViewDto>(challenge);
    }

    public async Task<DailyChallengeForViewDto?> GetTodayChallengeAsync()
    {
        var today = DateTime.UtcNow.Date;

        var challenge = await challengeRepository.GetAsync(
            c => c.CreatedAt.Date == today
        );

        return challenge is null
            ? null
            : mapper.Map<DailyChallengeForViewDto>(challenge);
    }


    public async Task CompleteChallengeAsync(long userId, long challengeId)
    {
        var challenge = await challengeRepository.GetAsync(c => c.Id == challengeId)
            ?? throw new HttpStatusCodeException(404, "Challenge not found");

        var userChallenge = new UserChallenge
        {
            UserId = userId,
            ChallengeId = challengeId,
            CompletedAt = DateTime.UtcNow
        };

        await userChallengeRepository.CreateAsync(userChallenge);

        // You can reward points dynamically based on the challenge
        await userService.AddPointsAsync(userId, challenge.RewardPoints);

        await userChallengeRepository.SaveChangesAsync();
    }

    public async Task<UserChallengeForViewDto> CompleteChallengeAsync(UserChallengeForCreationDto dto)
    {
        // Validate challenge existence
        var challenge = await challengeRepository.GetAsync(c => c.Id == dto.ChallengeId)
            ?? throw new HttpStatusCodeException(404, "Daily challenge not found");

        // Check if already completed
        var exists = await userChallengeRepository.GetAsync(
            uc => uc.UserId == dto.UserId && uc.ChallengeId == dto.ChallengeId);

        if (exists is not null)
            throw new AlreadyExistsException("You have already completed this challenge");

        // Create new UserChallenge
        var userChallenge = new UserChallenge
        {
            UserId = dto.UserId,
            ChallengeId = dto.ChallengeId,
            CompletedAt = DateTime.UtcNow
        };

        await userChallengeRepository.CreateAsync(userChallenge);

        // Add points to the user
        await userService.AddPointsAsync(dto.UserId, challenge.RewardPoints);

        await userChallengeRepository.SaveChangesAsync();

        // Map and return the result
        return mapper.Map<UserChallengeForViewDto>(userChallenge);
    }


}
