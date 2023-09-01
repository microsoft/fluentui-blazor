using System.ComponentModel;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public enum MessageBarIntent
{
    /// <summary />
    [Description("info")]
    Info,

    /// <summary />
    [Description("warning")]
    Warning,

    /// <summary />
    [Description("error")]
    Error,

    /// <summary />
    [Description("success")]
    Success,

    /// <summary />
    [Description("custom")]
    Custom,
}
