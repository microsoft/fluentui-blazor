using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public interface IToastData
    {
        ToastIntent Intent { get; set; }
        string? Title { get; set; }
        ToastTopCTAType TopCTAType { get; set; }
        ToastAction? TopAction { get; set; }

        (string Name, Color Color, IconVariant Variant)? Icon { get; set; }
        DateTime Timestamp { get; set; }
        int? Timeout { get; set; }

        ToastAction? PrimaryAction { get; set; }

        ToastAction? SecondaryAction { get; set; }

        EventCallback<ToastResult> OnToastResult { get; set; }
    }
}
