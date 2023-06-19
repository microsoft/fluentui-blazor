using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public interface IMessageBoxParameters : IDialogParameters
    {
        MessageBoxIntent Intent { get; set; }
        string? Icon { get; set; }
        Color IconColor { get; set; }
        string? Message { get; set; }
        MarkupString? MarkupMessage { get; set; }
        string PrimaryButtonText { get; set; }
        string SecondaryButtonText { get; set; }
        string? Width { get; set; }
        string? Height { get; set; }
    }
}