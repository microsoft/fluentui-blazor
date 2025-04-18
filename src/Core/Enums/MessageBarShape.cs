// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Specifies the shape of a <see cref="FluentMessageBar"/>.
/// </summary>

public enum MessageBarShape
{
    /// <summary>
    /// Square shape.
    /// </summary>
    [Description("square")]
    Square,

    /// <summary>
    /// Rounded shape.
    /// </summary>
    [Description("rounded")]
    Rounded,
}
