namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class MessageAction
{
    /// <summary />
    public string? Text { get; set; }

    /// <summary />
    public string? Href { get; set; }

    /// <summary />
    public Func<Message, Task>? OnClick { get; set; }
}
