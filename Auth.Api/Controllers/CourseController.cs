using Auth.Service.DTOs.Courses.CoursesDto;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Apply to all endpoints
    public class CourseController : Controller
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService courseService)
        {
            courseService = courseService;
        }

        // GET: api/Course
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await courseService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/Course/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var result = await courseService.GetAsync(id);
            return Ok(result);
        }

        // POST: api/Course
        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> CreateAsync([FromBody] CourseForCreationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await courseService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
        }

        // PUT: api/Course/{id}
        [HttpPatch("{id:long}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> UpdateAsync(long id, [FromBody] CourseForUpdateDto dto)
        {
            var updatedCourse = await courseService.UpdateAsync(id, dto);
            return Ok(updatedCourse);
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var isDeleted = await courseService.DeleteAsync(id);
            return Ok(isDeleted);
        }

    }
}
