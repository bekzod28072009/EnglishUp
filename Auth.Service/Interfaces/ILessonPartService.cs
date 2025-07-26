using Auth.Domain.Entities.Courses;
using Auth.Service.DTOs.Courses.CoursesDto;
using Auth.Service.DTOs.Courses.LessonPartsDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface ILessonPartService
{
    Task<IEnumerable<LessonPartForViewDto>> GetAllAsync(Expression<Func<LessonPart, bool>> filter = null, string[] includes = null);
    Task<LessonPartForViewDto> GetAsync(Expression<Func<LessonPart, bool>> filter, string[] includes = null);
    Task<LessonPartForViewDto> CreateAsync(LessonPartForCreationDto dto);
    Task<bool> DeleteAsync(Expression<Func<LessonPart, bool>> filter);
    Task<LessonPartForViewDto> UpdateAsync(long id, LessonPartForUpdateDto dto);
}
