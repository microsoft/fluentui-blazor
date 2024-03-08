namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public class MessageOptions

{
    /// <summary>
    /// Identification of the <see cref="FluentMessageBarContainer"/> the message belongs to.
    /// </summary>
    public string? Section { get; set; }

    /// <summary>
    /// The timestamp of the message.
    /// </summary>
    public DateTime? Timestamp { get; set; }

    /// <summary>
    /// Icon to show in the message bar based on the intent of the message. See <see cref="Icon"/> for more details.
    /// </summary>
    public Icon? Icon { get; set; }

    /// <summary>
    /// Most important info to be shown in the message bar.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Message to be shown in the message bar after the title.
    /// </summary>
    public string? Body { get; set; }

    /// <summary>
    /// Link to be shown in the message bar after the title/message. 
    /// </summary>
    public ActionLink<Message>? Link { get; set; } 

    /// <summary>
    /// Action to be executed when the message bar is closed.
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
    /// Intent of the message bar. Default is MessageIntent.Info.
    /// </summary>
    public MessageIntent? Intent { get; set; } 

    /// <summary>
    /// Remove the message bar after navigation.
    /// </summary>
    public bool ClearAfterNavigation { get; set; }

    /// <summary>
    /// Timeout in seconds after which the message bar is removed. Default is null.
    /// </summary>
    public int? Timeout { get; set; }

    /// <summary>
    /// Gets or sets whether the message bar can be dismissed.
    /// </summary>
    public bool AllowDismiss { get; set; } = true;
}
