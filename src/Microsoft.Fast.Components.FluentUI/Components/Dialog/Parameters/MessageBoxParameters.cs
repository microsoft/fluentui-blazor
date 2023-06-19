using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class MessageBoxParameters : DialogParameters, IMessageBoxParameters
{
    [Parameter]
    public MessageBoxIntent Intent { get; set; } = MessageBoxIntent.Info;

    [Parameter]
    public string? Message { get; set; } = string.Empty;

    [Parameter]
    public MarkupString? MarkupMessage { get; set; }

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public Color IconColor { get; set; } = Color.Accent;

    [Parameter]
    public string PrimaryButtonText { get; set; } = "Ok"; //DialogResources.ButtonPrimary;

    [Parameter]
    public string SecondaryButtonText { get; set; } = "Cancel"; //DialogResources.ButtonSecondary;

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public string? Height { get; set; }

}
