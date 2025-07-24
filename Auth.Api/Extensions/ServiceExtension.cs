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
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ILessonService, LessonService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IPermissionService, PermissionService>();
        services.AddTransient<ICourseService, CourseService>();
        services.AddTransient<ILessonPartService, LessonPartService>();
    }
}
