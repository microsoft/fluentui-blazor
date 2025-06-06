// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The (reading) direction of objects in the UI.
/// </summary>
public enum LocalizationDirection
{
    /// <summary>
    /// Left to right.
    /// </summary>
    [Description("ltr")]
    LeftToRight,

    /// <summary>
    /// Right to left.
    /// </summary>
    [Description("rtl")]
    RightToLeft,
}
