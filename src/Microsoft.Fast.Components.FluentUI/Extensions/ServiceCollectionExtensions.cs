using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Fast.Components.FluentUI.DesignTokens;
using Microsoft.Fast.Components.FluentUI.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

public static class ServiceCollectionExtensions
{

    /// <summary>
    /// Adds an Icon Service as a Scoped instance.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">Defines IconConfiguration for this instance.</param>
    public static IServiceCollection AddFluentIcons(this IServiceCollection services, IconConfiguration? configuration = null)
    {
        configuration ??= new IconConfiguration();

        services.AddScoped(builder => new IconService(configuration));

        return services;
    }

    /// <summary>
    /// Adds an Icon Service as a Scoped instance.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">Defines IconConfiguration for this instance.</param>
    public static IServiceCollection AddFluentIcons(this IServiceCollection services, Action<IconConfiguration> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        IconConfiguration? options = new();
        configuration(options);
        
        return AddFluentIcons(services, options);
    }

    /// <summary>
    /// Adds an Emoji Service as a Scoped instance.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">Defines EmojiConfiguration for this instance.</param>
    public static IServiceCollection AddFluentEmojis(this IServiceCollection services, EmojiConfiguration? configuration = null)
    {
        configuration ??= new EmojiConfiguration();

        services.AddScoped(builder => new EmojiService(configuration));

        return services;
    }

    /// <summary>
    /// Adds an Emoji Service as a Scoped instance.
    /// </summary>
    /// <param name="services">IServiceCollection</param>
    /// <param name="configuration">Defines EmojiConfiguration for this instance.</param>
    public static IServiceCollection AddFluentEmojis(this IServiceCollection services, Action<EmojiConfiguration> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        EmojiConfiguration? options = new();
        configuration(options);

        return AddFluentEmojis(services, options);
    }

    
    /// <summary>
    /// Add common servIconConfiguration?s required by the Fluent UI Web Components for Blazor library
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Library configuration</param>
    public static IServiceCollection AddFluentUIComponents(this IServiceCollection services, LibraryConfiguration? configuration = null)
    {
        services.AddScoped<GlobalState>();
        services.AddScoped<CacheStorageAccessor>();


        services.AddScoped<IStaticAssetService, HttpBasedStaticAssetService>();

        services.AddFluentIcons(configuration?.IconConfiguration);
        services.AddFluentEmojis(configuration?.EmojiConfiguration);

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

        services.AddFluentIcons(options?.IconConfiguration);
        services.AddFluentEmojis(options?.EmojiConfiguration);

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