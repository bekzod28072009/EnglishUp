namespace Auth.Service.DTOs.UserChallenges;

public class UserChallengeForViewDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;

    public long ChallengeId { get; set; }
    public string ChallengeTitle { get; set; } = string.Empty;

    public DateTime CompletedAt { get; set; }
}
