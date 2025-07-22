using Auth.DataAccess.Interface;
using Auth.DataAccess.Repository;

namespace Auth.Api.Extensions;

public static class ServiceExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        // Repository
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }
}
