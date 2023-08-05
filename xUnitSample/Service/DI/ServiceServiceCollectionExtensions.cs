using Microsoft.Extensions.DependencyInjection.Extensions;

namespace xUnitSample.Service.DI
{
    public static class ServiceServiceCollectionExtensions
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.TryAddScoped<IUserService, UserService>();
            return services;
        }
    }
}
