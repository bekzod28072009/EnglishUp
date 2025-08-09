using Auth.Domain.Entities.Courses;
using Auth.Service.DTOs.Courses.CourseCommentsDto;
using Auth.Service.DTOs.Courses.UserCoursesDto;
using Auth.Service.Helpers;
using Auth.Service.Interfaces;
using Auth.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Auth.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserCourseController : Controller
    {
        private readonly IUserCourseService userCourseService;

        public UserCourseController(IUserCourseService _userCourseService)
        {
            userCourseService = _userCourseService;
        }

        // GET: api/UserCourse
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await userCourseService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/UserCourse/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var result = await userCourseService.GetAsync(id);
            return Ok(result);
        }

        // POST: api/UserCourse
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CreateAsync([FromBody] UserCourseForCreationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await userCourseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
        }

        // PATCH: api/UserCourse/{userId}/{courseId}
        [HttpPatch("{userId:long}/{courseId:long}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateAsync(long userId, long courseId, [FromBody] UserCourseForUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedEntity = await userCourseService.UpdateAsync(userId, courseId, dto);
            return Ok(updatedEntity);
        }

        // DELETE: api/UserCourse/{id}
        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var isDeleted = await userCourseService.DeleteAsync(id);
            return Ok(isDeleted);
        }

        // POST: api/UserCourse/AddComment
        [HttpPost("AddComment")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> AddCommentAsync([FromBody] CourseCommentForCreationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await userCourseService.AddCommentAsync(dto);
            return Ok(result);
        }
    }
}
