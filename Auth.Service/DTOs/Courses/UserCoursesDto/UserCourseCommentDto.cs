namespace Auth.Service.DTOs.Courses.UserCoursesDto;

public class UserCourseCommentDto
{
    public long UserId { get; set; }
    public long CourseId { get; set; }

    public string CommentText { get; set; }
    public int Rating { get; set; } // 1 to 5
}
