using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// A dialog always has a title and a callback function for the result.
/// </summary>
public class DialogParameters<TData> : ComponentParameters
{
    /// <summary>
    /// Determines the alignment of the panel.
    /// </summary>
    public virtual HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Center;

    /// <summary>
    /// Title of the dialog
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Determines if the panel is modal. Defaults to true.
    /// When true, clicking outside the panel will dismiss the panel.
    /// </summary>
    public bool Modal { get; set; } = true;

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
    /// Width of the panel. Must be a valid CSS width value like "600px" or "3em"
    /// </summary>
    public string? Width { get; set; }

    /// <summary>
    /// Height of the panel. Must be a valid CSS height value like "600px" or "3em"
    /// Only used if Alignment is set to "HorizotalAlignment.Center"
    /// </summary>  
    public string? Height { get; set; }

    /// <summary>
    /// Data to pass to and from the panel.
    /// </summary>
    public TData Data { get; set; } = default!;

    /// <summary>
    /// Callback function for the result.
    /// </summary>
    public EventCallback<DialogResult> OnDialogResult { get; set; } = default!;
}
