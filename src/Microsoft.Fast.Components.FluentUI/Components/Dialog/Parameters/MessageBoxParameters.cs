using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// The set of parameters for a message box.
/// </summary>
public class MessageBoxParameters : DialogParameters, IMessageBoxParameters
{
    /// <summary>
    /// The intent of the message box. See <see cref="MessageBoxIntent"/> for more details.
    /// </summary>
    public MessageBoxIntent Intent { get; set; } = MessageBoxIntent.Info;

    /// <summary>
    /// The actual message to display.
    /// Do not use this property if you are using the <see cref="MarkupMessage"/> property.
    /// </summary>
    public string? Message { get; set; } = string.Empty;

    /// <summary>
    /// The actual message to display as a MarkupString.
    /// Do not use this property if you are using the <see cref="Message"/> property.
    /// </summary>
    public MarkupString? MarkupMessage { get; set; }

    /// <summary>
    /// Icon to display in the message box.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Color of the icon.
    /// </summary>
    public Color IconColor { get; set; } = Color.Accent;

    /// <summary>
    /// Text to display on the primary button.
    /// </summary>
    public string PrimaryButtonText { get; set; } = "Ok"; //DialogResources.ButtonPrimary;

    /// <summary>
    /// Text to display on the secondary button.
    /// </summary>
    public string SecondaryButtonText { get; set; } = "Cancel"; //DialogResources.ButtonSecondary;

    /// <summary>
    /// The width of the message box.
    /// Must be a valid CSS width string like "600px" or "3em".
    /// </summary>
    public string? Width { get; set; }

    /// <summary>
    /// Height of the message box.
    /// Usually no need to set this. Height adapts to the displayed content by default.
    /// Must be a valid CSS height string like "600px" or "3em".
    /// </summary>
    public string? Height { get; set; }

}
