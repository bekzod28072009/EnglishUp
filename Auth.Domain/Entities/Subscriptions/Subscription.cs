using Auth.Domain.Common;
using Auth.Domain.Entities.UserManagement;
using Auth.Domain.Enums;

namespace Auth.Domain.Entities.Subscriptions;

public class Subscription : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; } = default!;
    public SubscriptionType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive => EndDate >= DateTime.UtcNow;
}
