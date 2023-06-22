using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public interface IMessageBoxParameters
    {
        string? Title { get; set; }
        MessageBoxIntent Intent { get; set; }
        string? Icon { get; set; }
        Color IconColor { get; set; }
        string? Message { get; set; }
        MarkupString? MarkupMessage { get; set; }
        string? PrimaryButton { get; set; }
        string? SecondaryButton { get; set; }
        string? Width { get; set; }
        string? Height { get; set; }

        EventCallback<DialogResult> OnDialogResult { get; set; }
    }
}