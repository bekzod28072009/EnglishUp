using Auth.Service.DTOs.UserChallenges;

namespace Auth.Service.Interfaces;

public interface IUserChallengeService
{
    Task<UserChallengeForViewDto> CompleteChallengeAsync(UserChallengeForCreationDto dto);
    Task<IEnumerable<UserChallengeForViewDto>> GetAllAsync();
    Task<UserChallengeForViewDto> GetByIdAsync(long id);
}
