using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeAutomation;

public static class ServiceCollectionsExtensions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Style",
        "IDE0060:Remove unused parameter",
        Justification = "Example."
    )]
    public static IServiceCollection AddHomeAutomation(
        this IServiceCollection services,
        HostBuilderContext context
    )
    {
        services.AddSingleton(TimeProvider.System);
        return services;
    }
}
