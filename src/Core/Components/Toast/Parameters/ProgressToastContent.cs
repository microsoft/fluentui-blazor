using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class ProgressToastContent
{
    public string? Subtitle { get; set; }
    public string? Details { get; set; }
    public int? Progress { get; set; }

    public EventCallback<int?> ProgressChanged { get; set; }
}
