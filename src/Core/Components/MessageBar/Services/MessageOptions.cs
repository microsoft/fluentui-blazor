namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class MessageOptions

{
    /// <summary>
    /// Gets or sets the identification of the <see cref="FluentMessageBarProvider"/> the message belongs to.
    /// </summary>
    public string? Section { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the message.
    /// </summary>
    public DateTime? Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the icon to show in the message bar based on the intent of the message. See <see cref="Icon"/> for more details.
    /// </summary>
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// Most important info to be shown in the message bar.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the message to be shown in the message bar after the title.
    /// </summary>
    public string? Body { get; set; }

    /// <summary>
    /// Gets or sets the link to be shown in the message bar after the title/message. 
    /// </summary>
    public ActionLink<Message>? Link { get; set; }

    /// <summary>
    /// Gets or sets the action to be executed when the message bar is closed.
    /// </summary>
    public Func<Message, Task>? OnClose { get; set; }

    /// <summary>
    /// Action to be executed for the primary button.
    /// </summary>
    public ActionButton<Message>? PrimaryAction { get; set; } = new();

    /// <summary>
    /// Action to be executed for the secondary button.
    /// </summary>
    public ActionButton<Message>? SecondaryAction { get; set; } = new();

    /// <summary>
    /// Gets or sets the intent of the message bar. 
    /// Default is MessageIntent.Info.
    /// </summary>
    public MessageIntent? Intent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the message will be removed after navigation.
    /// </summary>
    public bool ClearAfterNavigation { get; set; }

    /// <summary>
    /// Gets or sets the timeout in milliseconds after which the message bar is removed. Default is null.
    /// </summary>
    public int? Timeout { get; set; }
}
