using Microsoft.Extensions.DependencyInjection;

namespace LMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register application services here as each module is implemented.
        return services;
    }
}

