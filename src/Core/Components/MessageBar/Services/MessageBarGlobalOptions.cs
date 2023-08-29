namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class MessageBarGlobalOptions
{
    /// <summary />
    public bool NewestOnTop { get; set; } = true;

    /// <summary />
    public int MaxMessageCount { get; set; } = 5;

    /// <summary />
    public bool ShowDismiss { get; set; } = true;

    /// <summary />
    public bool ClearAfterNavigation { get; set; } = false;
}
