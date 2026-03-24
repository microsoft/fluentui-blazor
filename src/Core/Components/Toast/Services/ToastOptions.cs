// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for configuring a Toast.
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
    /// <param name="implementationFactory"></param>
    public ToastOptions(Action<ToastOptions> implementationFactory)
    {
        implementationFactory.Invoke(this);
    }

    /// <summary>
    /// Gets or sets the unique identifier of the Toast element.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the CSS class names.
    /// If given, these will be included in the class attribute of the `fluent-Toast` or `fluent-drawer` element.
    /// To apply you styles to the `Toast` element, you need to create a class like `my-class::part(Toast) { ... }`
    /// </summary>
    public string? Class { get; set; }

    /// <summary>
    /// Gets or sets the in-line styles.
    /// If given, these will be included in the style attribute of the `Toast` element.
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
    /// Gets or sets the timeout duration for the Toast in milliseconds.
    /// </summary>
    public int Timeout { get; set; } = 5000;

    /// <summary>
    /// Gets or sets the toast position on screen.
    /// </summary>
    public ToastPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset in pixels.
    /// </summary>
    public int VerticalOffset { get; set; } = 16;

    /// <summary>
    /// Gets or sets the horizontal offset in pixels.
    /// </summary>
    public int HorizontalOffset { get; set; } = 20;

    /// <summary>
    /// Gets or sets the toast intent.
    /// </summary>
    public ToastIntent Intent { get; set; } = ToastIntent.Info;

    /// <summary>
    /// Gets or sets the politeness level used for accessibility.
    /// </summary>
    public ToastPoliteness? Politeness { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the timeout pauses while hovering the toast.
    /// </summary>
    public bool PauseOnHover { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the timeout pauses while the window is blurred.
    /// </summary>
    public bool PauseOnWindowBlur { get; set; }

    /// <summary>
    /// Gets or sets the toast title.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the body text of the toast.
    /// </summary>
    public string? Body { get; set; }

    /// <summary>
    /// Gets or sets the subtitle of the toast.
    /// </summary>
    public string? Subtitle { get; set; }

    /// <summary>
    /// Gets or sets the status text shown for progress toasts.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the first quick action label.
    /// </summary>
    public string? QuickAction1 { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the first quick action is clicked.
    /// </summary>
    public Func<Task>? QuickAction1Callback { get; set; }

    /// <summary>
    /// Gets or sets the second quick action label.
    /// </summary>
    public string? QuickAction2 { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the second quick action is clicked.
    /// </summary>
    public Func<Task>? QuickAction2Callback { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the dismiss button is shown.
    /// </summary>
    public bool ShowDismissButton { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether progress visuals are shown.
    /// </summary>
    public bool ShowProgress { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the progress is indeterminate.
    /// </summary>
    public bool Indeterminate { get; set; } = true;

    /// <summary>
    /// Gets or sets the current progress value.
    /// </summary>
    public int? Value { get; set; }

    /// <summary>
    /// Gets or sets the maximum progress value.
    /// </summary>
    public int? Max { get; set; } = 100;

    /// <summary>
    /// Gets or sets custom media rendered in the media slot.
    /// </summary>
    public RenderFragment? Media { get; set; }

    /// <summary>
    /// Gets or sets custom content rendered in the title slot.
    /// </summary>
    public RenderFragment? TitleContent { get; set; }

    /// <summary>
    /// Gets or sets custom content rendered in the subtitle slot.
    /// </summary>
    public RenderFragment? SubtitleContent { get; set; }

    /// <summary>
    /// Gets or sets custom content rendered in the footer slot.
    /// </summary>
    public RenderFragment? FooterContent { get; set; }

    /// <summary>
    /// Gets or sets custom content rendered in the default slot.
    /// </summary>
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the action raised when the toast lifecycle status changes.
    /// </summary>
    public Action<ToastEventArgs>? OnStatusChange { get; set; }

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
