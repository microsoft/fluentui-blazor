using System.ComponentModel;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public enum MessageType
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
}
