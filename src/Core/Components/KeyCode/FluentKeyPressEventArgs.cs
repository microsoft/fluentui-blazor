// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides data for a KeyPress event.
/// </summary>
public class FluentKeyPressEventArgs : EventArgs
{
    /// <summary>
    /// Gets the key press event data associated with the current operation.
    /// </summary>
    public required KeyPress KeyPress { get; init; }

    /// <summary>
    /// Gets the value associated with this instance.
    /// </summary>
    public required string Value { get; init; }
}
