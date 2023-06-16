using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public interface IMessageBoxParameters
    {
        string? Height { get; set; }
        string? Icon { get; set; }
        Color IconColor { get; set; }
        MessageBoxIntent Intent { get; set; }
        MarkupString? MarkupMessage { get; set; }
        string? Message { get; set; }
        string PrimaryButtonText { get; set; }
        string SecondaryButtonText { get; set; }
        string? Width { get; set; }
    }
}