// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for DialogReference
/// </summary>
public interface IDialogInstance
{
    /// <summary>
    /// Gets the component type of the dialog.
    /// </summary>
    internal Type ComponentType { get; }

    /// <summary>
    /// Gets the unique identifier for the dialog.
    /// If this value is not set in the <see cref="DialogOptions"/>, a new identifier is generated.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets the options used to configure the dialog.
    /// </summary>
    DialogOptions Options { get; }

    /// <summary>
    /// Gets the result of the dialog.
    /// </summary>
    Task<DialogResult> Result { get; }

    /// <summary>
    /// Closes the dialog with a cancel result.
    /// </summary>
    /// <returns></returns>
    Task CloseAsync();

    /// <summary>
    /// Closes the dialog with the specified result.
    /// </summary>
    /// <param name="result">Result to close the dialog with.</param>
    /// <returns></returns>
    Task CloseAsync(DialogResult result);
}
