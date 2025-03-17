// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// ZIndex values for FluentUI components
/// </summary>
public static class ZIndex
{
    /// <summary>
    /// ZIndex for the <see cref="FluentDialog" /> component.
    /// </summary>
    public static int Dialog { get; set; } = 999;

    /// <summary>
    /// ZIndex for the <see cref="FluentBadge" /> and <see cref="FluentCounterBadge"/> components.
    /// </summary>
    public static int Badge { get; set; } = 999;

    /// <summary>
    /// ZIndex for the <see cref="FluentTooltip" /> component.
    /// </summary>
    public static int Tooltip { get; set; } = 9999;

}
