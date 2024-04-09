using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class DialogParameters : ComponentParameters, IDialogParameters
{
    public string Id { get; set; } = Identifier.NewId();

    /// <summary>
    /// Gets or sets the dialog position:
    /// left (full height), right (full height)
    /// or screen middle (using Width and Height properties).
    /// </summary>
    public virtual HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Center;

    /// <summary>
    /// Gets or sets the title of the dialog.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Determines if the dialog is modal. Defaults to true.
    /// Obscures the area around the dialog.
    /// </summary>
    public bool? Modal { get; set; } = true;

    /// <summary>
    /// Determines if a modal dialog is dismissible by clicking
    /// outside the dialog. Defaults to false.
    /// </summary>
    public bool PreventDismissOnOverlayClick { get; set; } = false;

    /// <summary>
    /// Prevents scrolling outside of the dialog while it is shown.
    /// </summary>to use
    public bool PreventScroll { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether if dialog should trap focus.
    /// Defaults to true.
    /// </summary>
    public bool? TrapFocus { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether show the title in the header.
    /// Defaults to true.
    /// </summary>
    public bool ShowTitle { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether show the dismiss button in the header.
    /// Defaults to true.
    /// </summary>
    public bool ShowDismiss { get; set; } = true;

    /// <summary>
    /// Gets or sets the Title of the dismiss button, display in a tooltip.
    /// Defaults to "Close".
    /// </summary>
    public string? DismissTitle { get; set; } = "Close";

    /// <summary>
    /// Gets or sets the text to display for the primary action.
    /// </summary>
    public string? PrimaryAction { get; set; } = "OK"; //DialogResources.ButtonPrimary;

    /// <summary>
    /// When true, primary action's button is enabled.
    /// </summary>
    public bool PrimaryActionEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the text to display for the secondary action.
    /// </summary>
    public string? SecondaryAction { get; set; } = "Cancel"; //DialogResources.ButtonSecondary;

    /// <summary>
    /// When true, secondary action's button is enabled.
    /// </summary>
    public bool SecondaryActionEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the width of the dialog. Must be a valid CSS width value like "600px" or "3em"
    /// </summary>
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the dialog. Must be a valid CSS height value like "600px" or "3em"
    /// Only used if Alignment is set to "HorizontalAlignment.Center"
    /// </summary>  
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the extra styles applied to the Body content.
    /// </summary>
    public string DialogBodyStyle { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the element that labels the element it is applied to.
    /// </summary>
    public string? AriaLabelledby { get; set; }

    /// <summary>
    /// Gets or sets the element that describes the element on which the attribute is set.
    /// </summary>
    public string? AriaDescribedby { get; set; }

    /// <summary>
    /// Gets or sets the value that labels an interactive element.
    /// </summary>
    public string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets the type of dialog.
    /// </summary>
    public DialogType DialogType { get; set; } = DialogType.Dialog;

    /// <summary>
    /// Gets whether the primary button is displayed or not. Depends on PrimaryAction having a value.
    /// </summary>
    internal bool ShowPrimaryAction => !string.IsNullOrEmpty(PrimaryAction);

    /// <summary>
    /// Gets whether the secondary button is displayed or not. Depends on SecondaryAction having a value. 
    /// </summary>
    internal bool ShowSecondaryAction => !string.IsNullOrEmpty(SecondaryAction);

    /// <summary>
    /// Callback function for the result.
    /// </summary>
    public EventCallback<DialogResult> OnDialogResult { get; set; } = default!;
    /// <summary>
    /// Callback function that is called and awaited before the dialog fully closes.
    /// </summary>
    /// <remarks>
    /// This is a suitable callback to use for animating the Dialog <em>before</em> it fully closes and is removed from the DOM.
    /// This method is only called when using the <see cref="IDialogService"/>.
    /// </remarks>
    public EventCallback<DialogInstance> OnDialogClosing { get; set; } = default!;
    /// <summary>
    /// Callback function that is called and awaited after the dialog renders for the first time.
    /// </summary>
    /// <remarks>
    /// This is a suitable callback to use for animating the Dialog <em>after</em> it is fully rendered in the DOM.
    /// This method is only called when using the <see cref="IDialogService"/>.
    /// </remarks>
    public EventCallback<DialogInstance> OnDialogOpened { get; set; } = default!;
}

/// <summary>
/// Parameters for a dialog.
/// </summary>
public class DialogParameters<TContent> : DialogParameters, IDialogParameters<TContent>
    where TContent : class
{
    /// <summary>
    /// Gets or sets the content to pass to and from the dialog.
    /// </summary>
    public TContent Content { get; set; } = default!;
}
