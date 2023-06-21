using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public interface IDialogParameters
    {
        string? Title { get; set; }
        EventCallback<DialogResult> OnDialogResult { get; set; }
    }
}