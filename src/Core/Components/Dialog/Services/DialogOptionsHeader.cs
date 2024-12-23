// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for configuring a dialog header.
/// </summary>
public class DialogOptionsHeader
{
    /// <summary />
    internal DialogOptionsHeader()
    {
        
    }

    /// <summary>
    /// Gets or sets the title of the dialog.
    /// </summary>
    public string? Title { get; set; }

    /*
    /// <summary>
    /// Gets or sets whether the close button is visible.
    /// </summary>
    public bool DismissVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets the icon to use for the close button.
    /// </summary>
    public Icon DismissIcon { get; set; } = new CoreIcons.Regular.Size20.Dismiss();
    */
}
