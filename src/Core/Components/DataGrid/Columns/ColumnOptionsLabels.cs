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
    /// Gets the default labels for the options UI.
    /// </summary>
    public static ColumnOptionsLabels Default => new();

}

