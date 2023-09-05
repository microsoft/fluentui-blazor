namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class MessageBarOptions

{
    /// <summary />
    public string? Section { get; set; }

    /// <summary />
    public DateTime? Timestamp { get; set; }

    /// <summary />
    public Icon? Icon { get; set; }

    /// <summary />
    public string Title { get; set; } = string.Empty;

    /// <summary />
    public string? Body { get; set; }

    /// <summary />
    public Func<MessageBarContent, Task>? OnClose { get; set; }

    /// <summary />
    public MessageBarAction PrimaryAction { get; set; } = new MessageBarAction();

    /// <summary />
    public MessageBarAction SecondaryAction { get; set; } = new MessageBarAction();

    /// <summary />
    public MessageBarIntent? Intent { get; set; } // = MessageBarIntent.Info;

    /// <summary />
    public bool ClearAfterNavigation { get; set; }
}
