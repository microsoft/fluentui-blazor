using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class MessageBoxParameters : DialogParameters, IMessageBoxParameters
{
    public MessageBoxIntent Intent { get; set; } = MessageBoxIntent.Info;

    public string? Message { get; set; } = string.Empty;

    public MarkupString? MarkupMessage { get; set; }

    public string? Icon { get; set; }

    public Color IconColor { get; set; } = Color.Accent;
    public string PrimaryButtonText { get; set; } = "Ok"; //FluentPanelResources.ButtonPrimary;

    public string SecondaryButtonText { get; set; } = "Cancel"; //FluentPanelResources.ButtonSecondary;

    public string? Width { get; set; }

    public string? Height { get; set; }

}
