// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Sets the visual size of the <see cref="FluentTreeView"/> or the <see cref="FluentTreeItem"/>
/// </summary>
public enum TreeSize
{
    /// <summary>
    /// Medium size
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// Small size
    /// </summary>
    [Description("small")]
    Small,
}
