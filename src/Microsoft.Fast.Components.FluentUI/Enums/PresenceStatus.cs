using System.ComponentModel;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Represents the presence status.
/// </summary>
public enum PresenceState
{
    /// <summary />
    Busy,

    /// <summary />
    [Description("out-of-office")]
    OutOfOffice,

    /// <summary />
    Away,

    /// <summary />
    Available,

    /// <summary />
    Offline,

    /// <summary />
    [Description("do-not-disturb")]
    DoNotDisturb,

    /// <summary />
    Unknown
}