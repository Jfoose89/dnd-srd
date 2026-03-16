using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace dnd_srd.Services;

public static class CorsExtensions
{
    public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DndSrdPolicy", policy =>
            {
                policy
                    .WithOrigins(
                        "http://localhost:3000",
                        "http://localhost:5173",
                        "https://localhost:7120"
                    )
                    .WithMethods("GET", "POST", "PUT", "DELETE")
                    .WithHeaders("Content-Type", "X-Api-Key");
            });
        });

        return services;
    }
}