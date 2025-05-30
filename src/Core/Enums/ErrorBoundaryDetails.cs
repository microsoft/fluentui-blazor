// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The details to display in the <see cref="FluentErrorBoundary" />.
/// </summary>
public enum ErrorBoundaryDetails
{
    /// <summary>
    /// The error boundary does not display any details about the error.
    /// </summary>
    None,

    /// <summary>
    /// The error boundary displays the exception message about the error.
    /// </summary>
    ErrorMessage,

    /// <summary>
    /// The error boundary displays the stack trace of the error.
    /// </summary>
    ErrorStack,
}
