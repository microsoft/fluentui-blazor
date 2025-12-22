// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Determines how the image will be scaled and positioned within its parent container.
/// </summary>
public enum ImageFit
{
    /// <summary>
    /// The image will not be resized or repositioned.
    /// </summary>
    [Description("none")]
    None,

    /// <summary>
    /// The image will be centered in its container without changing its original size.
    /// </summary>
    [Description("center")]
    Center,

    /// <summary>
    /// The image will be scaled to fit within its container while maintaining its aspect ratio.
    /// </summary>
    [Description("contain")]
    Contain,

    /// <summary>
    /// The image will be scaled to cover the entire container while maintaining its aspect ratio.
    /// </summary>
    [Description("cover")]
    Cover,
}
