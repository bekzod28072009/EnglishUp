using Auth.Domain.Entities.Courses;
using Auth.Service.DTOs.Courses.CoursesDto;
using Auth.Service.DTOs.Courses.LessonsDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface ILessonService
{
    Task<IEnumerable<LessonForViewDto>> GetAllAsync(Expression<Func<Lesson, bool>> filter = null, string[] includes = null);
    Task<LessonForViewDto> GetAsync(long id);
    Task<LessonForViewDto> CreateAsync(LessonForCreationDto dto);
    Task<bool> DeleteAsync(long id);
    Task<LessonForViewDto> UpdateAsync(long id, LessonForUpdateDto dto);
}
