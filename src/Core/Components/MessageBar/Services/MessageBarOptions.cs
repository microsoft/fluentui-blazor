// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for configuring a message bar displayed by the <see cref="INotificationService"/>.
/// </summary>
public class MessageBarOptions : IFluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageBarOptions"/> class.
    /// </summary>
    public MessageBarOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageBarOptions"/> class
    /// using the specified implementation factory.
    /// </summary>
    /// <param name="implementationFactory"></param>
    public MessageBarOptions(Action<MessageBarOptions> implementationFactory)
    {
        implementationFactory.Invoke(this);
    }

    /// <summary>
    /// Gets or sets the unique identifier of the MessageBar element.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the CSS class name.
    /// </summary>
    public string? Class { get; set; }

    /// <summary>
    /// Gets or sets the in-line styles.
    /// </summary>
    public string? Style { get; set; }

    /// <summary>
    /// Gets or sets the component <see href="https://developer.mozilla.org/docs/Web/CSS/margin">CSS margin</see>
    /// property.
    /// </summary>
    public string? Margin { get; set; }

    /// <summary>
    /// Gets or sets the component <see href="https://developer.mozilla.org/docs/Web/CSS/padding">CSS padding</see>
    /// property.
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
    /// Gets a list of message bar parameters.
    /// Each parameter must correspond to a <c>[Parameter]</c> property defined in the message bar component.
    /// </summary>
    public IDictionary<string, object?> Parameters { get; set; } = new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    /// Gets or sets the section identifier for the message bar provider.
    /// This message bar will only be displayed in a <see cref="FluentMessageBarProvider"/> with the same section identifier.
    /// </summary>
    public string Section { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the intent of the message bar.
    /// </summary>
    public MessageBarIntent? Intent { get; set; }

    /// <summary>
    /// Gets or sets the layout of the message bar.
    /// </summary>
    public MessageBarLayout? Layout { get; set; }

    /// <summary>
    /// Gets or sets the shape of the message bar.
    /// </summary>
    public MessageBarShape? Shape { get; set; }

    /// <summary>
    /// Gets or sets the fade in animation when the message bar is shown.
    /// </summary>
    public MessageBarAnimation? Animation { get; set; }

    /// <summary>
    /// Gets or sets the <code>aria-live</code> attribute, used to inform assistive technologies (like screen readers)
    /// about updates to dynamic content.
    /// </summary>
    public AriaLive? AriaLive { get; set; }

    /// <summary>
    /// Gets or sets the icon to show in the message bar.
    /// When set, overrides the default icon determined by <see cref="Intent"/>.
    /// </summary>
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the title displayed in the message bar.
    /// For security reasons, the content is sanitized using the configured <see cref="LibraryConfiguration.MarkupSanitized"/> before rendering.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the message displayed in the message bar.
    /// For security reasons, the content is sanitized using the configured <see cref="LibraryConfiguration.MarkupSanitized"/> before rendering.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the message bar can be dismissed by the user.
    /// Default is `true`.
    /// </summary>
    public bool AllowDismiss { get; set; } = true;

    /// <summary>
    /// Gets or sets the timestamp when the message was created.
    /// </summary>
    public DateTime? TimeStamp { get; set; }

    /// <summary>
    /// Gets or sets the lifetime of the message bar.
    /// When set to a positive value, the message bar is automatically removed from the service
    /// (and from the <see cref="FluentMessageBarProvider"/>) after this duration elapses.
    /// When `null`, the message bar stays visible until it is dismissed programmatically
    /// or by the user.
    /// </summary>
    public TimeSpan? Lifetime { get; set; }

    /// <summary>
    /// Gets or sets the action raised when the message bar lifecycle status changes.
    /// </summary>
    public Action<MessageBarEventArgs>? OnStatusChange { get; set; }

    /// <summary>
    /// Gets the class, including the optional <see cref="Margin"/> and <see cref="Padding"/> values.
    /// </summary>
    internal virtual string? ClassValue => new CssBuilder(Class)
        .AddClass(Margin.ConvertSpacing().Class)
        .AddClass(Padding.ConvertSpacing().Class)
        .Build();

    /// <summary>
    /// Gets the style builder, containing the default margin and padding values.
    /// </summary>
    internal virtual string? StyleValue => new StyleBuilder(Style)
        .AddStyle("margin", Margin.ConvertSpacing().Style)
        .AddStyle("padding", Padding.ConvertSpacing().Style)
        .Build();
}
