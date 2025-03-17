// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// <see cref="IFluentServiceBase{TComponent}" />
/// </summary>
/// <typeparam name="TComponent"></typeparam>
public abstract class FluentServiceBase<TComponent> : IFluentServiceBase<TComponent>
{
    private readonly ConcurrentDictionary<string, TComponent> _list = [];

    /// <summary>
    /// <see cref="IFluentServiceBase{TComponent}.ProviderId" />
    /// </summary>
    string? IFluentServiceBase<TComponent>.ProviderId { get; set; }

    /// <summary>
    /// <see cref="IFluentServiceBase{TComponent}.Items" />
    /// </summary>
    ConcurrentDictionary<string, TComponent> IFluentServiceBase<TComponent>.Items => _list;

    /// <summary>
    /// <see cref="IFluentServiceBase{TComponent}.OnUpdatedAsync" />
    /// </summary>
    Func<TComponent, Task> IFluentServiceBase<TComponent>.OnUpdatedAsync { get; set; } = (item) => Task.CompletedTask;

    /// <summary>
    /// Gets the current instance of the service,
    /// converted to the <see cref="IFluentServiceBase{TComponent}"/> interface.
    /// </summary>
    internal IFluentServiceBase<TComponent> ServiceProvider => this;

    /// <summary>
    /// <see cref="IDisposable.Dispose" />
    /// </summary>
    public void Dispose()
    {
        ServiceProvider.Items.Clear();
    }
}
