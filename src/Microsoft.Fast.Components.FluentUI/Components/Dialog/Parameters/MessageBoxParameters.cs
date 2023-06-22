using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// The set of parameters for a message box.
/// </summary>
public class MessageBoxParameters : IMessageBoxParameters
{
    /// <summary>
    /// The title of the splash screen.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// The intent of the message box. See <see cref="MessageBoxIntent"/> for more details.
    /// </summary>
    [Parameter]
    public MessageBoxIntent Intent { get; set; } = MessageBoxIntent.Info;

    /// <summary>
    /// The actual message to display.
    /// Do not use this property if you are using the <see cref="MarkupMessage"/> property.
    /// </summary>
    [Parameter]
    public string? Message { get; set; } = string.Empty;

    /// <summary>
    /// The actual message to display as a MarkupString.
    /// Do not use this property if you are using the <see cref="Message"/> property.
    /// </summary>
    [Parameter]
    public MarkupString? MarkupMessage { get; set; }

    /// <summary>
    /// Icon to display in the message box.
    /// </summary>
    [Parameter]
    public string? Icon { get; set; }

    /// <summary>
    /// Color of the icon.
    /// </summary>
    [Parameter]
    public Color IconColor { get; set; } = Color.Accent;

    /// <summary>
    /// Text to display on the primary button.
    /// </summary>
    [Parameter]
    public string? PrimaryButton { get; set; } = "Ok"; //DialogResources.ButtonPrimary;

    /// <summary>
    /// Text to display on the secondary button.
    /// </summary>
    [Parameter]
    public string? SecondaryButton { get; set; } = "Cancel"; //DialogResources.ButtonSecondary;

    /// <summary>
    /// The width of the message box.
    /// Must be a valid CSS width string like "600px" or "3em".
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Height of the message box.
    /// Usually no need to set this. Height adapts to the displayed content by default.
    /// Must be a valid CSS height string like "600px" or "3em".
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// The callback to invoke to return the result when the dialog is closed or dismissed.
    /// </summary>
    [Parameter]
    public EventCallback<DialogResult> OnDialogResult { get; set; } = default!;
}
