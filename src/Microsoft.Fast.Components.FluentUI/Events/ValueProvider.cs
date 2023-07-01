namespace Microsoft.Fast.Components.FluentUI;

public class ValueProvider<TValue> : IObservableValue<TValue> where TValue : class, IEquatable<TValue>
{
    public TValue? Value { get; private set; }

    private readonly object _syncRoot = new object();
    private readonly List<Func<TValue?, Task>> _handlers = new List<Func<TValue?, Task>>();

    public void Subscribe(Func<TValue?, Task> handler)
    {
        lock (_syncRoot)
        {
            _handlers.Add(handler);
        }
    }

    public void Unsubscribe(Func<TValue?, Task> handler)
    {
        lock (_syncRoot)
        {
            _handlers.Remove(handler);
        }
    }

    public async Task SetValueAsync(TValue value)
    {
        if (value?.Equals(Value) == true)
            return;

        lock (_syncRoot)
        {
            Value = value;
        }

        await ValueChangedAsync();
    }

    private async Task ValueChangedAsync()
    {
        TValue? currentValue = Value;

        Func<TValue, Task>[] currentHandlers;
        lock (_syncRoot)
        {
            currentHandlers = _handlers.ToArray();
        }

        foreach (var handler in _handlers)
        {
            await handler(currentValue);
        }
    }
}

