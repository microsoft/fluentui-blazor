// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

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

    /// <inheritdoc />
    public string? Id { get; set; }

    /// <inheritdoc />
    public string? Class { get; set; }

    /// <inheritdoc />
    public string? Style { get; set; }

    /// <inheritdoc />
    public object? Data { get; set; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Gets or sets the title of the dialog.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the dialog alignment.
    /// </summary>
    public DialogAlignment Alignment { get; set; } = DialogAlignment.Default;

    /// <summary>
    /// Gets or sets the width of the dialog. Must be a valid CSS width value like '600px' or '3em'
    /// Only used if Alignment is set to <see cref="DialogAlignment.Default"/>.
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
}
