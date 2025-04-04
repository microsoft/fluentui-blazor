// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

internal class CachedServices : IDisposable
{
    private readonly ConcurrentDictionary<Type, object> _services = new();
    private bool _disposedValue;

    /// <summary>
    /// Initializes a new instance of the CachedServices class.
    /// </summary>
    /// <param name="serviceProvider">Provides access to application services and resources.</param>
    public CachedServices(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// Gets the service provider used for dependency injection. It allows access to registered services.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Get service of type <typeparamref name="T"/> from the <see cref="IServiceProvider"/> or null if not found.
    /// Keep in mind that this method will cache the service in the component memory for future use.
    /// </summary>
    /// <typeparam name="T">The type of service object to get.</typeparam>
    /// <returns></returns>
    public T? GetCachedServiceOrNull<T>()
    {
        if (_services.ContainsKey(typeof(T)))
        {
            return (T)_services[typeof(T)];
        }

        var service = ServiceProvider.GetService<T>();

        if (service is null)
        {
            return default;
        }

        _services.TryAdd(typeof(T), service);
        return service;
    }

    public ITooltipService? TooltipService => GetCachedServiceOrNull<ITooltipService>();

    /// <summary />
    public async Task RenderTooltipAsync(IFluentComponentBase component, string? label)
    {
        if (TooltipService is null || string.IsNullOrEmpty(label) || component is FluentTooltip)
        {
            return;
        }

        if (string.IsNullOrEmpty(component.Id))
        {
            component.Id = Identifier.NewId();
        }

        var tooltip = new FluentTooltip(anchor: component.Id, $"<text>{label}</text>");

        TooltipService.Items.TryAdd(component.Id, tooltip);
        await TooltipService.OnUpdatedAsync.Invoke(tooltip);
    }

    /// <summary />
    public async Task DisposeTooltipAsync(IFluentComponentBase component)
    {
        if (TooltipService is null || string.IsNullOrEmpty(component.Id) || component is FluentTooltip)
        {
            return;
        }

        var isRemoved = TooltipService.Items.TryRemove(component.Id, out var tooltip);

        if (isRemoved && tooltip is not null)
        {
            await TooltipService.OnUpdatedAsync.Invoke(tooltip);
        }
    }

    /// <summary />
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // Dispose managed state (managed objects)
                _services.Clear();
            }

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Dispose the <see cref="CachedServices"/> object.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
