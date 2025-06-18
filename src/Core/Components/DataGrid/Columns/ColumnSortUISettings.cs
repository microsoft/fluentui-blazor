// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the UI settings for column sorting.
/// </summary>
/// <remarks>This type provides customizable options for the icon displayed in the column sorting menu</remarks>
public record ColumnSortUISettings
{
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
    /// Gets the default settings for the sort UI.
    /// </summary>
    public static ColumnSortUISettings Default => new();

}

