namespace Auth.Service.DTOs.Subscriptions;

public class SubscriptionForViewDto
{
    public long Id { get; set; }
    public string UserFullName { get; set; }
    public string Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
}
