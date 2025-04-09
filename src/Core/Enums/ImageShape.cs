// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The image shape.
/// </summary>
public enum ImageShape
{
    /// <summary>
    /// The image will be displayed as a circle.
    /// </summary>
    [Description("circular")]
    Circular,

    /// <summary>
    /// The image will be displayed as a rounded rectangle.
    /// </summary>
    [Description("rounded")]
    Rounded,

    /// <summary>
    /// The image will be displayed as a square.
    /// </summary>
    [Description("square")]
    Square,
}
