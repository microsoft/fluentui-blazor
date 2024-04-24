using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class MessageBoxContent
{
    /// <summary>
    /// Gets or sets the title of the message box.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the intent of the message box. See <see cref="MessageBoxIntent"/> for details.
    /// </summary>    
    public MessageBoxIntent Intent { get; set; }

    /// <summary>
    /// Gets or sets the name of the icon to display.
    /// </summary>
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the color of the icon to display.
    /// </summary>
    public Color IconColor { get; set; }

    /// <summary>
    /// Gets or sets the text to display in the message box.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the html text to display in the message box.
    /// </summary>
    public MarkupString? MarkupMessage { get; set; }
}
