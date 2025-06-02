// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The visual appearance of the <see cref="FluentCard" />.
/// </summary>
public enum CardShadow
{
    /// <summary>
    /// The shadow is 4px below the card.
    /// </summary>
    [Description("default")]
    Default,

    /// <summary>
    /// The shadow is 2px below the card.
    /// </summary>
    [Description("small")]
    Small,

    /// <summary>
    /// The shadow is 8px below the card.
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// The shadow is 16px below the card.
    /// </summary>
    [Description("large")]
    Large,
}
