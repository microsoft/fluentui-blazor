using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        services.AddFluentIcons(configuration?.IconConfiguration);
        services.AddFluentEmojis(configuration?.EmojiConfiguration);

        if (configuration is not null)
        {
            switch (configuration.HostingModel)
            {
                case BlazorHostingModel.Server:
#pragma warning disable CA1416 // Validate platform compatibility
                    HttpClientHandler? handler = GetHandler();
#pragma warning restore CA1416 // Validate platform compatibility
                    if (string.IsNullOrEmpty(configuration.StaticAssetServiceConfiguration.BaseAddress))
                    {
                        services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>()
                            .ConfigurePrimaryHttpMessageHandler(() =>
                            {
                                return handler;
                            });
                    }
                    else
                    {
                        services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>(c =>
                        {
                            c.BaseAddress = new Uri(configuration.StaticAssetServiceConfiguration.BaseAddress);

                        }).ConfigurePrimaryHttpMessageHandler(() =>
                        {
                            return handler;
                        });

                    }
                    break;
                case BlazorHostingModel.WebAssembly:
                    if (string.IsNullOrEmpty(configuration.StaticAssetServiceConfiguration.BaseAddress))
                    {
                        services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>();
                    }
                    else
                    {
                        services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>(c =>
                        {
                            c.BaseAddress = new Uri(configuration.StaticAssetServiceConfiguration.BaseAddress);

                        });
                    }
                    break;
                case BlazorHostingModel.NotSpecified:
                    services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>();
                    break;
                case BlazorHostingModel.Hybrid:
                    break;
            }
        }
        else
        {
            services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>();
        }
                

        services.AddDesignTokens();

        return services;
    }

    /// <summary>
    /// Add common services required by the Fluent UI Web Components for Blazor library
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Library configuration</param>
    public static IServiceCollection AddFluentUIComponents(this IServiceCollection services, Action<LibraryConfiguration> configuration)
    {
        LibraryConfiguration options = new();
        configuration.Invoke(options);

        services.AddScoped<GlobalState>();
        services.AddScoped<CacheStorageAccessor>();
        services.AddFluentIcons(options.IconConfiguration);
        services.AddFluentEmojis(options.EmojiConfiguration);

        if (options is not null)
        {
            switch (options.HostingModel)
            {
                case BlazorHostingModel.Server:
#pragma warning disable CA1416 // Validate platform compatibility
                    HttpClientHandler? handler = GetHandler();
#pragma warning restore CA1416 // Validate platform compatibility
                    if (string.IsNullOrEmpty(options.StaticAssetServiceConfiguration.BaseAddress))
                    {
                        services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>()
                            .ConfigurePrimaryHttpMessageHandler(() =>
                            {
                                return handler;
                            });
                    }
                    else
                    {
                        services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>(c =>
                        {
                            c.BaseAddress = new Uri(options.StaticAssetServiceConfiguration.BaseAddress);

                        }).ConfigurePrimaryHttpMessageHandler(() =>
                        {
                            return handler;
                        });

                    }
                    break;
                case BlazorHostingModel.WebAssembly:
                    if (string.IsNullOrEmpty(options.StaticAssetServiceConfiguration.BaseAddress))
                    {
                        services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>();
                    }
                    else
                    {
                        services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>(c =>
                        {
                            c.BaseAddress = new Uri(options.StaticAssetServiceConfiguration.BaseAddress);

                        });
                    }
                    break;
                case BlazorHostingModel.NotSpecified:
                    services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>();
                    break;
                case BlazorHostingModel.Hybrid:
                    break;
            }
        }
        else
            services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>();

        services.AddDesignTokens();

        return services;
    }

    [UnsupportedOSPlatform("browser")]
    private static HttpClientHandler GetHandler()
    {
        return new()
        {
            UseDefaultCredentials = true
        };
    }
}