// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// FluentUI status colors and icons.
/// </summary>
public static class FluentStatus
{
    /// <summary>
    /// Gets the color for success status.
    /// </summary>
    public static string SuccessColor { get; } = Color.Success.ToAttributeValue()!;

    /// <summary>
    /// Gets the color for warning status.
    /// </summary>
    public static string WarningColor { get; } = Color.Warning.ToAttributeValue()!;

    /// <summary>
    /// Gets the color for error status.
    /// </summary>
    public static string ErrorColor { get; } = Color.Error.ToAttributeValue()!;

    /// <summary>
    /// Gets the color for info status.
    /// </summary>
    public static string InfoColor { get; } = Color.Info.ToAttributeValue()!;

    /// <summary>
    /// Gets the icon for success status.
    /// </summary>
    public static Icon SuccessIcon { get; } = new CoreIcons.Filled.Size20.CheckmarkCircle().WithColor(SuccessColor);

    /// <summary>
    /// Gets the icon for warning status.
    /// </summary>
    public static Icon WarningIcon { get; } = new CoreIcons.Filled.Size20.Warning().WithColor(WarningColor);

    /// <summary>
    /// Gets the icon for error status.
    /// </summary>
    public static Icon ErrorIcon { get; } = new CoreIcons.Filled.Size20.DismissCircle().WithColor(ErrorColor);

    /// <summary>
    /// Gets the icon for info status.
    /// </summary>
    public static Icon InfoIcon { get; } = new CoreIcons.Filled.Size20.Info().WithColor(InfoColor);
}
