using Auth.Service.DTOs.TokensDto;

namespace Auth.Service.Interfaces;

public interface IAuthService
{
    ValueTask<TokenDto> GenerateToken(string email, string password);
    ValueTask<string> RestartToken(string token);
    ValueTask<Dictionary<string, Dictionary<string, bool>>> GetPermissinWithToken(string token);
}
