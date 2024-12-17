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
        var options = configuration ?? new();

        var serviceLifetime = options?.ServiceLifetime ?? ServiceLifetime.Scoped;
        if (serviceLifetime == ServiceLifetime.Transient)
        {
            throw new NotSupportedException("Transient lifetime is not supported for Fluent UI services.");
        }

        // Add services
        services.Add<LibraryConfiguration>(provider => options ?? new(), serviceLifetime);
        services.Add<IDialogService, DialogService>(serviceLifetime);
        services.Add<IFluentLocalizer>(provider => options?.Localizer ?? FluentLocalizerInternal.Default, serviceLifetime);

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

    /// <summary />
    private static IServiceCollection Add<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory, ServiceLifetime lifetime)
        where TService : class
    {
        return lifetime switch
        {
            ServiceLifetime.Singleton => services.AddSingleton(implementationFactory),
            ServiceLifetime.Scoped => services.AddScoped(implementationFactory),
            _ => throw new NotSupportedException($"Service lifetime {lifetime} is not supported.")
        };
    }

    /// <summary />
    private static IServiceCollection Add<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        return lifetime switch
        {
            ServiceLifetime.Singleton => services.AddSingleton<TService, TImplementation>(),
            ServiceLifetime.Scoped => services.AddScoped<TService, TImplementation>(),
            _ => throw new NotSupportedException($"Service lifetime {lifetime} is not supported.")
        };
    }
}
