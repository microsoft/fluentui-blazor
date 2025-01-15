// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for configuring a dialog footer action button.
/// </summary>
public class DialogOptionsFooterAction
{
    /// <summary />
    public DialogOptionsFooterAction() : this(ButtonAppearance.Default)
    {
        
    }

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
    /// </summary>
    /// <remarks>
    /// The shortcut key is a combination of one or more keys separated by a plus sign.
    /// You must use the key names defined in the <see cref="KeyboardEventArgs.Key"/> class.
    /// You can use the following modifier keys: "Ctrl", "Alt", "Shift", in this order.
    /// </remarks>
    /// <example>
    /// "Enter", "Escape", "Ctrl+Enter", "Ctrl+Alt+Shift+Enter", "Escape;Enter".
    /// </example>
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
