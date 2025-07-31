using Auth.Domain.Enums;

namespace Auth.Service.DTOs.Subscriptions;

public class SubscriptionForUpdateDto
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string UserFullName { get; set; } = string.Empty;

    public long PlanId { get; set; }

    public string PlanName { get; set; } = string.Empty;

    public decimal PlanPrice { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }
}
