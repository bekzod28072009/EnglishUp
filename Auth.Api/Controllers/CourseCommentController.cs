using Auth.Service.DTOs.Courses.CourseCommentsDto;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseCommentController : Controller
    {
        private readonly ICourseCommentService courseCommentService;

        public CourseCommentController(ICourseCommentService courseCommentService)
        {
            this.courseCommentService = courseCommentService;
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> PostAsync([FromBody] CourseCommentForCreationDto dto)
        {
            var result = await courseCommentService.AddCommentAsync(dto);
            return Ok(result);
        }

        // Anyone authenticated (User/Admin) can view course comments
        [HttpGet("course/{courseId:long}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetByCourseIdAsync(long courseId)
        {
            var result = await courseCommentService.GetCommentsByCourseIdAsync(courseId);
            return Ok(result);
        }

        // Only the comment owner or Admin should be allowed ideally, but for now:
        [HttpPatch("{commentId:long}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> PatchAsync(long commentId, [FromBody] CourseCommentForUpdateDto dto)
        {
            var result = await courseCommentService.UpdateAsync(commentId, dto);
            return Ok(result);
        }

        // Only Admin can delete comments (customize this if needed)
        [HttpDelete("{commentId:long}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(long commentId)
        {
            var isDeleted = await courseCommentService.DeleteCommentAsync(commentId);
            return Ok(new { Message = isDeleted ? "Deleted successfully" : "Deletion failed" });
        }
    }
}
