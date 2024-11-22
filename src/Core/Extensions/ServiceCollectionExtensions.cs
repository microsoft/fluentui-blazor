// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides methods to add services required by the Fluent UI Web Components for Blazor library
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add common services required by the Fluent UI Web Components for Blazor library
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Library configuration</param>
    public static IServiceCollection AddFluentUIComponents(this IServiceCollection services, LibraryConfiguration? configuration = null)
    {
        /*
            services.AddScoped<GlobalState>();
            services.AddScoped<IToastService, ToastService>();
            services.AddScoped<IDialogService, DialogService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IKeyCodeService, KeyCodeService>();

            var options = configuration ?? new();
            if (options.UseTooltipServiceProvider)
            {
                services.AddScoped<ITooltipService, TooltipService>();
            }
            services.AddSingleton(options);
        */

        services.AddScoped<FluentLocalizer>(provider => configuration?.Localizer ?? new FluentLocalizer());

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
}
