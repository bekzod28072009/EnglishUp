using Auth.Domain.Configurations;
using Auth.Service.DTOs.Users;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async ValueTask<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
        => Ok(await _userService.GetAllAsync(@params));

        [HttpGet("{id:long}")]
        public async ValueTask<IActionResult> GetAsync(long id)
            => Ok(await _userService.GetAsync(u => u.Id == id));

        [HttpPost]
        public async ValueTask<IActionResult> CreateAsync([FromBody] UserForCreationDto dto)
            => Ok(await _userService.CreateAsync(dto));

        [HttpPatch("{id:long}")]
        public async ValueTask<IActionResult> UpdatePartialAsync(long id, [FromBody] UserForUpdateDto dto)
            => Ok(await _userService.UpdateAsync(id, dto));


        [HttpDelete("{id:long}")]
        public async ValueTask<IActionResult> DeleteAsync(long id)
            => Ok(await _userService.DeleteAsync(u => u.Id == id));

        [HttpPatch("change-password")]
        public async ValueTask<IActionResult> ChangePasswordAsync([FromQuery] string email, [FromQuery] string newPassword)
            => Ok(await _userService.ChangePassword(email, newPassword));

        [HttpPatch("{userId:long}/add-points")]
        public async ValueTask<IActionResult> AddPointsAsync(long userId, [FromQuery] int points)
        {
            await _userService.AddPointsAsync(userId, points);
            return Ok();
        }
    }
}
