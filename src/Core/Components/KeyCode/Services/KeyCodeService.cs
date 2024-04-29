// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.Fast.Components.FluentUI;

/// <inheritdoc cref="IKeyCodeService" />
public class KeyCodeService : IKeyCodeService
{
    private ReaderWriterLockSlim ServiceLock { get; } = new ReaderWriterLockSlim();

    private IList<(Guid, IKeyCodeListener)> ListenerList { get; } = new List<(Guid, IKeyCodeListener)>();

    /// <inheritdoc cref="IKeyCodeService.Listeners" />
    public IEnumerable<IKeyCodeListener> Listeners
    {
        get
        {
            ServiceLock.EnterReadLock();
            try
            {
                return ListenerList.Select(i => i.Item2);
            }
            finally
            {
                ServiceLock.ExitReadLock();
            }
        }
    }

    /// <inheritdoc cref="IKeyCodeService.RegisterListener(IKeyCodeListener)" />
    public Guid RegisterListener(IKeyCodeListener listener)
    {
        ServiceLock.EnterWriteLock();
        try
        {
            var id = Guid.NewGuid();
            ListenerList.Add((id, listener));
            return id;
        }
        finally
        {
            ServiceLock.ExitWriteLock();
        }
    }

    /// <inheritdoc cref="IKeyCodeService.RegisterListener(Func{FluentKeyCodeEventArgs, Task})" />
    public Guid RegisterListener(Func<FluentKeyCodeEventArgs, Task> handler)
    {
        ServiceLock.EnterWriteLock();
        try
        {
            var id = Guid.NewGuid();
            var listener = new KeyCodeListener(handler);
            ListenerList.Add((id, listener));
            return id;
        }
        finally
        {
            ServiceLock.ExitWriteLock();
        }
    }

    /// <inheritdoc cref="IKeyCodeService.UnregisterListener(IKeyCodeListener)" />
    public void UnregisterListener(IKeyCodeListener listener)
    {
        var item = ListenerList.FirstOrDefault(i => i.Item2 == listener);

        if (item.Item1 != Guid.Empty)
        {
            ListenerList.Remove(item);
        }
    }

    /// <inheritdoc cref="IKeyCodeService.UnregisterListener(Func{FluentKeyCodeEventArgs, Task})" />
    public void UnregisterListener(Func<FluentKeyCodeEventArgs, Task> handler)
    {
        var item = ListenerList.FirstOrDefault(i => (i.Item2 as KeyCodeListener)?.Handler == handler);

        if (item.Item1 != Guid.Empty)
        {
            ListenerList.Remove(item);
        }
    }

    /// <inheritdoc cref="IKeyCodeService.Clear" />
    public void Clear()
    {
        ListenerList.Clear();
    }

    /// <summary>
    /// Dispose the service and unregister all listeners.
    /// </summary>
    public void Dispose()
    {
        Clear();
    }

    /// <summary>
    /// Private KeyCodeListener
    /// </summary>
    private class KeyCodeListener : IKeyCodeListener
    {
        public KeyCodeListener(Func<FluentKeyCodeEventArgs, Task> handler)
        {
            Handler = handler;
        }

        public Func<FluentKeyCodeEventArgs, Task> Handler { get; }

        public Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
        {
            return Handler.Invoke(args);
        }
    }
}
