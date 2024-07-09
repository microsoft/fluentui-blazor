// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The orientation of a component.
/// </summary>
public enum Orientation
{
    /// <summary>
    /// The component is oriented horizontally.
    /// </summary>
    [Display(Name = "horizontal")]
    Horizontal,

    /// <summary>
    /// The component is oriented vertically.
    /// </summary>
    [Display(Name = "vertical")]
    Vertical
}
