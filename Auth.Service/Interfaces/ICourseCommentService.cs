using Auth.Service.DTOs.Courses.CourseCommentsDto;

namespace Auth.Service.Interfaces;

public interface ICourseCommentService
{
    Task<CourseCommentForViewDto> AddCommentAsync(CourseCommentForCreationDto dto);
    Task<IEnumerable<CourseCommentForViewDto>> GetCommentsByCourseIdAsync(long courseId);
    Task<CourseCommentForViewDto> UpdateAsync(long commentId, long userId, CourseCommentForUpdateDto dto);

}
