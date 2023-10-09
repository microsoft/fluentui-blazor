using System.Runtime.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using Microsoft.FluentUI.AspNetCore.Components.DesignTokens;
using Microsoft.FluentUI.AspNetCore.Components.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;

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
        services.AddScoped<IToastService, ToastService>();
        services.AddScoped<IDialogService, DialogService>();
        services.AddScoped<IMessageService, MessageService>();

        if (configuration is not null)
        {
            if (configuration.UseTooltipServiceProvider)
            {
                services.AddScoped<ITooltipService, TooltipService>();
            }

            switch (configuration.HostingModel)
            {
                case BlazorHostingModel.Server:
                    if (string.IsNullOrEmpty(configuration.StaticAssetServiceConfiguration.BaseAddress))
                    {
#pragma warning disable CA1416 // Validate platform compatibility
                        services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>()
                            .ConfigurePrimaryHttpMessageHandler(GetHandler);
#pragma warning restore CA1416 // Validate platform compatibility
                    }
                    else
                    {
#pragma warning disable CA1416 // Validate platform compatibility
                        services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>(c =>
                        {
                            c.BaseAddress = new Uri(configuration.StaticAssetServiceConfiguration.BaseAddress);
                        }).ConfigurePrimaryHttpMessageHandler(GetHandler);
#pragma warning restore CA1416 // Validate platform compatibility
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

        return AddFluentUIComponents(services, options);
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