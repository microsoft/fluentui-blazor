using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// 
/// </summary>
public class DialogParameters<TContent> : ComponentParameters
{
    /// <summary>
    /// Determines the alignment of the dialog.
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
    public bool Modal { get; set; } = true;

    /// <summary>
    /// When true, shows the title in the header.
    /// </summary>
    public bool ShowTitle { get; set; } = true;

    /// <summary>
    /// When true, shows the dismiss button in the header.
    /// </summary>
    public bool ShowDismiss { get; set; } = true;

    /// <summary>
    /// Text to display on the primary button.
    /// </summary>
    public string? PrimaryButton { get; set; } = "Ok"; //DialogResources.ButtonPrimary;

    /// <summary>
    /// Text to display on the secondary button.
    /// </summary>
    public string? SecondaryButton { get; set; } = "Cancel"; //DialogResources.ButtonSecondary;

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
    /// Content to pass to and from the dialog.
    /// </summary>
    public TContent Content { get; set; } = default!;

    /// <summary>
    /// Callback function for the result.
    /// </summary>
    public EventCallback<DialogResult> OnDialogResult { get; set; } = default!;
}
