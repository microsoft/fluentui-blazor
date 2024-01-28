using Microsoft.Extensions.DependencyInjection;

namespace FluentUI.Demo.Shared;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add common services required by the Fluent UI Web Components for Blazor library
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Library configuration</param>
    public static IServiceCollection AddFluentUIDemoServices(this IServiceCollection services)
    {
        services.AddScoped<CacheStorageAccessor>();
        services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>();

        return services;
    }
}
