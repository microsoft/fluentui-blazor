// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

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
            var listener = new KeyCodeListener(handler, null);
            ListenerList.Add((id, listener));
            return id;
        }
        finally
        {
            ServiceLock.ExitWriteLock();
        }
    }

    /// <inheritdoc cref="IKeyCodeService.RegisterListener(Func{FluentKeyCodeEventArgs, Task}, Func{FluentKeyCodeEventArgs, Task})" />
    public Guid RegisterListener(Func<FluentKeyCodeEventArgs, Task> keyDownHandler, Func<FluentKeyCodeEventArgs, Task> keyUpHandler)
    {
        ServiceLock.EnterWriteLock();
        try
        {
            var id = Guid.NewGuid();
            var listener = new KeyCodeListener(keyDownHandler, keyUpHandler);
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
        var item = ListenerList.FirstOrDefault(i => (i.Item2 as KeyCodeListener)?.HandlerKeyDown == handler);

        if (item.Item1 != Guid.Empty)
        {
            ListenerList.Remove(item);
        }
    }

    /// <inheritdoc cref="IKeyCodeService.UnregisterListener(Func{FluentKeyCodeEventArgs, Task}, Func{FluentKeyCodeEventArgs, Task})" />
    public void UnregisterListener(Func<FluentKeyCodeEventArgs, Task> keyDownHandler, Func<FluentKeyCodeEventArgs, Task> keyUpHandler)
    {
        var item = ListenerList.FirstOrDefault(i => (i.Item2 as KeyCodeListener)?.HandlerKeyDown == keyDownHandler);

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
        public KeyCodeListener(Func<FluentKeyCodeEventArgs, Task> handlerKeyDown, Func<FluentKeyCodeEventArgs, Task>? handlerKeyUp)
        {
            HandlerKeyDown = handlerKeyDown;
            HandlerKeyUp = handlerKeyUp;
        }

        public Func<FluentKeyCodeEventArgs, Task> HandlerKeyDown { get; }

        public Func<FluentKeyCodeEventArgs, Task>? HandlerKeyUp { get; }

        public Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
        {
            if (HandlerKeyDown != null)
            {
                return HandlerKeyDown.Invoke(args);
            }

            return Task.CompletedTask;
        }

        public Task OnKeyUpAsync(FluentKeyCodeEventArgs args)
        {
            if (HandlerKeyUp != null)
            {
                return HandlerKeyUp.Invoke(args);
            }

            return Task.CompletedTask;
        }
    }
}
