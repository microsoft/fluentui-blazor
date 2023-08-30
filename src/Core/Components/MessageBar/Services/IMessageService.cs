namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public interface IMessageService : IDisposable
{
    /// <summary />
    event Action? OnMessageItemsUpdated;

    //// <summary />
    //MessageBarGlobalOptions Configuration { get; }

    /// <summary />
    IEnumerable<Message> AllMessages { get; }

    /// <summary />
    IEnumerable<Message> MessagesShown(int count = 5, string? category = null);

    /// <summary />
    Message Add(Action<MessageOptions> options);

    /// <summary />
    Message Add(string message);

    /// <summary />
    Message Add(string message, MessageType severity);

    /// <summary />
    Message Add(string message, string category, MessageType severity);

    /// <summary />
    void Clear(string? category = null);

    /// <summary />
    void Remove(Message message);

    int Count(string? category);
}
