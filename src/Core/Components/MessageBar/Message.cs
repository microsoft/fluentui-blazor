namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class Message
{
    public Message()
    {
        Options = new MessageOptions();
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
    public MessageIntent? Intent => Options.Intent;

    /// <summary />
    public string? Section => Options.Section;

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
    public MessageAction Link
    {
        get
        {
            return Options.Link;
        }

        set
        {
            Options.Link = value;
        }
    }

    

    /// <summary />
    public void Close()
    {
        OnClose?.Invoke(this);
    }

    /// <summary />
    internal static Message Empty()
    {
        return new Message(new MessageOptions()
        {
            Intent = MessageIntent.Info,
        });
    }
}
