// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class MessageOptions : IFluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageOptions"/> class.
    /// </summary>
    public MessageOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageOptions"/> class
    /// using the specified implementation factory.
    /// </summary>
    /// <param name="implementationFactory"></param>
    public MessageOptions(Action<MessageOptions> implementationFactory)
    {
        implementationFactory.Invoke(this);
    }
    /// <summary>
    /// Gets or sets the unique identifier of the Dialog element.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the CSS class names.
    /// If given, these will be included in the class attribute of the `fluent-dialog` or `fluent-drawer` element.
    /// To apply you styles to the `dialog` element, you need to create a class like `my-class::part(dialog) { ... }`
    /// </summary>
    public string? Class { get; set; }

    /// <summary>
    /// Gets or sets the in-line styles.
    /// If given, these will be included in the style attribute of the `dialog` element.
    /// </summary>
    public string? Style { get; set; }

    /// <summary>
    /// Gets or sets the component <see href="https://developer.mozilla.org/docs/Web/CSS/margin">CSS margin</see> property.
    /// </summary>
    public string? Margin { get; set; }

    /// <summary>
    /// Gets or sets the component <see href="https://developer.mozilla.org/docs/Web/CSS/padding">CSS padding</see> property.
    /// </summary>
    public string? Padding { get; set; }

    /// <summary>
    /// Gets or sets custom data, to attach any user data object to the component.
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

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
    public ActionLink<IMessageInstance>? Link { get; set; }

    /// <summary>
    /// Gets or sets the action to be executed when the message bar is closed.
    /// </summary>
    public Func<IMessageInstance, Task>? OnClose { get; set; }

    /// <summary>
    /// Action to be executed for the primary button.
    /// </summary>
    public ActionButton<IMessageInstance>? PrimaryAction { get; set; } = new();

    /// <summary>
    /// Action to be executed for the secondary button.
    /// </summary>
    public ActionButton<IMessageInstance>? SecondaryAction { get; set; } = new();

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

    /// <summary>
    /// Gets or sets whether the message bar can be dismissed.
    /// </summary>
    public bool AllowDismiss { get; set; } = true;
}
