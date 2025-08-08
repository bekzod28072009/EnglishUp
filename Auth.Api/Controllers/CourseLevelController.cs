using Auth.Service.DTOs.Courses.CourseLevelsDto;
using Auth.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseLevelController : Controller
    {
        private readonly ICourseLevelService _courseLevelService;

        public CourseLevelController(ICourseLevelService courseLevelService)
        {
            _courseLevelService = courseLevelService;
        }

        /// <summary>
        /// Create a new course level
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")] // Only Admin can create
        public async Task<IActionResult> CreateAsync([FromBody] CourseLevelForCreationDto dto)
        {
            var result = await _courseLevelService.CreateAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Get all course levels
        /// </summary>
        [HttpGet]
        [AllowAnonymous] // Everyone can view
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _courseLevelService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get course level by ID
        /// </summary>
        [HttpGet("{id:long}")]
        [AllowAnonymous] // Everyone can view
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var result = await _courseLevelService.GetAsync(id);
            return Ok(result);
        }

        /// <summary>
        /// Update course level fully or partially (PATCH)
        /// </summary>
        [HttpPatch("{id:long}")]
        [Authorize(Roles = "Admin")] // Only Admin can update
        public async Task<IActionResult> UpdateAsync(long id, [FromBody] CourseLevelForUpdateDto dto)
        {
            var result = await _courseLevelService.UpdateAsync(id, dto);
            return Ok(result);
        }

        /// <summary>
        /// Delete course level
        /// </summary>
        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Admin")] // Only Admin can delete
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var result = await _courseLevelService.DeleteAsync(id);
            return Ok(result);
        }


    }
}
