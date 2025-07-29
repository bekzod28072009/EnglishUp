using Auth.DataAccess.Interface;
using Auth.DataAccess.Repository;
using Auth.Service.Interfaces;
using Auth.Service.Services;

namespace Auth.Api.Extensions;

public static class ServiceExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        // Repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Services
        services.AddTransient<ICourseService, CourseService>();
        services.AddTransient<IDaliyChallengeService, DailyChallengeService>();
        services.AddTransient<IHomeworkService, HomeworkService>();
        services.AddTransient<ILessonPartService, LessonPartService>();
        services.AddTransient<ILessonService, LessonService>();
        services.AddTransient<IMockTestService, MockTestService>();
        services.AddTransient<IPermissionService, PermissionService>();
        services.AddTransient<IPointTransactionService, PointTransactionService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IStreakLogService, StreakLogService>();
        services.AddTransient<IStreakService, StreakService>();
        services.AddTransient<ISubscriptionService, SubscriptionService>();
        services.AddTransient<ITestResultService, TestResultService>();
        services.AddTransient<IUserCourseService, UserCourseService>();
        services.AddTransient<IUserHomeworkService, UserHomeworkService>();
        services.AddTransient<IUserService, UserService>();
    }
}
