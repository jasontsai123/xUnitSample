using Microsoft.Extensions.DependencyInjection.Extensions;

namespace xUnitSample.Repository.DI
{
    public static class RepositoryServiceCollectionExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.TryAddSingleton<IUserRepository, UserRepository>();
            return services;
        }
    }
}
