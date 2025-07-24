using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Courses;
using Auth.Service.DTOs.Courses.UserCoursesDto;
using Auth.Service.Exceptions;
using Auth.Service.Helpers;
using AutoMapper;
using System.Linq.Expressions;

namespace Auth.Service.Services;

public class UserCourseService
{
    private readonly IGenericRepository<UserCourse> repository;
    private readonly IMapper mapper;

    public UserCourseService(IGenericRepository<UserCourse> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<UserCourseForViewDto>> GetAllAsync(string[] includes = null)
    {
        var userCourses = repository.GetAll(null, includes);
        return userCourses.Select(mapper.Map<UserCourseForViewDto>);
    }

    public async Task<UserCourseForViewDto> GetAsync(Expression<Func<UserCourse, bool>> filter, string[] includes = null)
    {
        var userCourse = await repository.GetAsync(filter, includes);
        return mapper.Map<UserCourseForViewDto>(userCourse);
    }

    public async Task<UserCourseForViewDto> CreateAsync(UserCourseForCreationDto dto)
    {
        var entity = mapper.Map<UserCourse>(dto);
        var created = await repository.CreateAsync(entity);
        await repository.SaveChangesAsync(); // Ensure changes are saved asynchronously
        return mapper.Map<UserCourseForViewDto>(created);
    }

    public async Task<UserCourseForViewDto> UpdateAsync(long id, UserCourseForUpdateDto dto)
    {
        var userCourse = await repository.GetAsync(x => x.Id == id);
        if (userCourse is null)
            throw new Exception("UserCourse not found");

        userCourse.UserId = dto.UserId;
        userCourse.CourseId = dto.CourseId;
        userCourse.UpdatedAt = DateTime.UtcNow;

        var updated = repository.Update(userCourse);
        await repository.SaveChangesAsync(); // Ensure changes are saved asynchronously
        return mapper.Map<UserCourseForViewDto>(updated);
    }

    public async Task<bool> DeleteAsync(Expression<Func<UserCourse, bool>> filter)
    {
        var res = await repository.GetAsync(filter);

        if (res == null)
            throw new HttpStatusCodeException(404, "UserCourse not found");

        res.DeletedBy = HttpContextHelper.UserId;
        await repository.DeleteAsync(res);
        await repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddCommentAsync(UserCourseCommentDto dto)
    {
        var userCourse = await repository.GetAsync(x =>
            x.UserId == dto.UserId && x.CourseId == dto.CourseId);

        if (userCourse == null)
            throw new Exception("Siz bu kursni sotib olmagansiz va comment yoza olmaysiz.");

        userCourse.CommentText = dto.CommentText;
        userCourse.Rating = dto.Rating;
        userCourse.UpdatedAt = DateTime.UtcNow;

        repository.Update(userCourse);
        await repository.SaveChangesAsync();
        return true;
    }
}
