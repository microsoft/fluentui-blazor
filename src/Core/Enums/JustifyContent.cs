using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the alignment of items along the main axis of a flex container.
/// </summary>
public enum JustifyContent
{
    /// <summary>
    /// Justify content flex-start.
    /// </summary>
    [Description("flex-start")]
    FlexStart,

    /// <summary>
    /// Justify content center.
    /// </summary>
    [Description("center")]
    Center,

    /// <summary>
    /// Justify content flex-end.
    /// </summary>
    [Description("flex-end")]
    FlexEnd,

    /// <summary>
    /// Justify content space-between.
    /// </summary>
    [Description("space-between")]
    SpaceBetween,

    /// <summary>
    /// Justify content space-around.
    /// </summary>
    [Description("space-around")]
    SpaceAround,

    /// <summary>
    /// Justify content space-evenly.
    /// </summary>
    [Description("space-evenly")]
    SpaceEvenly
}
