using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using Microsoft.FluentUI.AspNetCore.Components.DesignTokens;

// This namespace is deliberately "Components" and not "Components.Extensions".
namespace Microsoft.FluentUI.AspNetCore.Components;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add common services required by the Fluent UI Web Components for Blazor library
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Library configuration</param>
    /// <param name="serviceLifetime">In case you want to use any of the services outside of the request, you can change the lifetime to <see cref="ServiceLifetime.Singleton"/>. Normally all services are registered as <see cref="ServiceLifetime.Scoped"/>.</param>
    public static IServiceCollection AddFluentUIComponents(this IServiceCollection services, LibraryConfiguration? configuration = null, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        if(serviceLifetime == ServiceLifetime.Transient)
        {
            throw new ArgumentException("Transient lifetime is not supported for Fluent UI services.", nameof(serviceLifetime));
        }
        if (serviceLifetime == ServiceLifetime.Singleton)
        {
            services.AddSingleton<GlobalState>();
            services.AddSingleton<IToastService, ToastService>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<IKeyCodeService, KeyCodeService>();
            services.AddSingleton<IMenuService, MenuService>();
        }
        else
        {
            services.AddScoped<GlobalState>();
            services.AddScoped<IToastService, ToastService>();
            services.AddScoped<IDialogService, DialogService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IKeyCodeService, KeyCodeService>();
            services.AddScoped<IMenuService, MenuService>();
        }

        var options = configuration ?? new();
        if (options.UseTooltipServiceProvider)
        {
            if (serviceLifetime == ServiceLifetime.Singleton)
            {
                services.AddSingleton<ITooltipService, TooltipService>();
            }
            else
            {
                services.AddScoped<ITooltipService, TooltipService>();
            }
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
    /// <param name="serviceLifetime">In case you want to use any of the services outside of the request, you can change the lifetime to <see cref="ServiceLifetime.Singleton"/>. Normally all services are registered as <see cref="ServiceLifetime.Scoped"/>.</param>
    public static IServiceCollection AddFluentUIComponents(this IServiceCollection services, Action<LibraryConfiguration> configuration, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        LibraryConfiguration options = new();
        configuration.Invoke(options);

        return AddFluentUIComponents(services, options, serviceLifetime);
    }
}
