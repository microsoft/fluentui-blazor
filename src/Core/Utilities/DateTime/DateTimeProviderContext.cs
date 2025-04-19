// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections.Immutable;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the context for the <see cref="DateTimeProvider" />.
/// This context returns the specified date and time while it is in scope.
/// </summary>
public record DateTimeProviderContext : IDisposable
{
    private static readonly AsyncLocal<ImmutableStack<DateTimeProviderContext>> s_asyncScopeStack = new();
    private readonly AsyncLocal<uint> _asyncCurrentIndex = new();

    /// <summary>
    /// Gets the current <see cref="DateTimeProviderContext" />.
    /// </summary>
    internal static DateTimeProviderContext? Current => s_asyncScopeStack.Value?.IsEmpty == false ? s_asyncScopeStack.Value.Peek() : null;

    /// <summary>
    /// Create a new context for the <see cref="DateTimeProvider" /> using a sequence of date and time.
    /// </summary>
    /// <param name="sequence">Sequence of date and time to return while in scope.</param>
    public DateTimeProviderContext(Func<uint, DateTime> sequence)
    {
        s_asyncScopeStack.Value = (s_asyncScopeStack.Value ?? ImmutableStack<DateTimeProviderContext>.Empty).Push(this);
        Sequence = sequence;
    }

    /// <summary>
    /// Create a new context for the <see cref="DateTimeProvider" /> using the specified date and time.
    /// </summary>
    /// <param name="value">Specifies the date and time to return while in scope.</param>
    public DateTimeProviderContext(DateTime value) : this(_ => value) { }

    /// <summary>
    /// Create a new context for the <see cref="DateTimeProvider" /> using a list of date and time.
    /// Each call to <see cref="DateTimeProvider.Now" /> will return the next date and time in the list,
    /// until the last date and time is reached.
    /// If more calls are made after the last date and time, an <see cref="InvalidOperationException" /> is thrown.
    /// </summary>
    /// <param name="values"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public DateTimeProviderContext(DateTime[] values)
        : this(i => i < values.Length
                  ? values[i]
                  : throw new InvalidOperationException("This is the last call in the sequence. No more dates are available."))
    { }

    /// <summary>
    /// Gets the date and time to return while in scope.
    /// </summary>
    internal Func<uint, DateTime> Sequence { get; }

    /// <summary>
    /// Returns the next number between 0 and 99999999.
    /// </summary>
    /// <returns></returns>
    internal DateTime NextValue()
    {
        var currentIndex = _asyncCurrentIndex.Value;
        var value = Sequence.Invoke(currentIndex);

        _asyncCurrentIndex.Value = currentIndex >= uint.MaxValue ? 0 : currentIndex + 1;

        return value;
    }

    /// <summary>
    /// Force the next value to used with the NextValue method.
    /// Only for testing purposes.
    /// </summary>
    /// <param name="value"></param>
    internal void ForceNextValue(uint value)
    {
        _asyncCurrentIndex.Value = value;
    }

    /// <summary>
    /// Disposes the <see cref="DateTimeProviderContext" />
    /// and return to the previous context.
    /// </summary>
    public void Dispose()
    {
        if (s_asyncScopeStack.Value?.IsEmpty == false)
        {
            s_asyncScopeStack.Value = s_asyncScopeStack.Value.Pop();
        }
    }
}
