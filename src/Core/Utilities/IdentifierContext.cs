// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Utilities;

// Inspiration: https://dvoituron.com/2020/01/22/UnitTest-DateTime/

/// <summary>
/// IdentifierContext is a class that allows you to generate sequential identifiers.
/// </summary>
public sealed class IdentifierContext : IDisposable
{
    private static readonly ThreadLocal<Stack<IdentifierContext>> _threadScopeStack = new(() => new Stack<IdentifierContext>());

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentifierContext"/> class.
    /// </summary>
    /// <param name="newId">Function to be applied to generate a new identifier</param>
    public IdentifierContext(Func<uint, string> newId)
    {
        _threadScopeStack.Value?.Push(this);
        NewId = newId;
        CurrentIndex = 0;
    }

    /// <summary>
    /// Gets a reference to the current <see cref="IdentifierContext"/> instance.
    /// </summary>
    public static IdentifierContext? Current
    {
        get
        {
            if (_threadScopeStack.Value == null || _threadScopeStack.Value.Count == 0)
            {
                return null;
            }

            return _threadScopeStack.Value?.Peek();
        }
    }

    /// <summary />
    internal uint CurrentIndex { get; set; }

    /// <summary />
    private Func<uint, string> NewId { get; init; }

    /// <summary>
    /// Returns the next number between 0 and 99999999.
    /// </summary>
    /// <returns></returns>
    internal string GenerateId()
    {
        var id = NewId.Invoke(CurrentIndex);

        CurrentIndex++;

        if (CurrentIndex >= 99999999)
        {
            CurrentIndex = 0;
        }

        return id;
    }

    /// <summary>
    /// Disposes the current <see cref="IdentifierContext"/> instance.
    /// </summary>
    public void Dispose()
    {
        _ = _threadScopeStack.Value?.TryPop(out _);
    }
}
