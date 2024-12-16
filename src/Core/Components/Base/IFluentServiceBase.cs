// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Common interface for a service provider (DialogService, MenuService, ...).
/// </summary>
/// <typeparam name="TComponent"></typeparam>
public interface IFluentServiceBase<TComponent> where TComponent : FluentComponentBase
{
    /// <summary>
    /// Gets or sets the Provider ID.
    /// This value is set by the provider and will be empty if the provider is not initialized.
    /// </summary>
    public string? ProviderId { get; set; }

    /// <summary>
    /// Gets the list of <typeparamref name="TComponent"/>.
    /// </summary>
    IEnumerable<TComponent> Items { get; }

    /// <summary>
    /// Action to be called when the <typeparamref name="TComponent"/> is updated.
    /// </summary>
    Func<TComponent, Task> OnUpdatedAsync { get; set; }

    /// <summary>
    /// Add a <typeparamref name="TComponent"/> to the list.
    /// </summary>
    /// <param name="item"></param>
    void Add(TComponent item);

    /// <summary>
    /// Remove a <typeparamref name="TComponent"/> from the list.
    /// </summary>
    /// <param name="item"></param>
    void Remove(TComponent item);

    /// <summary>
    /// Clear the list of <typeparamref name="TComponent"/>.
    /// </summary>
    void Clear();
}
