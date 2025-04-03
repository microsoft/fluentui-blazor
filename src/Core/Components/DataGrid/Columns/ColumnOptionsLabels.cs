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
    /// Gets or sets the position the icon can be showed in.
    /// Possible options are 'start' (default) or 'end'
    /// </summary>
    public string Slot { get; set; } = "start";

    /// <summary>
    /// Gets the default labels for the options UI.
    /// </summary>
    public static ColumnOptionsLabels Default => new();

}

