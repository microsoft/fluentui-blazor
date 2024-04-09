using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

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
