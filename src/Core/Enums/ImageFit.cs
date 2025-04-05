// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Determines how the image will be scaled and positioned within its parent container.
/// </summary>
public enum ImageFit
{
    /// <summary>
    /// The image will not be resized or repositioned.
    /// </summary>
    None,

    /// <summary>
    /// The image will be centered in its container without changing its original size.
    /// </summary>
    Center,

    /// <summary>
    /// The image will be scaled to fit within its container while maintaining its aspect ratio.
    /// </summary>
    Contain,

    /// <summary>
    /// The image will be scaled to cover the entire container while maintaining its aspect ratio.
    /// </summary>
    Cover
}
