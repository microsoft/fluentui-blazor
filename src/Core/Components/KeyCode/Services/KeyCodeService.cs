// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

public class KeyCodeService : IKeyCodeService
{
    private ReaderWriterLockSlim ServiceLock { get; } = new ReaderWriterLockSlim();
    private IList<(Guid, IKeyCodeListener)> ListenerList { get; } = new List<(Guid, IKeyCodeListener)>();

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

    public void UnregisterListener(IKeyCodeListener listener)
    {
        //_listeners.Remove(listener, out _);
    }

    public void Clear()
    {
        //_listeners.Clear();
    }

    public void Dispose()
    {        
        Clear();
    }

    private class KeyCodeListener : IKeyCodeListener
    {
        private readonly Func<FluentKeyCodeEventArgs, Task> _handler;

        public KeyCodeListener(Func<FluentKeyCodeEventArgs, Task> handler)
        {
            _handler = handler;
        }

        public Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
        {
            return _handler.Invoke(args);
        }
    }
}
