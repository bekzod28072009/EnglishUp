using Auth.Domain.Enums;

namespace Auth.Service.DTOs.Subscriptions;

public class SubscriptionForUpdateDto
{
    public SubscriptionType? Type { get; set; }
    public DateTime? EndDate { get; set; }
}
