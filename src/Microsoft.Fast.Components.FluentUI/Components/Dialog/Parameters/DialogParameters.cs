using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class DialogParameters : ComponentParameters, IDialogParameters
{
    public string? Title { get; set; }
    public EventCallback<DialogResult> OnDialogResult { get; set; } = default!;
}
