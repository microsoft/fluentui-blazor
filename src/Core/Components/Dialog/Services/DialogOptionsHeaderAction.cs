// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for configuring a dialog footer action button.
/// </summary>
public class DialogOptionsHeaderAction
{
    /// <summary />
    public DialogOptionsHeaderAction()
    {
    }

    /// <summary />
    internal DialogOptionsHeaderAction(bool isClosedAction)
    {
        IsClosedAction = isClosedAction;

        if (isClosedAction)
        {
            Icon = new CoreIcons.Regular.Size20.Dismiss();
        }
        else
        {
            Icon = new CoreIcons.Regular.Size20.Info();
        }
    }

    /// <summary />
    internal bool IsClosedAction { get; init; }

    /// <summary>
    /// Gets or sets the icon of the action button.
    /// </summary>
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the label of the action button.
    /// By default, this label is not set. So, only the icon will be displayed.
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the tooltip text of the action button (supports HTML tags).
    /// This parameter requires the presence of a 'FluentTooltipProvider' in your main/layout page.
    /// </summary>
    /// <remarks>
    /// This parameter cannot be updated after the component has been rendered.
    /// </remarks>
    public string? Tooltip { get; set; }

    /// <summary>
    /// Gets or sets whether the action button is visible.
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// Gets or sets the action to be performed when the action button is clicked.
    /// </summary>
    public Func<IDialogInstance, Task>? OnClickAsync { get; set; }

    /// <summary />
    internal bool ToDisplay => (!string.IsNullOrEmpty(Label) || Icon is not null) && Visible;
}
