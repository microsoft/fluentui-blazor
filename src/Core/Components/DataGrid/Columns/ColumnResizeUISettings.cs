// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the UI settings for column resizing.
/// </summary>
/// <remarks>This record provides customizable settings for the column resize UI, such as icon configuration. Use this type to  configure the
/// appearance and behavior of the column resize functionality in your application.</remarks>
public record ColumnResizeUISettings
{
    /// <summary>
    /// Gets or sets the icon to show in the column menu
    /// </summary>
    public Icon? Icon { get; set; } = new CoreIcons.Regular.Size20.TableResizeColumn();

    /// <summary>
    /// Gets or sets whether the icon is positioned at the start (true) or
    /// at the end (false) of the menu item
    /// </summary>
    public bool IconPositionStart { get; set; } = true;

    /// <summary>
    /// Gets the default labels for the resize UI.
    /// </summary>
    public static ColumnResizeUISettings Default => new();
}
