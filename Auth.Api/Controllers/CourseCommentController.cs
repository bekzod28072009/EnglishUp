using Auth.Service.DTOs.Courses.CourseCommentsDto;
using Auth.Service.Helpers;
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
            // Fix: Get the single comment by its ID, not by course ID
            var comment = await courseCommentService.GetCommentByIdAsync(commentId);
            if (comment is null)
                return NotFound("Comment not found");

            var currentUserId = HttpContextHelper.UserId;
            var currentUserRole = HttpContextHelper.UserRole;

            if (comment.UserId != currentUserId && currentUserRole != "Admin")
                return Forbid("You're not allowed to update this comment");

            var updated = await courseCommentService.UpdateAsync(commentId, dto);
            return Ok(updated);
        }


        // Only Admin can delete comments (customize this if needed)
        [HttpDelete("{commentId:long}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteAsync(long commentId)
        {
            var comment = await courseCommentService.GetCommentByIdAsync(commentId);
            if (comment is null)
                return NotFound("Comment not found");

            var currentUserId = HttpContextHelper.UserId;
            var currentUserRole = HttpContextHelper.UserRole;

            if (comment.UserId != currentUserId && currentUserRole != "Admin")
                return Forbid("You're not allowed to delete this comment");

            var isDeleted = await courseCommentService.DeleteCommentAsync(commentId);
            return Ok(new { Message = isDeleted ? "Deleted successfully" : "Failed to delete" });
        }

    }
}
