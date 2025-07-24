namespace Auth.Service.DTOs.TokensDto;

public class TokenDto
{
    public string AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
