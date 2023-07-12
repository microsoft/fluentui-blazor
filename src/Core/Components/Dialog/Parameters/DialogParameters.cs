using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class DialogParameters : ComponentParameters, IDialogParameters
{
    public string? Id { get; set; }
    /// <summary>
    /// Gets or sets the dialog position:
    /// left (full height), right (full height)
    /// or screen middle (using Width and Height properties).
    /// </summary>
    public virtual HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Center;

    /// <summary>
    /// Title of the dialog
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Determines if the dialog is modal. Defaults to true.
    /// When true, clicking outside the dialog will dismiss the dialog.
    /// </summary>
    public bool? Modal { get; set; } = true;

    /// <summary>
    /// Indicates that the dialog should trap focus.
    /// </summary>
    public bool? TrapFocus { get; set; } = true;

    /// <summary>
    /// When true, shows the title in the header.
    /// </summary>
    public bool ShowTitle { get; set; } = true;

    /// <summary>
    /// When true, shows the dismiss button in the header.
    /// </summary>
    public bool ShowDismiss { get; set; } = true;

    /// <summary>
    /// Text to display for the primary action.
    /// </summary>
    public string? PrimaryAction { get; set; } = "Ok"; //DialogResources.ButtonPrimary;

    //public EventCallback<DialogResult>? OnPrimaryAction { get; set; } = default!;

    /// <summary>
    /// Text to display for the secondary action.
    /// </summary>
    public string? SecondaryAction { get; set; } = "Cancel"; //DialogResources.ButtonSecondary;

    //public EventCallback<DialogResult>? OnSecondaryAction { get; set; } = default!;

    /// <summary>
    /// Width of the dialog. Must be a valid CSS width value like "600px" or "3em"
    /// </summary>
    public string? Width { get; set; }

    /// <summary>
    /// Height of the dialog. Must be a valid CSS height value like "600px" or "3em"
    /// Only used if Alignment is set to "HorizontalAlignment.Center"
    /// </summary>  
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the extra styles appied to the Body content.
    /// </summary>
    public string DialogBodyStyle { get; set; } = string.Empty;

    /// <summary>
    /// Identifies the element that labels the element it is applied to.
    /// </summary>
    public string? AriaLabelledby { get; set; }

    /// <summary>
    /// Identifies the element that describes the element on which the attribute is set.
    /// </summary>
    public string? AriaDescribedby { get; set; }

    /// <summary>
    /// The value that labels an interactive element.
    /// </summary>
    public string? AriaLabel { get; set; }

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
}

/// <summary>
/// Parameters for a dialog.
/// </summary>
public class DialogParameters<TContent> : DialogParameters, IDialogParameters<TContent>
    where TContent : class
{
    /// <summary>
    /// Content to pass to and from the dialog.
    /// </summary>
    public TContent Content { get; set; } = default!;


}
