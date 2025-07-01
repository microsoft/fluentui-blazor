// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the UI settings for column options (usually filtering).
/// </summary>
/// <remarks>This type provides customizable settings for the column options menu icon,
/// and icon positioning. It also includes a default configuration that can be used as a baseline.</remarks>
public record ColumnOptionsUISettings()
{
    /// <summary>
    /// Gets or sets the icon to show in the column menu
    /// </summary>
    public Icon? Icon { get; set; } = new CoreIcons.Regular.Size20.Filter();

    /// <summary>
    /// Gets or sets whether the icon is positioned at the start (true) or
    /// at the end (false) of the menu item
    /// </summary>
    public bool IconPositionStart { get; set; } = true;

    /// <summary>
    /// Gets the default labels for the options UI.
    /// </summary>
    public static ColumnOptionsUISettings Default => new();

}

