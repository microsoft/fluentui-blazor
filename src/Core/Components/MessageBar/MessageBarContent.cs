namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class MessageBarContent
{
    public MessageBarContent()
    {
        Options = new MessageBarOptions();
    }

    /// <summary />
    internal MessageBarContent(MessageBarOptions options)
    {
        Options = options;
    }

    internal event Action<MessageBarContent>? OnClose;

    /// <summary />
    internal MessageBarOptions Options { get; }

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
    public MessageBarIntent? Intent => Options.Intent;

    /// <summary />
    public void Close()
    {
        OnClose?.Invoke(this);
    }

    /// <summary />
    internal static MessageBarContent Empty()
    {
        return new MessageBarContent(new MessageBarOptions()
        {
            Intent = MessageBarIntent.Info,
        });
    }
}
