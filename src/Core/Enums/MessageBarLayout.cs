// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Specifies the layout of a <see cref="FluentMessageBar"/>.
/// </summary>

public enum MessageBarLayout
{
    /// <summary>
    /// Single line.
    /// </summary>
    [Description("singleline")]
    SingleLine,

    /// <summary>
    /// Multi line.
    /// </summary>
    [Description("multiline")]
    MultiLine,
}
