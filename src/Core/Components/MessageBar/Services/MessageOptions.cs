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

    public ActionLink<Message>? Link { get; set; } = new ();

    /// <summary />
    public Func<Message, Task>? OnClose { get; set; }

    /// <summary />
    public ActionButton<Message>? PrimaryAction { get; set; } = new();

    /// <summary />
    public ActionButton<Message>? SecondaryAction { get; set; } = new();

    /// <summary />
    public MessageIntent? Intent { get; set; } 

    /// <summary />
    public bool ClearAfterNavigation { get; set; }
}
