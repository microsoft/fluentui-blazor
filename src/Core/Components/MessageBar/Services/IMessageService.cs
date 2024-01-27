namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public interface IMessageService : IDisposable
{
    /// <summary />
    public event Action OnMessageItemsUpdated;
    public event Func<Task> OnMessageItemsUpdatedAsync;
    /// <summary />
    IEnumerable<Message> AllMessages { get; }

    /// <summary />
    IEnumerable<Message> MessagesToShow(int count = 5, string? section = null);

    /// <summary />
    Message ShowMessageBar(Action<MessageOptions> options);

    /// <summary />
    Message ShowMessageBar(string message);

    /// <summary />
    Message ShowMessageBar(string message, MessageIntent intent);

    /// <summary />
    Message ShowMessageBar(string message, MessageIntent intent, string section);

    /// <summary />
    Task<Message> ShowMessageBarAsync(Action<MessageOptions> options);

    /// <summary />
    Task<Message> ShowMessageBarAsync(string message);

    /// <summary />
    Task<Message> ShowMessageBarAsync(string message, MessageIntent intent);

    /// <summary />
    Task<Message> ShowMessageBarAsync(string message, MessageIntent intent, string section);

    /// <summary />
    void Clear(string? section = null);

    /// <summary />
    void Remove(Message message);

    int Count(string? section);
}
