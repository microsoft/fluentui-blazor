using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using Microsoft.FluentUI.AspNetCore.Components.DesignTokens;

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
        services.AddScoped<IToastService, ToastService>();
        services.AddScoped<IDialogService, DialogService>();
        services.AddScoped<IMessageService, MessageService>();

        LibraryConfiguration options = configuration ?? new();
        if (options.UseTooltipServiceProvider)
        {
            services.AddScoped<ITooltipService, TooltipService>();
        }
        services.AddSingleton(options);

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
}
