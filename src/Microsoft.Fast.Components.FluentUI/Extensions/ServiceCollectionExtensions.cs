using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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
        services.AddScoped<GlobalState>();
        services.AddScoped<CacheStorageAccessor>();


        services.AddScoped<IStaticAssetService, HttpBasedStaticAssetService>();

        if (configuration is not null)
        {
            if (configuration.HostingModel != BlazorHostingModel.Hybrid && !string.IsNullOrEmpty(configuration.StaticAssetServiceConfiguration.BaseAddress))
                services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>(c =>
                {
                    c.BaseAddress = new Uri(configuration.StaticAssetServiceConfiguration.BaseAddress);
                });
        }
        else
            services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>();

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


        services.AddScoped<IStaticAssetService, HttpBasedStaticAssetService>();

        if (options is not null)
        {
            if (options.HostingModel != BlazorHostingModel.Hybrid && !string.IsNullOrEmpty(options.StaticAssetServiceConfiguration.BaseAddress))
                services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>(c =>
                {
                    c.BaseAddress = new Uri(options.StaticAssetServiceConfiguration.BaseAddress);
                });
        }
        else
            services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>();

        services.AddDesignTokens();

        return services;
    }
}