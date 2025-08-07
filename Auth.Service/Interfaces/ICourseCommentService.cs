using Auth.Service.DTOs.Courses.CourseCommentsDto;

namespace Auth.Service.Interfaces;

public interface ICourseCommentService
{
    Task<CourseCommentForViewDto> AddCommentAsync(CourseCommentForCreationDto dto);
    Task<IEnumerable<CourseCommentForViewDto>> GetCommentsByCourseIdAsync(long courseId);
    Task<CourseCommentForViewDto> UpdateAsync(long commentId, CourseCommentForUpdateDto dto);
    Task<bool> DeleteCommentAsync(long commentId);

    // Fix for CS1061: Add missing method signature
    Task<CourseCommentForViewDto> GetCommentByIdAsync(long commentId);
}
