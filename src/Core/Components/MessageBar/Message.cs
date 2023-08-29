namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class Message
{
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
    public string Text
    {
        get
        {
            return Options.Text;
        }

        set
        {
            Options.Text = value;
        }
    }

    /// <summary />
    public string? DetailledText
    {
        get
        {
            return Options.DetailedText;
        }

        set
        {
            Options.DetailedText = value;
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
