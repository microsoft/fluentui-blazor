// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The size of Tabs.
/// </summary>
public enum TabsSize
{
    /// <summary>
    /// Medium tabs. This is the default size.
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// Small tabs.
    /// </summary>
    [Description("small")]
    Small,

    /// <summary>
    /// Large tabs.
    /// </summary>
    [Description("large")]
    Large,
}
