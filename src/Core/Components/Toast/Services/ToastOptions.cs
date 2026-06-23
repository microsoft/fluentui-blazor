// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for configuring a toast displayed by the <see cref="INotificationService"/>.
/// </summary>
public class ToastOptions : IFluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ToastOptions"/> class.
    /// </summary>
    public ToastOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ToastOptions"/> class 
    /// using the specified implementation factory.
    /// </summary>
    /// <param name="implementationFactory">Action used to configure the toast options.</param>
    public ToastOptions(Action<ToastOptions> implementationFactory)
    {
        implementationFactory.Invoke(this);
    }

    /// <summary>
    /// Gets or sets the unique identifier of the Toast element.
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
    /// Gets or sets the component <see href="https://developer.mozilla.org/docs/Web/CSS/margin"> CSS margin</see>
    /// property.
    /// </summary>
    public string? Margin { get; set; }

    /// <summary>
    /// Gets or sets the component <see href="https://developer.mozilla.org/docs/Web/CSS/padding"> CSS padding</see>
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
    /// Gets a list of toast parameters.
    /// Each parameter must correspond to a <c>[Parameter]</c> property defined in the toast component.
    /// </summary>
    public IDictionary<string, object?> Parameters { get; set; } = new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    /// Gets or sets the lifetime of the toast.
    /// When set to a positive value, the toast is automatically removed after this duration elapses.
    /// When `TimeSpan.Zero`, the toast stays visible until it is dismissed programmatically or by the user.
    /// </summary>
    public TimeSpan? Lifetime { get; set; }

    /// <summary>
    /// Gets or sets the toast position on screen.
    /// </summary>
    public ToastPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset in pixels.
    /// </summary>
    public int? VerticalOffset { get; set; }

    /// <summary>
    /// Gets or sets the horizontal offset in pixels.
    /// </summary>
    public int? HorizontalOffset { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the toast uses inverted colors.
    /// </summary>
    public bool? Inverted { get; set; }

    /// <summary>
    /// Gets or sets the toast intent.
    /// </summary>
    public ToastIntent? Intent { get; set; }

    /// <summary>
    /// Gets or sets the politeness level used for accessibility.
    /// </summary>
    public ToastPoliteness? Politeness { get; set; }

        /// <summary>
    /// Gets or sets the toast title.
    /// For security reasons, the content is sanitized using the configured <see cref="LibraryConfiguration.MarkupSanitized"/> before rendering.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the message displayed in the toast.
    /// For security reasons, the content is sanitized using the configured <see cref="LibraryConfiguration.MarkupSanitized"/> before rendering.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the subtitle of the toast.
    /// For security reasons, the content is sanitized using the configured <see cref="LibraryConfiguration.MarkupSanitized"/> before rendering.
    /// </summary>
    public string? Subtitle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the timeout pauses while hovering the toast.
    /// </summary>
    public bool? PauseOnHover { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the timeout pauses while the window is blurred.
    /// </summary>
    public bool? PauseOnWindowBlur { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the toast can be dismissed by the user.
    /// </summary>
    public bool? AllowDismiss { get; set; }

    /// <summary>
    /// Gets or sets the dismiss action link displayed in the toast.
    /// Only relevant when <see cref="AllowDismiss"/> is <see langword="true"/>.
    /// If `CallbackAsync` is set, the toast is not closed automatically, and the action is responsible for closing the toast by calling <see cref="IToastInstance.CloseAsync(ToastCloseReason, object?)"/>.
    /// If `CallbackAsync` is not set, the toast is closed setting the <see cref="ToastResult.Reason"/> to <see cref="ToastCloseReason.Dismissed"/>.
    /// </summary>
    public ToastOptionsAction DismissAction { get; } = new ToastOptionsAction();

    /// <summary>
    /// Gets or sets the primary action for the toast.
    /// This action link in displayed in the footer of the toast, and is used to trigger the most important action related to the toast message.
    /// When the user clicks on this action, the toast is not closed automatically, and the action is responsible for closing the toast by calling <see cref="IToastInstance.CloseAsync(ToastCloseReason, object?)"/> with the appropriate <see cref="ToastCloseReason"/>.
    /// </summary>
    public ToastOptionsAction QuickAction1 { get; } = new ToastOptionsAction();

    /// <summary>
    /// Gets or sets the secondary action for the toast.
    /// This action link in displayed in the footer of the toast, and is used to trigger the secondary action related to the toast message.
    /// When the user clicks on this action, the toast is not closed automatically, and the action is responsible for closing the toast by calling <see cref="IToastInstance.CloseAsync(ToastCloseReason, object?)"/> with the appropriate <see cref="ToastCloseReason"/>.
    /// </summary>
    public ToastOptionsAction QuickAction2 { get; } = new ToastOptionsAction();

    /// <summary>
    /// Gets or sets the action raised when the toast lifecycle status changes.
    /// </summary>
    public Action<ToastEventArgs>? OnStatusChange { get; set; }

    /// <summary>
    /// Gets or sets the icon rendered in the toast header.
    /// When set, this overrides the default icon determined by the <see cref="Intent" />
    /// (Warning, Error, Success, Info) of the toast.
    /// </summary>
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets the width of the toast.
    /// </summary>
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets when the <see cref="IToastInstance.Result"/> task is completed.
    /// The default is <see cref="ToastResultTiming.Closed"/>.
    /// </summary>
    public ToastResultTiming ResultTiming { get; set; } = ToastResultTiming.Closed;

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
