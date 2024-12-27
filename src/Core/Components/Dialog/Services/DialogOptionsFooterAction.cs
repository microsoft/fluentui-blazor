// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for configuring a dialog footer action button.
/// </summary>
public class DialogOptionsFooterAction
{
    /// <summary />
    internal DialogOptionsFooterAction(ButtonAppearance appearance)
    {
        Appearance = appearance;
        ShortCut = appearance == ButtonAppearance.Primary ? "Enter" : "Escape";
    }

    /// <summary />
    internal ButtonAppearance Appearance { get; }

    /// <summary>
    /// Gets or sets the label of the action button.
    /// By default, this label is not set. So the button will not be displayed.
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the shortcut key for the action button.
    /// By default, "Enter" for the primary action and "Escape" for the secondary action.
    /// Example: "Enter", "Escape", "Ctrl+Enter", "Escape;Enter"
    /// </summary>
    public string? ShortCut { get; set; }

    /// <summary>
    /// Gets or sets whether the action button is visible.
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the action button is disabled.
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the action to be performed when the action button is clicked.
    /// </summary>
    public Func<IDialogInstance, Task>? OnClickAsync { get; set; }

    /// <summary />
    internal bool ToDisplay => !string.IsNullOrEmpty(Label) && Visible;
}
