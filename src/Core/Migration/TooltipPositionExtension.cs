// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Migration;

/// <summary />
public static class TooltipPositionExtension
{
    /// <summary>
    /// Converts an obsolete <see cref="TooltipPosition"/> enum value to a <see cref="Positioning"/>.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static Positioning? ToPositioning(this TooltipPosition position)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        return position switch
        {
            TooltipPosition.Top => Positioning.Above,
            TooltipPosition.Bottom => Positioning.Below,
            TooltipPosition.Left => Positioning.Before,
            TooltipPosition.Right => Positioning.After,
            TooltipPosition.Start => Positioning.AboveStart,
            TooltipPosition.End => Positioning.AboveEnd,
            _ => Positioning.Above
        };
#pragma warning restore CS0618 // Type or member is obsolete
    }
}
