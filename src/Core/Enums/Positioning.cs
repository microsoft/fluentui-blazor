// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Determines the placement of aligned content relative to the base content.
/// This is used for example in the <see cref="FluentCounterBadge"/> component.
/// </summary>
public enum Positioning
{
    /// <summary>
    /// The content is aligned above and at the end.
    /// </summary>
    [Description("above-end")]
    AboveEnd,

    /// <summary>
    /// The content is aligned above and at the start.
    /// </summary>
    [Description("above-start")]
    AboveStart,

    /// <summary>
    /// The content is aligned above and at the center.
    /// </summary>
    [Description("above")]
    Above,

    /// <summary>
    /// The content is aligned below and at the start.
    /// </summary>
    [Description("below-start")]
    BelowStart,

    /// <summary>
    /// The content is aligned below and at the center.
    /// </summary>
    [Description("below")]
    Below,

    /// <summary>
    /// The content is aligned below and at the end.
    /// </summary>
    [Description("below-end")]
    BelowEnd,

    /// <summary>
    /// The content is aligned before and at the top.
    /// </summary>
    [Description("before-top")]
    BeforeTop,

    /// <summary>
    /// The content is aligned before and at the center.
    /// </summary>
    [Description("before")]
    Before,

    /// <summary>
    /// The content is aligned before and at the bottom.
    /// </summary>
    [Description("before-bottom")]
    BeforeBottom,

    /// <summary>
    /// The content is aligned after and at the top.
    /// </summary>
    [Description("after-top")]
    AfterTop,

    /// <summary>
    /// The content is aligned after and at the center.
    /// </summary>
    [Description("after")]
    After,

    /// <summary>
    /// The content is aligned after and at the bottom.
    /// </summary>
    [Description("after-bottom")]
    AfterBottom,

    /// <summary>
    ///  The content is aligned in the center.
    /// </summary>
    [Description("center-center")]
    CenterCenter,
}
