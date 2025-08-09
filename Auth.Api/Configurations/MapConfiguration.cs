using Auth.Domain.Entities.Courses;
using Auth.Domain.Entities.Gamification;
using Auth.Domain.Entities.Homeworks;
using Auth.Domain.Entities.Permissions;
using Auth.Domain.Entities.Roles;
using Auth.Domain.Entities.Subscriptions;
using Auth.Domain.Entities.Tests;
using Auth.Domain.Entities.UserManagement;
using Auth.Service.DTOs.Courses.CourseCommentsDto;
using Auth.Service.DTOs.Courses.CourseLevelsDto;
using Auth.Service.DTOs.Courses.CoursesDto;
using Auth.Service.DTOs.Courses.LessonsDto;
using Auth.Service.DTOs.Courses.UserCoursesDto;
using Auth.Service.DTOs.Gamification.DailyChallengesDto;
using Auth.Service.DTOs.Gamification.StreakLogDto;
using Auth.Service.DTOs.Gamification.StreaksDto;
using Auth.Service.DTOs.Homeworks.HomeworksDto;
using Auth.Service.DTOs.Homeworks.UserHomeworksDto;
using Auth.Service.DTOs.Permissions;
using Auth.Service.DTOs.Roles;
using Auth.Service.DTOs.SubscriptionPlans;
using Auth.Service.DTOs.Subscriptions;
using Auth.Service.DTOs.Tests.MockTestResultsDto;
using Auth.Service.DTOs.Tests.MockTestsDto;
using Auth.Service.DTOs.UserChallenges;
using Auth.Service.DTOs.Users;
using AutoMapper;

namespace Auth.Api.Configurations;

public class MapConfiguration : Profile
{
    public MapConfiguration()
    {
        // Course
        CreateMap<Course, CourseForViewDto>().ReverseMap();
        CreateMap<Course, CourseForCreationDto>().ReverseMap();
        CreateMap<Course, CourseForUpdateDto>().ReverseMap();

        // CourseComment
        CreateMap<CourseComment, CourseCommentForViewDto>().ReverseMap();
        CreateMap<CourseComment, CourseCommentForCreationDto>().ReverseMap();
        CreateMap<CourseComment, CourseCommentForUpdateDto>().ReverseMap();

        // CourseLevel
        CreateMap<CourseLevel, CourseLevelForViewDto>().ReverseMap();
        CreateMap<CourseLevel, CourseLevelForCreationDto>().ReverseMap();
        CreateMap<CourseLevel, CourseLevelForUpdateDto>().ReverseMap();

        // Lesson
        CreateMap<LessonForCreationDto, Lesson>().ReverseMap();
        CreateMap<LessonForUpdateDto, Lesson>()
            .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<LessonForViewDto, Lesson>()
            .ForMember(dest => dest.Course, opt => opt.MapFrom(src =>
                !string.IsNullOrEmpty(src.CourseTitle) ? new Course { Title = src.CourseTitle } : null))
            .ReverseMap()
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src =>
                src.Course != null ? src.Course.Title : string.Empty));

        // UserCourse
        CreateMap<UserCourseForCreationDto, UserCourse>().ReverseMap();
        CreateMap<UserCourseForUpdateDto, UserCourse>().ReverseMap();
        CreateMap<UserCourse, UserCourseForViewDto>()
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
            .ReverseMap()
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Course, opt => opt.Ignore());


        // DailyChallenge
        CreateMap<DailyChallengge, DailyChallengeForViewDto>().ReverseMap();
        CreateMap<DailyChallengge, DailyChallengeForCreateDto>().ReverseMap();
        CreateMap<DailyChallengge, DailyChallengeForUpdateDto>().ReverseMap();

        // Streak
        CreateMap<Streak, StreakForViewDto>().ReverseMap();
        CreateMap<Streak, StreakForCreationDto>().ReverseMap();
        CreateMap<Streak, StreakForUpdateDto>().ReverseMap();

        // StreakLog
        CreateMap<StreakLog, StreakLogForViewDto>().ReverseMap();
        CreateMap<StreakLog, StreakLogForCreationDto>().ReverseMap();
        CreateMap<StreakLog, StreakLogForUpdateDto>().ReverseMap();

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


        // Permission
        CreateMap<Permission, PermissionForViewDto>().ReverseMap();
        CreateMap<Permission, PermissionForCreateDto>().ReverseMap();
        CreateMap<Permission, PermissionForUpdateDto>().ReverseMap();

        // Role
        CreateMap<Role, RoleForViewDto>().ReverseMap();
        CreateMap<Role, RoleForCreationDto>().ReverseMap();
        CreateMap<Role, RoleForUpdateDto>().ReverseMap();

        // Subscription
        CreateMap<SubscriptionForCreateDto, Subscription>().ReverseMap();
        CreateMap<SubscriptionForUpdateDto, Subscription>().ReverseMap();
        CreateMap<Subscription, SubscriptionForViewDto>()
            .ForMember(dest => dest.UserFullName,
                opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.PlanName,
                opt => opt.MapFrom(src => src.Plan.Name))
            .ForMember(dest => dest.PlanPrice,
                opt => opt.MapFrom(src => src.Plan.Price))
            .ForMember(dest => dest.IsActive,
                opt => opt.MapFrom(src => src.EndDate >= DateTime.UtcNow));


        // SubscriptionPlan
        CreateMap<SubscriptionPlan, SubscriptionPlanForViewDto>().ReverseMap();
        CreateMap<SubscriptionPlan, SubscriptionPlanForCreationDto>().ReverseMap();
        CreateMap<SubscriptionPlan, SubscriptionPlanForUpdateDto>().ReverseMap();

        // MockTest
        CreateMap<MockTest, MockTestForViewDto>().ReverseMap();
        CreateMap<MockTest, MockTestForCreationDto>().ReverseMap();
        CreateMap<MockTest, MockTestForUpdateDto>().ReverseMap();

        // TestResult
        CreateMap<TestResult, ResultForViewDto>().ReverseMap();
        CreateMap<TestResult, ResultForCreationDto>().ReverseMap();
        CreateMap<TestResult, ResultForUpdateDto>().ReverseMap();

        // User
        CreateMap<UserForCreationDto, User>().ReverseMap();
        CreateMap<UserForUpdateDto, User>().ReverseMap();
        CreateMap<UserForViewDto, User>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src =>
                src.RoleName != null ? new Role { Name = src.RoleName } : null))
            .ReverseMap()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src =>
                src.Role != null ? src.Role.Name : null));


        // UserChallenge
        CreateMap<UserChallenge, UserChallengeForViewDto>().ReverseMap();
        CreateMap<UserChallenge, UserChallengeForCreationDto>().ReverseMap();


    }
}
