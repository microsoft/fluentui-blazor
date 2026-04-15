// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents settings for a column header menu action.
/// </summary>
/// <remarks>
/// This record provides common icon configuration for column menu items.
/// </remarks>
public record ColumnMenuSettings(Icon Icon, string Text)
{
    /// <summary>
    /// Gets or sets whether the icon is positioned at the start (true) or at the end (false) of the menu item.
    /// </summary>
    public bool IconPositionStart { get; set; } = true;
}
