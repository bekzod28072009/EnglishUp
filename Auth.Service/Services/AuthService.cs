using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Roles;
using Auth.Domain.Entities.Tokens;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.TokensDto;
using Auth.Service.Exceptions;
using Auth.Service.Interfaces;
using Auth.Service.Security;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

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
        var user = await userRepository.GetAsync(p => p.Email == Username)
                   ?? throw new HttpStatusCodeException(400, "Login or Password is incorrect");

        var res = await roleRepository.GetAsync(p => p.Id == user.RoleId, includes: ["RolePermissions"]);


        var res1 = new List<string>();
        foreach (var role in res.RolePermission)
        {
            res1.Add(role.Name);
        }

        var jsonPermissin = JsonSerializer.Serialize(res1);

        if (!SecurePasswordHasher.Verify(password, user.PasswordHash))
            throw new HttpStatusCodeException(400, "Login or Password is incorrect");


        if (user is null)
            throw new HttpStatusCodeException(400, "Login or Password is incorrect");

        var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

        Token token1 = new Token()
        {
            UserId = user.Id,
            Key = Guid.NewGuid()
        };

        var token = new JwtSecurityToken(
             issuer: configuration["JWT:ValidIssuer"],
             expires: DateTime.Now.AddDays(int.Parse(configuration["JWT:Expire"])),
             claims: new List<Claim>
             {
                    new Claim("Permissions", jsonPermissin),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, res.Name.ToString())
             },
             signingCredentials: new SigningCredentials(
             key: authSigningKey,
             algorithm: SecurityAlgorithms.HmacSha256)
        );

        var resToken = new JwtSecurityToken(
             issuer: configuration["JWT:ValidIssuer"],
             expires: DateTime.Now.AddDays(int.Parse(configuration["JWT:ResExpire"])),
              claims: new List<Claim>
             {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, token1.Key.ToString()),
             },
             signingCredentials: new SigningCredentials(
             key: authSigningKey,
             algorithm: SecurityAlgorithms.HmacSha256)
        );


        token1.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
        token1.ResetToken = new JwtSecurityTokenHandler().WriteToken(resToken);

        if (await tokenRepository.GetAsync(p => p.UserId == user.Id) == null)
        {
            await tokenRepository.CreateAsync(token1);
            await tokenRepository.SaveChangesAsync();
        }
        else
        {
            var getToken = await tokenRepository.GetAsync(p => p.UserId == user.Id);

            getToken.AccessToken = token1.AccessToken;
            getToken.ResetToken = token1.ResetToken;
            getToken.Key = token1.Key;
            getToken.CreatedAt = DateTime.UtcNow.AddHours(5);

            tokenRepository.Update(getToken);
            await tokenRepository.SaveChangesAsync();
        }

        return mapper.Map<TokenDto>(token1);
    }

    public async ValueTask<string> RestartToken(string token)
    {
        bool resToken = IsTokenExpired(token);
        if (!resToken)
        {
            var getToken = await tokenRepository.GetAsync(p => p.ResetToken == token);

            if (getToken == null)
            {
                throw new HttpStatusCodeException(400, "Token not found");
            }

            var user = await userRepository.GetAsync(p => p.Id == getToken.UserId)
                   ?? throw new HttpStatusCodeException(400, "User not found");

            var res = await roleRepository.GetAsync(p => p.Id == user.RoleId, includes: ["RolePermissions"])
                       ?? throw new HttpStatusCodeException(400, "Role note fount");

            var res1 = new List<string>();
            foreach (var role in res.RolePermission)
            {
                res1.Add(role.Name);
            }

            var jsonPermissin = JsonSerializer.Serialize(res1);


            var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

            var newToken = new JwtSecurityToken(
                 issuer: configuration["JWT:ValidIssuer"],
                 expires: DateTime.Now.AddDays(int.Parse(configuration["JWT:Expire"])),
                 claims: new List<Claim>
                 {
                    new Claim("Permissions", jsonPermissin),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, res.Name.ToString())
                 },
                 signingCredentials: new SigningCredentials(
                 key: authSigningKey,
                 algorithm: SecurityAlgorithms.HmacSha256)
            );



            getToken.AccessToken = new JwtSecurityTokenHandler().WriteToken(newToken);
            getToken.CreatedAt = DateTime.UtcNow.AddHours(5);

            tokenRepository.Update(getToken);
            await tokenRepository.SaveChangesAsync();

            return new JwtSecurityTokenHandler().WriteToken(newToken);

        }

        throw new HttpStatusCodeException(400, "Token is expired");
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
}
