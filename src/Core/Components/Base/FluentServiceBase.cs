// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// <see cref="IFluentServiceBase{TComponent}" />
/// </summary>
/// <typeparam name="TComponent"></typeparam>
public abstract class FluentServiceBase<TComponent> : IFluentServiceBase<TComponent> where TComponent : FluentComponentBase
{
    private readonly ReaderWriterLockSlim _lock = new();
    private readonly IList<TComponent> _list = [];

    /// <summary>
    /// <see cref="IFluentServiceBase{TComponent}.ProviderId" />
    /// </summary>
    string? IFluentServiceBase<TComponent>.ProviderId { get; set; }

    /// <summary>
    /// <see cref="IFluentServiceBase{TComponent}.Items" />
    /// </summary>
    IEnumerable<TComponent> IFluentServiceBase<TComponent>.Items
    {
        get
        {
            _lock.EnterReadLock();
            try
            {
                return _list;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }

    /// <summary>
    /// <see cref="IFluentServiceBase{TComponent}.OnUpdatedAsync" />
    /// </summary>
    Func<TComponent, Task> IFluentServiceBase<TComponent>.OnUpdatedAsync { get; set; } = (item) => Task.CompletedTask;

    /// <summary>
    /// <see cref="IFluentServiceBase{TComponent}.Add(TComponent)" />
    /// </summary>
    void IFluentServiceBase<TComponent>.Add(TComponent item)
    {
        _lock.EnterWriteLock();
        try
        {
            _list.Add(item);
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// <see cref="IFluentServiceBase{TComponent}.Remove(TComponent)" />
    /// </summary>
    void IFluentServiceBase<TComponent>.Remove(TComponent item)
    {
        _lock.EnterWriteLock();
        try
        {
            var firstItem = _list.FirstOrDefault(i => string.Equals(i.Id, item.Id, StringComparison.OrdinalIgnoreCase));
            if (firstItem != null)
            {
                _list.Remove(firstItem);
            }
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// <see cref="IFluentServiceBase{TComponent}.Clear()" />
    /// </summary>
    void IFluentServiceBase<TComponent>.Clear()
    {
        _lock.EnterWriteLock();
        try
        {
            _list.Clear();
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Gets the current instance of the service,
    /// converted to the <see cref="IFluentServiceBase{TComponent}"/> interface.
    /// </summary>
    internal IFluentServiceBase<TComponent> InternalService => this;

    /// <summary>
    /// <see cref="IDisposable.Dispose" />
    /// </summary>
    public void Dispose()
    {
        InternalService.Clear();
    }
}
