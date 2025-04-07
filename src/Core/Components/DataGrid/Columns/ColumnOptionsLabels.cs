// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------
namespace Microsoft.FluentUI.AspNetCore.Components;

public record ColumnOptionsLabels
{
    /// <summary>
    /// Gets or sets the text shown in the column menu
    /// </summary>
    public string OptionsMenu { get; set; } = "Filter";

    /// <summary>
    /// Gets or sets the icon to show in the column menu
    /// </summary>
    public Icon? Icon { get; set; } = new CoreIcons.Regular.Size16.Filter();

    /// <summary>
    /// Gets or sets whether the icon is positioned at the start (true) or
    /// at the end (false) of the menu item
    /// </summary>
    public bool IconPositionStart { get; set; } = true;

    /// <summary>
    /// Gets the default labels for the options UI.
    /// </summary>
    public static ColumnOptionsLabels Default => new();

}

