// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Dialog.MessageBox;

/// <summary>
/// Represents a message box dialog.
/// </summary>
public partial class FluentMessageBox
{
    /// <summary>
    /// Gets or sets the content of the message box.
    /// </summary>
    [Parameter]
    public MarkupString? Message { get; set; }

    /// <summary>
    /// Gets or sets the icon of the message box.
    /// </summary>
    [Parameter]
    public Icon Icon { get; set; } = new CoreIcons.Filled.Size20.CheckmarkCircle();

    /// <summary>
    /// Gets or sets the icon color.
    /// </summary>
    [Parameter]
    public Color IconColor { get; set; } = Color.Success;
}
