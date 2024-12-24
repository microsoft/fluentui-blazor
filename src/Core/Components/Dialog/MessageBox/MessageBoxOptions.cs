// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for the message box dialog.
/// </summary>
public class MessageBoxOptions
{
    /// <summary>
    /// Gets or sets the message to display in the dialog.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the title to display in the dialog header.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the text to display in the primary action button.
    /// </summary>
    public string? PrimaryButton { get; set; }

    /// <summary>
    /// Gets or sets the text to display in the secondary action button.
    /// </summary>
    public string? SecondaryButton { get; set; }

    /// <summary>
    /// Gets or sets the icon to display in the dialog.
    /// </summary>
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the color of the icon.
    /// </summary>
    public Color IconColor { get; set; } = Color.Default;
}
