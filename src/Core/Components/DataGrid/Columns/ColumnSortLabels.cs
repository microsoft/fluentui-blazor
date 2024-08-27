// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
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
    /// Gets the default labels for the sort UI.
    /// </summary>
    public static ColumnSortLabels Default => new();

}

