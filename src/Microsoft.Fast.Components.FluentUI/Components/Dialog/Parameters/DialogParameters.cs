using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// A dialog always has a title and a callback function for the result.
/// </summary>
public class DialogParameters : ComponentParameters, IDialogParameters
{
    /// <summary>
    /// Title of the dialog
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Callback function for the result.
    /// </summary>
    public EventCallback<DialogResult> OnDialogResult { get; set; } = default!;
}
