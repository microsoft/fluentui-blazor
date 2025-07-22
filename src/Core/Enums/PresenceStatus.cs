// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

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
