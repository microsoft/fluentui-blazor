// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Indicates the size of the <see cref="FluentTextInput"/>.
/// </summary>
public enum TextInputSize
{
    /// <summary>
    /// Small size.
    /// </summary>
    [Description("small")]
    Small,

    /// <summary>
    /// Medium size.
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// Large size.
    /// </summary>
    [Description("large")]
    Large,
}
