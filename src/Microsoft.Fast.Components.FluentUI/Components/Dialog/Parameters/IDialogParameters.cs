using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public interface IDialogParameters
    {
        EventCallback<DialogResult> OnDialogResult { get; set; }
        string? Title { get; set; }
    }
}