// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for configuring a dialog.
/// </summary>
public class DialogOptions : IFluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DialogOptions"/> class.
    /// </summary>
    public DialogOptions()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogOptions"/> class
    /// using the specified implementation factory.
    /// </summary>
    /// <param name="implementationFactory"></param>
    public DialogOptions(Action<DialogOptions> implementationFactory)
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
    /// Gets the header title of the dialog.
    /// </summary>
    public DialogOptionsHeader Header { get; } = new();

    /// <summary>
    /// Gets the footer actions of the dialog.
    /// </summary>
    public DialogOptionsFooter Footer { get; } = new();

    /// <summary>
    /// Gets or sets the dialog alignment (by default, the dialog is centered).
    /// </summary>
    public DialogAlignment? Alignment { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this dialog is displayed modally.
    /// By default, the dialog is displayed modally (Modal = true).
    /// </summary>
    /// <remarks>
    /// When a dialog is displayed modally, no input (keyboard or mouse click) can occur except to objects on the modal dialog.
    /// The program must hide or close a modal dialog (usually in response to some user action) before input to another dialog can occur.
    /// </remarks>
    public bool? Modal { get; set; }

    /// <summary>
    /// Gets or sets the default size of the dialog.
    /// This value is overridden by <see cref="Width" /> or <see cref="Height" />. 
    /// </summary>
    public DialogSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the width of the dialog. Must be a valid CSS width value like '600px' or '3em'
    /// </summary>
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the dialog. Must be a valid CSS height value like '600px' or '3em'
    /// Only used if Alignment is set to <see cref="DialogAlignment.Default"/>.
    /// </summary>
    public string? Height { get; set; }

    /// <summary>
    /// Gets a list of dialog parameters.
    /// Each parameter must correspond to a `[Parameter]` property defined in the component.
    /// </summary>
    public IDictionary<string, object?> Parameters { get; set; } = new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    /// Gets or sets the action raised when the dialog is opened or closed.
    /// </summary>
    public Action<DialogEventArgs>? OnStateChange { get; set; }

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
