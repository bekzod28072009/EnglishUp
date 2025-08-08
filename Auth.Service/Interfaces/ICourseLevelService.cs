using Auth.Domain.Entities.Courses;
using Auth.Service.DTOs.Courses.CourseLevelsDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface ICourseLevelService
{
    Task<IEnumerable<CourseLevelForViewDto>> GetAllAsync(Expression<Func<CourseLevel, bool>> filter = null, string[] includes = null);
    Task<CourseLevelForViewDto> GetAsync(long id);
    Task<CourseLevelForViewDto> CreateAsync(CourseLevelForCreationDto dto);
    Task<CourseLevelForViewDto> UpdateAsync(long id, CourseLevelForUpdateDto dto);
    Task<bool> DeleteAsync(long id);
}
