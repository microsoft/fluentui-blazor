// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the size options for an avatar.
/// </summary>
public enum AccordionItemSize
{
    /// <summary>
    /// Medium
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// Small
    /// </summary>
    [Description("small")]
    Small,

    /// <summary>
    /// Large
    /// </summary>
    [Description("large")]
    Large,

    /// <summary>
    /// Extra large
    /// </summary>
    [Description("extra-large")]
    ExtraLarge,
}
