namespace Microsoft.Fast.Components.FluentUI;

public interface IObservableValue<TValue> where TValue : class, IEquatable<TValue>
{
    TValue? Value { get; }
    void Subscribe(Func<TValue?, Task> handler);
    void Unsubscribe(Func<TValue?, Task> handler);
}