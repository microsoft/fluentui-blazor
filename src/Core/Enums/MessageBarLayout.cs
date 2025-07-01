// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public enum MessageBarLayout
{
    /// <summary>
    /// The message bar actions are displayed next to the message content, allowing for a compact layout.
    /// </summary>
    [Description("singleline")]
    SingleLine,

    /// <summary>
    /// The message bar actions are displayed on a new line, allowing for more space for the message content.
    /// </summary>
    [Description("multiline")]
    MultiLine,
}
