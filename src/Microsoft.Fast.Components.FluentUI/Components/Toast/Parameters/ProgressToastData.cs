using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class ProgressToastData
{

    public string? Subtitle { get; set; }
    public string? Details { get; set; }
    //public ToastAction? PrimaryAction { get; set; }
    //public ToastAction? SecondaryAction { get; set; }
    public string? Progress { get; set; }
    public EventCallback<string?> ProgressChanged { get; set; }
}
