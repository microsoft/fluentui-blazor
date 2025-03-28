// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The appearance of select list.
/// </summary>
public enum ListAppearance
{
    /// <summary>
    /// The list is filled lighter.
    /// </summary>
    [Description("filled-lighter")]
    FilledLighter,

    /// <summary>
    /// The list is filled darker.
    /// </summary>
    [Description("filled-darker")]
    FilledDarker,

    /// <summary>
    /// The list is outlined.
    /// </summary>
    [Description("outline")]
    Outline,

    /// <summary>
    /// The list is transparent.
    /// </summary>
    [Description("transparent")]
    Transparent,
}
