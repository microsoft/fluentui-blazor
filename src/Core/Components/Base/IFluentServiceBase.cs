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
    /// Gets or sets the Provider ID.
    /// This value is set by the provider and will be empty if the provider is not initialized.
    /// </summary>
    string? ProviderId { get; set; }

    /// <summary>
    /// Gets the list of <typeparamref name="TComponent"/>.
    /// </summary>
    ConcurrentDictionary<string, TComponent> Items { get; }

    /// <summary>
    /// Action to be called when the <typeparamref name="TComponent"/> is updated.
    /// </summary>
    Func<TComponent, Task> OnUpdatedAsync { get; set; }
}
