using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public class ToastData : ComponentParameters, IToastData
    {
        public string? Id { get; set; }
        public ToastIntent Intent { get; set; }
        public string? Title { get; set; }
        public ToastTopCTAType TopCTAType { get; set; }
        public ToastAction? TopAction { get; set; }
        public (Icon Value, Color Color)? Icon { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public int? Timeout { get; set; }
        public ToastAction? PrimaryAction { get; set; }
        public ToastAction? SecondaryAction { get; set; }
        public EventCallback<ToastResult> OnToastResult { get; set; }
    }
}
