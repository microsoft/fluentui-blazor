using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class MessageBoxContent
{
    /// <summary>
    /// The title of the message box.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The intent of the message box. See <see cref="MessageBoxIntent"/> for details.
    /// </summary>    
    public MessageBoxIntent Intent { get; set; }

    /// <summary>
    /// Name of the icon to display.
    /// </summary>
    public Icon? Icon { get; set; }

    /// <summary>
    /// Color of the icon to display.
    /// </summary>
    public Color IconColor { get; set; }

    /// <summary>
    /// The text to display in the message box.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// The html text to display in the message box.
    /// </summary>
    public MarkupString? MarkupMessage { get; set; }
}
