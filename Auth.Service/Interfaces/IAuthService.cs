﻿using Auth.Service.DTOs.TokensDto;
using Auth.Service.Helpers;

namespace Auth.Service.Interfaces;

public interface IAuthService
{
    ValueTask<TokenDto> GenerateToken(string email, string password);
    ValueTask<string> RestartToken(string token);
    ValueTask<Dictionary<string, Dictionary<string, bool>>> GetPermissinWithToken(string token);
    TokenValidationResult ValidateToken(string token);
}
