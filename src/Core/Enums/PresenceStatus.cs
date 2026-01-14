// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the presence status of a user.
/// </summary>
public enum PresenceStatus
{
    /// <summary>
    /// Available
    /// </summary>
    Available,

    /// <summary>
    /// Busy
    /// </summary>
    Busy,

    /// <summary>
    /// Away
    /// </summary>
    Away,

    /// <summary>
    /// Out of office
    /// </summary>
    OutOfOffice,

    /// <summary>
    /// Offline
    /// </summary>
    Offline,

    /// <summary>
    /// Do not disturb
    /// </summary>
    DoNotDisturb,

    /// <summary>
    /// Unknown
    /// </summary>
    Unknown,

    /// <summary>
    /// Blocked
    /// </summary>
    Blocked,
}
