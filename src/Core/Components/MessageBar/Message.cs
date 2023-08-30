namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class Message
{
    public Message()
    {
        Options = new MessageOptions(new MessageBarGlobalOptions())
        {
            Type = MessageType.Info,
        };

    }
    /// <summary />
    internal Message(MessageOptions options)
    {
        Options = options;
    }

    internal event Action<Message>? OnClose;

    /// <summary />
    internal MessageOptions Options { get; }

    /// <summary />
    public string? Category => Options.Category;

    /// <summary />
    public string Title
    {
        get
        {
            return Options.Title;
        }

        set
        {
            Options.Title = value;
        }
    }

    /// <summary />
    public string? Body
    {
        get
        {
            return Options.Body;
        }

        set
        {
            Options.Body = value;
        }
    }

    /// <summary />
    public MessageType? Type => Options.Type;

    /// <summary />
    public void Close()
    {
        OnClose?.Invoke(this);
    }

    /// <summary />
    internal static Message Empty()
    {
        return new Message(new MessageOptions(new MessageBarGlobalOptions())
        {
            Type = MessageType.Info,
        });
    }
}
