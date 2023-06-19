using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class DialogParameters : ComponentParameters, IDialogParameters
{
    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public EventCallback<DialogResult> OnDialogResult { get; set; } = default!;
}
