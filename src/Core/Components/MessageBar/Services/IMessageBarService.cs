namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public interface IMessageBarService : IDisposable
{
    /// <summary />
    event Action? OnMessageItemsUpdated;

    //// <summary />
    //MessageBarGlobalOptions Configuration { get; }

    /// <summary />
    IEnumerable<MessageBarContent> AllMessages { get; }

    /// <summary />
    IEnumerable<MessageBarContent> MessagesShown(int count = 5, string? category = null);

    /// <summary />
    MessageBarContent Add(Action<MessageBarOptions> options);

    /// <summary />
    MessageBarContent Add(string message);

    /// <summary />
    MessageBarContent Add(string message, MessageBarIntent severity);

    /// <summary />
    MessageBarContent Add(string message, string category, MessageBarIntent severity);

    /// <summary />
    void Clear(string? category = null);

    /// <summary />
    void Remove(MessageBarContent message);

    int Count(string? category);
}
