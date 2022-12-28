using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Fast.Components.FluentUI.DesignTokens;
using Microsoft.Fast.Components.FluentUI.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add common services required by the Fluent UI Web Components for Blazor library
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Library configuration</param>
    public static IServiceCollection AddFluentUIComponents(this IServiceCollection services, LibraryConfiguration? configuration = null)
    {
        configuration ??= new LibraryConfiguration();
        
        services.AddScoped<GlobalState>();
        services.AddScoped<CacheStorageAccessor>();


        services.AddScoped<IStaticAssetService, StaticAssetService>();
        services.AddHttpClient<IStaticAssetService, StaticAssetService>();

        services.AddDesignTokens();

        return services;
    }

    /// <summary>
    /// Add common services required by the Fluent UI Web Components for Blazor library
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Library configuration</param>
    public static IServiceCollection AddFluentUIComponents(this IServiceCollection services, Action<LibraryConfiguration?> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        LibraryConfiguration? options = new();
        configuration.Invoke(options);
        

        services.AddScoped<GlobalState>();
        services.AddScoped<CacheStorageAccessor>();


        services.AddScoped<IStaticAssetService, StaticAssetService>();
        services.AddHttpClient<IStaticAssetService, StaticAssetService>();
        

        services.AddDesignTokens();

        return services;
    }
}