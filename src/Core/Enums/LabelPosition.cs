// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the position of the label relative to the input.
/// </summary>
public enum LabelPosition
{
    /// <summary>
    /// The label is positioned above the input.
    /// </summary>
    [Description("above")]
    Above,

    /// <summary>
    /// The label is positioned after the input.
    /// </summary>
    [Description("after")]
    After,

    /// <summary>
    /// The label is positioned before the input.
    /// </summary>
    [Description("before")]
    Before,
}
