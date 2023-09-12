using System.ComponentModel;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public enum MessageIntent
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
