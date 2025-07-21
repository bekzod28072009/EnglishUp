using Auth.Domain.Common;

namespace Auth.Domain.Entities.Tokens;

public class Token : Auditable
{
    public Guid Key { get; set; }
    public string AccessToken { get; set; }
    public string? ResetToken { get; set; }
    public long UserId { get; set; }
}
