namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class MessageOptions

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

    public MessageAction Link { get; set; } = new MessageAction();

    /// <summary />
    public Func<Message, Task>? OnClose { get; set; }

    /// <summary />
    public MessageAction PrimaryAction { get; set; } = new MessageAction();

    /// <summary />
    public MessageAction SecondaryAction { get; set; } = new MessageAction();

    /// <summary />
    public MessageIntent? Intent { get; set; } 

    /// <summary />
    public bool ClearAfterNavigation { get; set; }
}
