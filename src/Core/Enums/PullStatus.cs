// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes the status in the PullToRefresh component
/// </summary>
public enum PullStatus
{
    /// <summary>
    /// Not started yet
    /// </summary>
    [Description("awaiting")]
    Awaiting = 0,

    /// <summary>
    /// Pulling down has started and triggerpoint has not been reached
    /// </summary>
    [Description("pulling")]
    Pulling = 1,

    /// <summary>
    /// Reached the triggerpoint and waiting to let loose
    /// </summary>
    [Description("waiting-for-release")]
    WaitingForRelease = 2,

    /// <summary>
    /// Already let loose, refreshing
    /// </summary>
    [Description("loading")]
    Loading = 3,

    /// <summary>
    /// The refresh is complete
    /// </summary>
    [Description("completed")]
    Completed = 4,

    /// <summary>
    /// The refresh completes, but there is no more data to load
    /// </summary>
    [Description("no-data")]
    NoData = 5,
}
