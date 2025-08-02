using Auth.DataAccess.Interface;
using Auth.Domain.Entities.Courses;
using Auth.Service.DTOs.Courses.CourseCommentsDto;
using Auth.Service.Exceptions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Auth.Service.Services;

public class CourseCommentService
{
    private readonly IGenericRepository<CourseComment> repository;
    private readonly IMapper mapper;

    public CourseCommentService(IGenericRepository<CourseComment> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<CourseCommentForViewDto> AddCommentAsync(CourseCommentForCreationDto dto)
    {
        var comment = mapper.Map<CourseComment>(dto);
        comment.CreatedAt = DateTime.UtcNow;

        await repository.CreateAsync(comment);
        await repository.SaveChangesAsync();

        return mapper.Map<CourseCommentForViewDto>(comment);
    }

    public async Task<IEnumerable<CourseCommentForViewDto>> GetCommentsByCourseIdAsync(long courseId)
    {
        var comments = await repository.GetAll(
            c => c.CourseId == courseId,
            includes: new[] { nameof(CourseComment.User) }).ToListAsync();

        return comments.Select(comment => new CourseCommentForViewDto
        {
            Id = comment.Id,
            UserId = comment.UserId,
            UserFullName = comment.User?.FullName ?? "Unknown",
            CourseId = comment.CourseId,
            Content = comment.Content,
            CommentedAt = comment.CreatedAt
        });
    }

    public async Task<CourseCommentForViewDto> UpdateAsync(long commentId, long userId, CourseCommentForUpdateDto dto)
    {
        var comment = await repository.GetAsync(c => c.Id == commentId && c.UserId == userId, new[] { "User" })
            ?? throw new HttpStatusCodeException(404, "Comment not found or you don't have permission to update this.");

        comment.Content = dto.Content;
        comment.UpdatedAt = DateTime.UtcNow;

        await repository.SaveChangesAsync();

        return mapper.Map<CourseCommentForViewDto>(comment);
    }

}
