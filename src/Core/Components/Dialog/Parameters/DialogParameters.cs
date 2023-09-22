﻿using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

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
    /// Title of the dialog
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Determines if the dialog is modal. Defaults to true.
    /// When true, clicking outside the dialog will dismiss the dialog.
    /// </summary>
    public bool? Modal { get; set; } = true;

    /// <summary>
    /// Prevents scrolling outside of the dialog while it is shown.
    /// </summary>to use
    public bool PreventScroll { get; set; } = true;

    /// <summary>
    /// Indicates if dialog should trap focus.
    /// Defaults to true.
    /// </summary>
    public bool? TrapFocus { get; set; } = true;

    /// <summary>
    /// Show the title in the header.
    /// Defaults to true.
    /// </summary>
    public bool ShowTitle { get; set; } = true;

    /// <summary>
    /// Show the dismiss button in the header.
    /// Defaults to true.
    /// </summary>
    public bool ShowDismiss { get; set; } = true;

    /// <summary>
    /// Title of the dismiss button, display in a tooltip.
    /// Defaults to "Close".
    /// </summary>
    public string? DismissTitle { get; set; } = "Close";

    /// <summary>
    /// Text to display for the primary action.
    /// </summary>
    public string? PrimaryAction { get; set; } = "OK"; //DialogResources.ButtonPrimary;

    /// <summary>
    /// When true, primary action's button is enabled.
    /// </summary>
    public bool PrimaryActionEnabled { get; set; } = true;

    /// <summary>
    /// Text to display for the secondary action.
    /// </summary>
    public string? SecondaryAction { get; set; } = "Cancel"; //DialogResources.ButtonSecondary;

    /// <summary>
    /// When true, secondary action's button is enabled.
    /// </summary>
    public bool SecondaryActionEnabled { get; set; } = true;

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
    /// The type of dialog.
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
