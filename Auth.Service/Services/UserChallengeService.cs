using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Gamification;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.UserChallenges;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using AutoMapper;

namespace Auth.Service.Services;

public class UserChallengeService : IUserChallengeService
{
    private readonly IGenericRepository<UserChallenge> userChallengeRepository;
    private readonly IUserService userService;
    private readonly IGenericRepository<DailyChallengge> dailyChallengeRepository;
    private readonly IMapper mapper;

    public UserChallengeService(IGenericRepository<UserChallenge> userChallengeRepository,
        IUserService userService,
        IGenericRepository<DailyChallengge> dailyChallengeRepository,
        IMapper mapper)
    {
        this.userChallengeRepository = userChallengeRepository;
        this.userService = userService;
        this.dailyChallengeRepository = dailyChallengeRepository;
        this.mapper = mapper;
    }

    public async Task<UserChallengeForViewDto> CompleteChallengeAsync(UserChallengeForCreationDto dto)
    {
        // Check if already completed
        var existing = await userChallengeRepository.GetAsync(uc =>
            uc.UserId == dto.UserId && uc.ChallengeId == dto.ChallengeId);
        if (existing is not null)
            throw new AlreadyExistsException("Challenge already completed by this user.");

        // Check challenge existence
        var challenge = await dailyChallengeRepository.GetAsync(c => c.Id == dto.ChallengeId)
            ?? throw new HttpStatusCodeException(404, "Challenge not found.");

        // Create entry
        var userChallenge = new UserChallenge
        {
            UserId = dto.UserId,
            ChallengeId = dto.ChallengeId,
            CompletedAt = DateTime.UtcNow
        };

        await userChallengeRepository.CreateAsync(userChallenge);
        await userChallengeRepository.SaveChangesAsync();

        // Add reward points
        //await userService.AddPointsAsync(dto.UserId, challenge.RewardPoints);

        // Fetch with includes to return full view DTO
        var result = await userChallengeRepository.GetAsync(
            uc => uc.Id == userChallenge.Id,
            new[] { nameof(UserChallenge.User), nameof(UserChallenge.Challenge) });

        return mapper.Map<UserChallengeForViewDto>(result);
    }

    public async Task<IEnumerable<UserChallengeForViewDto>> GetAllAsync()
    {
        var userChallenges =  userChallengeRepository.GetAll(
            includes: new[] { nameof(UserChallenge.User), nameof(UserChallenge.Challenge) });

        return userChallenges.Select(mapper.Map<UserChallengeForViewDto>);
    }

    public async Task<UserChallengeForViewDto> GetByIdAsync(long id)
    {
        var challenge = await userChallengeRepository.GetAsync(
            uc => uc.Id == id,
            new[] { nameof(UserChallenge.User), nameof(UserChallenge.Challenge) });

        return challenge is null
            ? throw new HttpStatusCodeException(404, "User challenge not found.")
            : mapper.Map<UserChallengeForViewDto>(challenge);
    }
}
