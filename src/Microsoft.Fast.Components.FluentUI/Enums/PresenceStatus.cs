using System.ComponentModel;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Represents the presence status.
/// </summary>
public enum PresenceStatus
{
    /// <summary />
    Busy,

    /// <summary />
    [Description("OOF")]
    OutOfOffice,

    /// <summary />
    Away,

    /// <summary />
    Available,

    /// <summary />
    Offline,

    /// <summary />
    [Description("DND")]
    DoNotDisturb,

    /// <summary />
    Unknown
}