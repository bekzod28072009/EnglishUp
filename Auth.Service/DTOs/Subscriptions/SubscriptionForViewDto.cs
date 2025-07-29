namespace Auth.Service.DTOs.Subscriptions;

public class SubscriptionForViewDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // mapped from enum.ToString()
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } // calculated
}
