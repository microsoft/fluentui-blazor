// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The size of select list.
/// </summary>
public enum ListSize
{
    /// <summary>
    /// Medium list.
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// Small list.
    /// </summary>
    [Description("small")]
    Small,

    /// <summary>
    /// Large list.
    /// </summary>
    [Description("large")]
    Large,
}
