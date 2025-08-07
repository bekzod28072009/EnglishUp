using Auth.Service.DTOs.Logins;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var tokenDto = await _authService.GenerateToken(loginDto.Email, loginDto.Password);
            return Ok(tokenDto);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var newAccessToken = await _authService.RestartToken(refreshToken);
            return Ok(new { AccessToken = newAccessToken });
        }

        [HttpPost("validate-token")]
        public IActionResult ValidateToken([FromBody] string token)
        {
            var result = _authService.ValidateToken(token);
            return Ok(result);
        }

        [HttpGet("GetPermissionWithToken")]
        public async Task<IActionResult> GetPermissions([FromBody] string token)
        {
            var permissions = await _authService.GetPermissinWithToken(token);
            return Ok(permissions);
        }
    }
}
