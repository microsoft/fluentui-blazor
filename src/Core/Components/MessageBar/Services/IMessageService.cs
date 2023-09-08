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
    IEnumerable<Message> MessagesToShow(int count = 5, string? section = null);

    /// <summary />
    Message Add(Action<MessageOptions> options);

    /// <summary />
    Message Add(string message);

    /// <summary />
    Message Add(string message, MessageIntent intent);

    /// <summary />
    Message Add(string message, MessageIntent intent, string section);

    /// <summary />
    void Clear(string? section = null);

    /// <summary />
    void Remove(Message message);

    int Count(string? section);
}
