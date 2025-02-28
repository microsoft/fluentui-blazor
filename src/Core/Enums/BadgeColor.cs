// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The color of the <see cref="FluentBadge" />.
/// </summary>
public enum BadgeColor
{
    /// <summary>
    /// Badge uses the brand color
    /// </summary>
    [Description("brand")]
    Brand,

    /// <summary>
    /// Badge uses the danger color
    /// </summary>
    [Description("danger")]
    Danger,

    /// <summary>
    /// Badge uses the important color
    /// </summary>
    [Description("important")]
    Important,

    /// <summary>
    /// Badge uses the informative color
    /// </summary>
    [Description("informative")]
    Informative,

    /// <summary>
    /// Badge uses the severe color
    /// </summary>
    [Description("severe")]
    Severe,

    /// <summary>
    /// Badge uses the subtle color
    /// </summary>
    [Description("subtle")]
    Subtle,

    /// <summary>
    /// Badge uses the success color
    /// </summary>
    [Description("success")]
    Success,

    /// <summary>
    /// Badge uses the warning color
    /// </summary>
    [Description("warning")]
    Warning,
}
