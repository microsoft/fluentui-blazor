// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

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
    /// Gets a list of Toast parameters.
    /// Each parameter must correspond to a `[Parameter]` property defined in the component.
    /// </summary>
    public IDictionary<string, object?> Parameters { get; set; } = new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    /// Gets or sets the action raised when the Toast is opened or closed.
    /// </summary>
    public Action<ToastEventArgs>? OnStateChange { get; set; }

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
