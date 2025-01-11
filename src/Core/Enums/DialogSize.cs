// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the size of a dialog or a panel.
/// </summary>
public enum DialogSize
{
    /// <summary>
    /// The width of the dialog or panel is 320px.
    /// </summary>
    [Description("small")]
    Small,

    /// <summary>
    /// The width of the dialog or panel is 592px.
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// The width of the dialog or panel is 940px.
    /// </summary>
    [Description("large")]
    Large,

    /// <summary>
    /// The width of the dialog or panel is 100%.
    /// </summary>
    [Description("full")]
    Full,
}
