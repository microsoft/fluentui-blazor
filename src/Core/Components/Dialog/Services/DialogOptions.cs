// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for configuring a dialog.
/// </summary>
public class DialogOptions : IFluentComponentBase
{
    /// <summary />
    public DialogOptions()
    {
    }

    /// <summary />
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

    ///// <summary>
    ///// Gets or sets a value indicating whether this dialog is displayed modally.
    ///// </summary>
    ///// <remarks>
    ///// When a dialog is displayed modally, no input (keyboard or mouse click) can occur except to objects on the modal dialog.
    ///// The program must hide or close a modal dialog (usually in response to some user action) before input to another dialog can occur.
    ///// </remarks>
    //public bool Modal { get; set; }

    /// <summary>
    /// Gets or sets the dialog alignment.
    /// </summary>
    public DialogAlignment Alignment { get; set; } = DialogAlignment.Default;

    /// <summary>
    /// Gets the content of the dialog.
    /// </summary>
    public IDictionary<string, object?> Content { get; set; } = new Dictionary<string, object?>(StringComparer.Ordinal);

    /// <summary>
    /// Gets or sets the action raised when the dialog is opened or closed.
    /// </summary>
    public Action<DialogEventArgs>? OnStateChange { get; set; }
}
