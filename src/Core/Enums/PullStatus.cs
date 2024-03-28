// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

public enum PullStatus
{
    /// <summary>
    /// Not started yet
    /// </summary>
    Awaiting = 0,

    /// <summary>
    /// Pulling down has started and triggerpoint has not been reached
    /// </summary>
    Pulling = 1,

    /// <summary>
    /// Reached the triggerpoint and waiting to let loose
    /// </summary>
    WaitingForRelease = 2,

    /// <summary>
    /// Already let loose, refreshing
    /// </summary>
    Loading = 3,

    /// <summary>
    /// The refresh is complete
    /// </summary>
    Completed = 4,

    /// <summary>
    /// The refresh completes, but there is no more data to load
    /// </summary>
    NoData = 5,
}
