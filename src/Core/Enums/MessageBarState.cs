// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the state of a message bar.
/// </summary>
public enum MessageBarState
{
    /// <summary>
    /// The message bar is dismissed.
    /// </summary>
    Dismissed,

    /// <summary>
    /// The message bar is showing.
    /// </summary>
    Opening,

    /// <summary>
    /// The message bar is shown.
    /// </summary>
    Open,

    /// <summary>
    /// The message bar is dismissing.
    /// </summary>
    Dismissing,
}
