using Auth.Domain.Entities.Subscriptions;
using Auth.Service.DTOs.Subscriptions;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface ISubscriptionService
{
    Task<IEnumerable<SubscriptionForViewDto>> GetAllAsync(Expression<Func<Subscription, bool>> filter = null, string[] includes = null);
    Task<SubscriptionForViewDto> GetAsync(Expression<Func<Subscription, bool>> filter, string[] includes = null);
    Task<SubscriptionForViewDto> CreateAsync(SubscriptionForCreateDto dto);
    Task<SubscriptionForViewDto> UpdateAsync(long id, SubscriptionForUpdateDto dto);
    Task<bool> DeleteAsync(Expression<Func<Subscription, bool>> filter);
}
