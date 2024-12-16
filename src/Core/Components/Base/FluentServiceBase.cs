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
    public virtual string? ProviderId { get; set; }

    /// <summary>
    /// <see cref="IFluentServiceBase{TComponent}.Items" />
    /// </summary>
    public virtual IEnumerable<TComponent> Items
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
    public virtual Func<TComponent, Task> OnUpdatedAsync { get; set; } = (item) => Task.CompletedTask;

    /// <summary>
    /// <see cref="IFluentServiceBase{TComponent}.Add(TComponent)" />
    /// </summary>
    public virtual void Add(TComponent item)
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
    public virtual void Remove(TComponent item)
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
    public virtual void Clear()
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
}
