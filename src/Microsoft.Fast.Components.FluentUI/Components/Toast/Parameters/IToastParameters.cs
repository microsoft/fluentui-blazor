using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public interface IToastParameters
    {
        (string Name, Color Color, IconVariant Variant)? Icon { get; set; }
        string? Id { get; set; }
        ToastIntent Intent { get; set; }
        EventCallback<ToastResult> OnPrimaryAction { get; set; }
        EventCallback<ToastResult> OnSecondaryAction { get; set; }
        EventCallback<ToastResult> OnTopAction { get; set; }
        ToastAction? PrimaryAction { get; set; }
        ToastAction? SecondaryAction { get; set; }
        int? Timeout { get; set; }
        DateTime Timestamp { get; set; }
        string? Title { get; set; }
        ToastAction? TopAction { get; set; }
        ToastTopCTAType TopCTAType { get; set; }
    }
}