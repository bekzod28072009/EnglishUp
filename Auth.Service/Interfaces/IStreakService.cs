using Auth.Service.DTOs.Gamification.StreaksDto;

namespace Auth.Service.Interfaces;

public interface IStreakService
{
    Task<StreakForViewDto> GetByUserIdAsync(long userId);
    Task<StreakForViewDto> CreateAsync(StreakForCreationDto dto);
    Task<StreakForViewDto> UpdateAsync(long id, StreakForUpdateDto dto);
}
