using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public interface IDialogParameters
    {
        HorizontalAlignment Alignment { get; set; }
        string? Title { get; set; }
        bool Modal { get; set; }
        bool ShowDismiss { get; set; }
        string? PrimaryButton { get; set; }
        string? SecondaryButton { get; set; }
        string? Width { get; set; }
        string? Height { get; set; }
        object? Data { get; set; }
        EventCallback<DialogResult> OnDialogResult { get; set; }
    }
}