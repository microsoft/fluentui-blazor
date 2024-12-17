// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Parameters for configuring a dialog.
/// </summary>
public class DialogParameters
{
    private static readonly Dictionary<string, object?> EmptyData = new(StringComparer.Ordinal);

    /// <summary />
    public DialogParameters()
    {
        
    }

    /// <summary />
    public DialogParameters(Action<DialogParameters> implementationFactory)
    {
        implementationFactory.Invoke(this);
    }

    /// <summary>
    /// Gets or sets the title of the dialog.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets the content of the dialog.
    /// </summary>
    public IDictionary<string, object?> Data { get; set; } = EmptyData;

    /// <summary>
    /// Gets or sets the action raised when the dialog is opened or closed.
    /// </summary>
    public Action<DialogEventArgs>? OnStateChange { get; set; }
}
