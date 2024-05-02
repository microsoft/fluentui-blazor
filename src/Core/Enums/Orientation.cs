// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The orientation of a component.
/// </summary>
public enum Orientation
{
    /// <summary>
    /// The component is oriented horizontally.
    /// </summary>
    [Description("horizontal")]
    Horizontal,

    /// <summary>
    /// The component is oriented vertically.
    /// </summary>
    [Description("vertical")]
    Vertical
}
