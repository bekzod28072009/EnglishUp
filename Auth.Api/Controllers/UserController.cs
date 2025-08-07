using Auth.Domain.Configurations;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Users;
using Auth.Service.Helpers;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Auth.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async ValueTask<IActionResult> GetAllAsync([FromQuery] PaginationParams @params)
        => Ok(await _userService.GetAllAsync(@params));

        [HttpGet("{id:long}")]
        public async ValueTask<IActionResult> GetAsync(long id)
            => Ok(await _userService.GetAsync(u => u.Id == id));


        [AllowAnonymous]
        [HttpPost]
        public async ValueTask<IActionResult> CreateAsync([FromBody] UserForCreationDto dto)
            => Ok(await _userService.CreateAsync(dto));

        [HttpPatch("{id:long}")]
        public async ValueTask<IActionResult> UpdateAsync(long id, [FromBody] UserForUpdateDto dto)
        {
            var currentUserId = HttpContextHelper.UserId;
            var isAdmin = HttpContextHelper.UserRole == "Admin";

            if (currentUserId != id && !isAdmin)
                return Forbid(); // 403 Forbidden

            return Ok(await _userService.UpdateAsync(id, dto));
        }


        [Authorize]
        [HttpDelete("{id:long}")]
        public async ValueTask<IActionResult> DeleteAsync(long id)
        {
            var currentUserId = HttpContextHelper.UserId;
            var isAdmin = HttpContextHelper.UserRole == "Admin";

            if (currentUserId != id && !isAdmin)
                return Forbid();

            Expression<Func<User, bool>> filter = user => user.Id == id;
            var result = await _userService.DeleteAsync(filter);

            return Ok(result);
        }

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
