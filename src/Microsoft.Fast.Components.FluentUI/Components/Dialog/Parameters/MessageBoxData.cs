using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class MessageBoxData
{
    public string? Title { get; set; }
    public MessageBoxIntent Intent { get; set; }
    public Icon? Icon { get; set; }
    public Color IconColor { get; set; }
    public string? Message { get; set; }
    public MarkupString? MarkupMessage { get; set; }
    public bool Result { get; set; }
}
