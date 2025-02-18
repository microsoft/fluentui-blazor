// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The size of the spinner.
/// </summary>
public enum SpinnerSize
{
    /// <summary>
    /// The tiny size.
    /// </summary>
    [Description("tiny")]
    Tiny,

    /// <summary>
    /// The extra-small size.
    /// </summary>
    [Description("extra-small")]
    ExtraSmall,

    /// <summary>
    /// The small size.
    /// </summary>
    [Description("small")]
    Small,

    /// <summary>
    /// The medium size.
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// The large size.
    /// </summary>
    [Description("large")]
    Large,

    /// <summary>
    /// The extra-large size.
    /// </summary>
    [Description("extra-large")]
    ExtraLarge,

    /// <summary>
    /// The huge size.
    /// </summary>
    [Description("huge")]
    Huge,
}
