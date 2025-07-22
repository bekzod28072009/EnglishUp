using Auth.Domain.Enums;

namespace Auth.Service.DTOs.Subscriptions;

public class SubscriptionForCreateDto
{
    public long UserId { get; set; }
    public SubscriptionType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
