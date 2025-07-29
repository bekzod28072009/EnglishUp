using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.Gamification;
using Auth.Domain.Entities.Homeworks;
using Auth.Domain.Entities.Permissions;
using Auth.Domain.Entities.Roles;
using Auth.Domain.Entities.Subscriptions;
using Auth.Domain.Entities.Tests;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Courses.CoursesDto;
using Auth.Service.DTOs.Courses.LessonPartsDto;
using Auth.Service.DTOs.Courses.LessonsDto;
using Auth.Service.DTOs.Courses.UserCoursesDto;
using Auth.Service.DTOs.Gamification.DailyChallengesDto;
using Auth.Service.DTOs.Gamification.StreakLogDto;
using Auth.Service.DTOs.Gamification.StreaksDto;
using Auth.Service.DTOs.Homeworks.HomeworksDto;
using Auth.Service.DTOs.Homeworks.PointTransactionsDto;
using Auth.Service.DTOs.Homeworks.UserHomeworksDto;
using Auth.Service.DTOs.Permissions;
using Auth.Service.DTOs.Roles;
using Auth.Service.DTOs.Subscriptions;
using Auth.Service.DTOs.Tests.MockTestResultsDto;
using Auth.Service.DTOs.Tests.MockTestsDto;
using Auth.Service.DTOs.Users;
using AutoMapper;

namespace Auth.Api.Configurations;

public class MapConfiguration : Profile
{
    public MapConfiguration()
    {
        // User
        CreateMap<UserForCreationDto, User>().ReverseMap();
        CreateMap<UserForUpdateDto, User>().ReverseMap();
        CreateMap<UserForViewDto, User>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src =>
                src.RoleName != null ? new Role { Name = src.RoleName } : null))
            .ReverseMap()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src =>
                src.Role != null ? src.Role.Name : null));

        // Role
        CreateMap<RoleForCreationDto, Role>().ReverseMap();
        CreateMap<RoleForUpdateDto, Role>().ReverseMap();
        CreateMap<RoleForViewDto, Role>().ReverseMap();

        // Permission
        CreateMap<PermissionForCreateDto, Permission>().ReverseMap();
        CreateMap<PermissionForUpdateDto, Permission>().ReverseMap();
        CreateMap<PermissionForViewDto, Permission>().ReverseMap();

        // Course
        CreateMap<CourseForCreationDto, Course>().ReverseMap();
        CreateMap<CourseForUpdateDto, Course>().ReverseMap();
        CreateMap<CourseForViewDto, Course>().ReverseMap();

        // Lesson
        CreateMap<LessonForCreationDto, Lesson>().ReverseMap();
        CreateMap<LessonForUpdateDto, Lesson>().ReverseMap();
        CreateMap<LessonForViewDto, Lesson>()
            .ForMember(dest => dest.Course, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.CourseTitle) ? new Course { Title = src.CourseTitle } : null))
            .ReverseMap()
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src =>
                src.Course != null ? src.Course.Title : string.Empty));


        // LessonPart
        CreateMap<LessonPartForCreationDto, LessonPart>().ReverseMap();
        CreateMap<LessonPartForUpdateDto, LessonPart>().ReverseMap();
        CreateMap<LessonPartForViewDto, LessonPart>();

        // Homework
        CreateMap<HomeworkForCreationDto, Homework>().ReverseMap();
        CreateMap<HomeworkForUpdateDto, Homework>().ReverseMap();
        CreateMap<HomeworkForViewDto, Homework>()
            .ForMember(dest => dest.Lesson, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.LessonTitle) ? new Lesson { Title = src.LessonTitle } : null))
            .ReverseMap()
            .ForMember(dest => dest.LessonTitle, opt => opt.MapFrom(src =>
                src.Lesson != null ? src.Lesson.Title : string.Empty));


        // UserHomework
        CreateMap<UserHomeworkForCreationDto, UserHomework>().ReverseMap();
        CreateMap<UserHomeworkForUpdateDto, UserHomework>().ReverseMap();
        CreateMap<UserHomework, UserHomeworkForViewDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.HomeworkQuestion, opt => opt.MapFrom(src => src.Homework.Question))
            .ReverseMap()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Homework, opt => opt.Ignore());


        // UserCourse
        CreateMap<UserCourseForCreationDto, UserCourse>().ReverseMap();
        CreateMap<UserCourseForUpdateDto, UserCourse>().ReverseMap();
        CreateMap<UserCourse, UserCourseForViewDto>()
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
            .ReverseMap()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Course, opt => opt.Ignore());


        // Subscription
        CreateMap<SubscriptionForCreateDto, Subscription>().ReverseMap();
        CreateMap<SubscriptionForUpdateDto, Subscription>().ReverseMap();
        CreateMap<Subscription, SubscriptionForViewDto>()
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.EndDate >= DateTime.UtcNow))
            .ReverseMap()
            .ForMember(dest => dest.User, opt => opt.Ignore());


        //// PointTransaction
        //CreateMap<PointTransactionForCreationDto, PointTransaction>().ReverseMap();
        //CreateMap<PointTransactionForUpdateDto, PointTransaction>().ReverseMap();
        //CreateMap<PointTransactionForViewDto, PointTransaction>()
        //    .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null))
        //    .ReverseMap();

        //// DailyChallenge
        //CreateMap<DailyChallengeForCreationDto, DailyChallenge>().ReverseMap();
        //CreateMap<DailyChallengeForUpdateDto, DailyChallenge>().ReverseMap();
        //CreateMap<DailyChallengeForViewDto, DailyChallenge>().ReverseMap();

        //// Streak
        //CreateMap<StreakForCreationDto, Streak>().ReverseMap();
        //CreateMap<StreakForUpdateDto, Streak>().ReverseMap();
        //CreateMap<StreakForViewDto, Streak>()
        //    .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null))
        //    .ReverseMap();

        //// StreakLog
        //CreateMap<StreakLogForCreationDto, StreakLog>().ReverseMap();
        //CreateMap<StreakLogForUpdateDto, StreakLog>().ReverseMap();
        //CreateMap<StreakLogForViewDto, StreakLog>()
        //    .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null))
        //    .ReverseMap();

        //// MockTest
        //CreateMap<MockTestForCreationDto, MockTest>().ReverseMap();
        //CreateMap<MockTestForUpdateDto, MockTest>().ReverseMap();
        //CreateMap<MockTestForViewDto, MockTest>().ReverseMap();

        //// TestResult
        //CreateMap<ResultForCreationDto, TestResult>().ReverseMap();
        //CreateMap<ResultForUpdateDto, TestResult>().ReverseMap();
        //CreateMap<ResultForViewDto, TestResult>()
        //    .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(src => src.User != null ? src.User.Email : null))
        //    .ForMember(dest => dest.MockTestTitle, opt => opt.MapFrom(src => src.MockTest != null ? src.MockTest.Title : null))
        //    .ReverseMap();

    }
}
