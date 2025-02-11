// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The size of the <see cref="TextAreaSize" />.
/// </summary>
public enum TextAreaSize
{
    /// <summary>
    /// The small size.
    /// </summary>
    [Description("small")]
    Small,

    /// <summary>
    /// Default value for the size.
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// The large size.
    /// </summary>
    [Description("large")]
    Large,
}
