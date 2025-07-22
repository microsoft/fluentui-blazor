// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components;

public record ColumnSortLabels
{
    /// <summary>
    /// Gets or sets the text shown in the column menu
    /// </summary>
    public string SortMenu { get; set; } = "Sort";

    /// <summary>
    /// Gets or sets the text shown in the column menu when in ascending order
    /// </summary>
    public string SortMenuAscendingLabel { get; set; } = "Sort (ascending)";

    /// <summary>
    /// Gets or sets the text shown in the column menu when in descending order
    /// </summary>
    public string SortMenuDescendingLabel { get; set; } = "Sort (descending)";

    /// <summary>
    /// Gets or sets the icon to show in the column menu
    /// </summary>
    public Icon? Icon { get; set; } = new CoreIcons.Regular.Size16.ArrowSort();

    /// <summary>
    /// Gets or sets whether the icon is positioned at the start (true) or
    /// at the end (false) of the menu item
    /// </summary>
    public bool IconPositionStart { get; set; } = true;

    /// <summary>
    /// Gets the default labels for the sort UI.
    /// </summary>
    public static ColumnSortLabels Default => new();

}

