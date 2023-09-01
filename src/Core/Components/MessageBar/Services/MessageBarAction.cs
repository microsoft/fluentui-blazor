namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class MessageBarAction
{
    /// <summary />
    public string? Text { get; set; }

    /// <summary />
    public string? Href { get; set; }

    /// <summary />
    public Func<MessageBarContent, Task>? OnClick { get; set; }
}
