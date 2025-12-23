// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

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
    public MarkupStringSanitized? Message { get; set; }

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
