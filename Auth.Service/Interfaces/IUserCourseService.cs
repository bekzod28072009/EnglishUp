using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.Gamification;
using Auth.Service.DTOs.Courses.CourseCommentsDto;
using Auth.Service.DTOs.Courses.UserCoursesDto;
using System.Linq.Expressions;

namespace Auth.Service.Interfaces;

public interface IUserCourseService
{
    Task<IEnumerable<UserCourseForViewDto>> GetAllAsync(Expression<Func<UserCourse, bool>> filter = null, string[] includes = null);
    Task<UserCourseForViewDto> GetAsync(long id);

    Task<UserCourseForViewDto> CreateAsync(UserCourseForCreationDto dto);
    ValueTask<UserCourse> UpdateAsync(long id, long courseId, UserCourseForUpdateDto dto);
    Task<bool> DeleteAsync(long id);

    Task<bool> AddCommentAsync(CourseCommentForCreationDto dto);
}
