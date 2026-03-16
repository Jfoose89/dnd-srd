namespace dnd_srd.Services
{
    public static class CacheExtensions
    {
        public static IServiceCollection AddCachingPolicies(this IServiceCollection services)
        {
            services.AddMemoryCache();
            return services;
        }
    }
}
