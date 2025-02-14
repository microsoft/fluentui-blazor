// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The appearance of the <see cref="FluentDivider" />.
/// </summary>
public enum DividerAppearance
{
    /// <summary>
    /// The divider appears with the default style.
    /// </summary>
    [Description("")]
    Default,

    /// <summary>
    /// The divider appears with the default style.
    /// </summary>
    [Description("strong")]
    Strong,

    /// <summary>
    /// The divider appears with the brand style.
    /// </summary>
    [Description("brand")]
    Brand,

    /// <summary>
    /// The divider appears with the subtle style.
    /// </summary>
    [Description("subtle")]
    Subtle,
}
