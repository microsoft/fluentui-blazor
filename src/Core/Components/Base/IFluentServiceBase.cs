// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Common interface for a service provider (DialogService, MenuService, ...).
/// </summary>
/// <typeparam name="TComponent"></typeparam>
public interface IFluentServiceBase<TComponent> : IDisposable
{
    /// <summary>
    /// Gets the Provider ID.
    /// This value is set by the provider and will be empty if the provider is not initialized.
    /// </summary>
    public string? ProviderId { get; internal set; }

    /// <summary>
    /// Gets the list of <typeparamref name="TComponent"/>.
    /// </summary>
    internal ConcurrentDictionary<string, TComponent> Items { get; }

    /// <summary>
    /// Action to be called when the <typeparamref name="TComponent"/> is updated.
    /// </summary>
    internal Func<TComponent, Task> OnUpdatedAsync { get; set; }
}
