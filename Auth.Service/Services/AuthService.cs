using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Roles;
using Auth.Domain.Entities.Tokens;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.TokensDto;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using Auth.Service.Security;
using Auth.Service.Helpers;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;
using TokenValidationResult = Auth.Service.Helpers.TokenValidationResult;

namespace Auth.Service.Services;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<User> userRepository;
    private readonly IGenericRepository<Role> roleRepository;
    private readonly IGenericRepository<Token> tokenRepository;
    private readonly IPermissionService permissionService;
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;

    public AuthService(IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository,
        IGenericRepository<Token> tokenRepository, IPermissionService permissionService,
        IMapper mapper, IConfiguration configuration)
    {
        this.userRepository = userRepository;
        this.roleRepository = roleRepository;
        this.tokenRepository = tokenRepository;
        this.permissionService = permissionService;
        this.mapper = mapper;
        this.configuration = configuration;
    }

    public async ValueTask<TokenDto> GenerateToken(string Username, string password)
    {
        var user = await userRepository.GetAsync(u => u.Email == Username)
        ?? throw new HttpStatusCodeException(400, "Login or Password is incorrect");

        if (!SecurePasswordHasher.Verify(password, user.PasswordHash))
            throw new HttpStatusCodeException(400, "Login or Password is incorrect");

        var role = await roleRepository.GetAsync(r => r.Id == user.RoleId, includes: ["RolePermission"])
            ?? throw new HttpStatusCodeException(404, "User role not found");

        var permissionNames = role.RolePermission.Select(p => p.Name).ToList();
        var jsonPermissions = JsonSerializer.Serialize(permissionNames);

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
        var accessExpireDays = int.Parse(configuration["JWT:Expire"]);
        var resetExpireDays = int.Parse(configuration["JWT:ResExpire"]);

        var tokenEntity = new Token
        {
            UserId = user.Id,
            Key = Guid.NewGuid()
        };

        // 🔐 Access Token
        var accessToken = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"],
            expires: DateTime.UtcNow.AddDays(accessExpireDays),
            claims: new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, role.Name),
            new Claim("Permissions", jsonPermissions),
            new Claim(JwtRegisteredClaimNames.Jti, tokenEntity.Key.ToString())
            },
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        // 🔁 Refresh Token
        var refreshToken = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"],
            expires: DateTime.UtcNow.AddDays(resetExpireDays),
            claims: new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, tokenEntity.Key.ToString())
            },
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        // Store tokens in DB
        tokenEntity.AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);
        tokenEntity.ResetToken = new JwtSecurityTokenHandler().WriteToken(refreshToken);

        var existingToken = await tokenRepository.GetAsync(t => t.UserId == user.Id);
        if (existingToken is null)
        {
            await tokenRepository.CreateAsync(tokenEntity);
        }
        else
        {
            existingToken.AccessToken = tokenEntity.AccessToken;
            existingToken.ResetToken = tokenEntity.ResetToken;
            existingToken.Key = tokenEntity.Key;
            existingToken.CreatedAt = DateTime.UtcNow.AddHours(5);

            tokenRepository.Update(existingToken);
        }

        await tokenRepository.SaveChangesAsync();

        return mapper.Map<TokenDto>(tokenEntity);
    }

    //------------------------------------------------------------

    public async ValueTask<string> RestartToken(string refreshToken)
    {
        // Step 1: Validate the refresh token signature
        var tokenHandler = new JwtSecurityTokenHandler();
        ClaimsPrincipal principal;
        SecurityToken validatedToken;

        try
        {
            principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["JWT:ValidIssuer"],
                ValidateAudience = false,
                ValidateLifetime = false, // We validate manually
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
            }, out validatedToken);
        }
        catch
        {
            throw new HttpStatusCodeException(401, "Invalid refresh token");
        }

        // Step 2: Extract claims
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
        var jtiClaim = principal.FindFirst(JwtRegisteredClaimNames.Jti);

        if (userIdClaim is null || jtiClaim is null)
            throw new HttpStatusCodeException(401, "Invalid token claims");

        Guid userId = Guid.Parse(userIdClaim.Value);
        Guid tokenKey = Guid.Parse(jtiClaim.Value);

        // Fix for CS0019: Convert userId (Guid) to long before comparison
        // Replace this line:
        //var getToken = await tokenRepository.GetAsync(t => t.UserId == userId && t.ResetToken == refreshToken && t.Key == tokenKey);
        // With this:
        var getToken = await tokenRepository.GetAsync(t => t.UserId == long.Parse(userId.ToString()) && t.ResetToken == refreshToken && t.Key == tokenKey);

        // Step 4: Get user and permissions
        var user = await userRepository.GetAsync(p => p.Id == getToken.UserId)
            ?? throw new HttpStatusCodeException(404, "User not found");

        var role = await roleRepository.GetAsync(r => r.Id == user.RoleId, includes: ["RolePermission"])
            ?? throw new HttpStatusCodeException(404, "Role not found");

        var permissionNames = role.RolePermission.Select(p => p.Name).ToList();
        var jsonPermissions = JsonSerializer.Serialize(permissionNames);

        // Step 5: Generate new access token
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
        var expireDays = int.Parse(configuration["JWT:Expire"]);

        var newAccessToken = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"],
            expires: DateTime.UtcNow.AddDays(expireDays),
            claims: new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, role.Name),
            new Claim("Permissions", jsonPermissions),
            new Claim(JwtRegisteredClaimNames.Jti, getToken.Key.ToString()) // use same Key
            },
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        string writtenAccessToken = tokenHandler.WriteToken(newAccessToken);

        // Step 6: Update access token and return
        getToken.AccessToken = writtenAccessToken;
        getToken.CreatedAt = DateTime.UtcNow.AddHours(5);

        tokenRepository.Update(getToken);
        await tokenRepository.SaveChangesAsync();

        return writtenAccessToken;
    }


    private bool IsTokenExpired(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Token cannot be null or empty.", nameof(token));
        }

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            DateTime expirationTime = jwtToken.ValidTo;

            bool isExpired = expirationTime < DateTime.UtcNow;

            return isExpired;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking token expiration: {ex.Message}");
            throw;
        }
    }

    public async ValueTask<Dictionary<string, Dictionary<string, bool>>> GetPermissinWithToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            throw new ArgumentException("Token cannot be null or empty.", nameof(token));
        }
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            DateTime expirationTime = jwtToken.ValidTo;
            bool isExpired = expirationTime < DateTime.UtcNow;

            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User ID not found in the token.");
            }

            var user = await userRepository.GetAsync(p => p.Id == long.Parse(userId))
                  ?? throw new HttpStatusCodeException(400, "User not found");

            var res = await roleRepository.GetAsync(p => p.Id == user.RoleId, includes: ["RolePermissions"])
                       ?? throw new HttpStatusCodeException(400, "Role note fount");

            var res1 = res.RolePermission
              .Select(rp => rp.Name)
              .ToList();

            var res2 = await permissionService.GetPermissionsAsync(res1);

            return res2;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing token: {ex.Message}");
            throw;
        }
    }


    TokenValidationResult IAuthService.ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return new TokenValidationResult
            {
                IsValid = false,
                Message = "Token is empty or missing."
            };
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var expiryDate = jwtToken.ValidTo;

            if (expiryDate < DateTime.UtcNow)
            {
                return new TokenValidationResult
                {
                    IsValid = false,
                    IsExpired = true,
                    Message = "Token has expired."
                };
            }

            return new TokenValidationResult
            {
                IsValid = true
            };
        }
        catch (Exception ex)
        {
            return new TokenValidationResult
            {
                IsValid = false,
                Message = $"Token is invalid: {ex.Message}"
            };
        }
    }
}
