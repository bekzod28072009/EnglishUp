using Auth.Service.DTOs.Courses.LessonsDto;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LessonController : Controller
    {
        private readonly ILessonService lessonService;

        public LessonController(ILessonService lessonService)
        {
            this.lessonService = lessonService;
        }

        /// <summary>
        /// Create a new lesson
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(LessonForViewDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync([FromBody] LessonForCreationDto dto)
        {
            var created = await lessonService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update a lesson
        /// </summary>
        [HttpPatch("{id:long}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(LessonForViewDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAsync(long id, [FromBody] LessonForUpdateDto dto)
        {
            var updated = await lessonService.UpdateAsync(id, dto);
            return Ok(updated);
        }

        /// <summary>
        /// Delete a lesson
        /// </summary>
        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await lessonService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Get a lesson by ID
        /// </summary>
        [HttpGet("{id:long}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LessonForViewDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var lesson = await lessonService.GetAsync(id);
            return Ok(lesson);
        }

        /// <summary>
        /// Get all lessons
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<LessonForViewDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var lessons = await lessonService.GetAllAsync();
            return Ok(lessons);
        }
    }
}
