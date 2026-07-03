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
    /// Complete the result when the message bar becomes visible.
    /// </summary>
    Visible,
}
