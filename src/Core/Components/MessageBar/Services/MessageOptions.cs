namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class MessageOptions

{
    /// <summary />
    internal MessageOptions(MessageBarGlobalOptions commonOptions)
    {
        Global = commonOptions;
        ShowDismiss = commonOptions.ShowDismiss;
    }

    /// <summary />
    internal MessageBarGlobalOptions Global { get; }

    /// <summary />
    public string? Category { get; set; }

    /// <summary />
    public DateTime? Timestamp { get; set; }

    /// <summary />
    public Icon? Icon { get; set; }

    /// <summary />
    public string Title { get; set; } = string.Empty;

    /// <summary />
    public string? Body { get; set; }

    /// <summary />
    public bool ShowDismiss { get; set; }

    /// <summary />
    public Func<Message, Task>? OnClose { get; set; }

    /// <summary />
    public MessageAction Action { get; set; } = new MessageAction();

    /// <summary />
    public MessageType? Type { get; set; } = MessageType.Info;

    /// <summary />
    public bool ClearAfterNavigation { get; set; }
}
