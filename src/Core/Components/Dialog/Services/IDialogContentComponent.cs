// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A component implementing this interface can be used as dialog content.
/// </summary>
public interface IDialogContentComponent
{
}

/// <summary>
/// A component implementing this interface can be used as dialog content.
/// </summary>
/// <typeparam name="TContent"></typeparam>
public interface IDialogContentComponent<TContent> : IDialogContentComponent
{
    /// <summary>
    /// Gets or sets the content to display in the dialog.
    /// </summary>
    TContent Content { get; set; }
}
