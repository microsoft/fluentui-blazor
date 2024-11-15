// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The size of label.
/// </summary>
public enum LabelSize
{
    /// <summary>
    /// Small label.
    /// </summary>
    [Description("small")]
    Small,

    /// <summary>
    /// Medium label.
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// Large label.
    /// </summary>
    [Description("large")]
    Large,
}
