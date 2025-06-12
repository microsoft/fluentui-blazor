// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the UI settings for column sorting, including labels, aria attributes, and menu options in the <see cref="FluentDataGrid{TGridItem}"/>.
/// </summary>
/// <remarks>This type provides customizable options for the text and icons displayed in the column sorting menu,
/// including labels for ascending and descending sort orders, and the position of the sort icon.</remarks>
public record ColumnSortUISettings
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
    public Icon? Icon { get; set; } = new CoreIcons.Regular.Size20.ArrowSort();

    /// <summary>
    /// Gets or sets whether the icon is positioned at the start (true) or
    /// at the end (false) of the menu item
    /// </summary>
    public bool IconPositionStart { get; set; } = true;

    /// <summary>
    /// Gets the default labels for the sort UI.
    /// </summary>
    public static ColumnSortUISettings Default => new();

}

