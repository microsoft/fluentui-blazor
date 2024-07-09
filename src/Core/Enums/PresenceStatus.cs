using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the presence status.
/// </summary>
public enum PresenceStatus
{
    /// <summary />
    Busy,

    /// <summary />
    [Display(Name = "OOF")]
    OutOfOffice,

    /// <summary />
    Away,

    /// <summary />
    Available,

    /// <summary />
    Offline,

    /// <summary />
    [Display(Name = "DND")]
    DoNotDisturb,

    /// <summary />
    Unknown
}
