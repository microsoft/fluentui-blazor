namespace Microsoft.FluentUI.AspNetCore.Components;

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

    /// <summary>
    /// Gets or sets the intent of the message bar. 
    /// Default is MessageIntent.Info. 
    /// See <see cref="MessageIntent"/> for more details.
    /// </summary>
    public MessageIntent? Intent => Options.Intent;

    /// <summary>
    /// Indication of in which message bar the message needs to be shown. Default is null.
    /// </summary>
    public string? Section => Options.Section;

    /// <summary>
    /// Gets or sets the title.
    /// Most important info to be shown in the message bar.
    /// </summary>
    public string? Title
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

    /// <summary>
    /// Gets or sets the message to be shown in the message bar.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the link to be shown in the message bar (after the body).
    /// </summary>
    public ActionLink<Message>? Link
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

    public int? Timeout
    {
        get
        {
            return Options.Timeout;
        }

        set
        {
            Options.Timeout = value;
        }
    }

    /// <summary>
    /// Close the message bar.
    /// </summary>
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
