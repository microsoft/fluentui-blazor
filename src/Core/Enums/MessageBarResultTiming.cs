// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Controls when <see cref="IMessageBarInstance.Result"/> is completed.
/// </summary>
public enum MessageBarResultTiming
{
    /// <summary>
    /// Complete the result when the message bar is dismissed or closed.
    /// </summary>
    Closed,

    /// <summary>
    /// Complete the result when the message bar is registered as visible.
    /// In this mode, <see cref="MessageBarResult.Reason"/> will be <see cref="MessageBarCloseReason.Programmatic"/>.
    /// </summary>
    Visible,
}
