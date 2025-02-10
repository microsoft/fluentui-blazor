// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The visual appearance of the <see cref="FluentCheckbox" />.
/// </summary>
public enum CheckboxShape
{
    /// <summary>
    /// The default appearance. The border is square.
    /// </summary>
    [Description("square")]
    Square,

    /// <summary>
    /// The appearance where the border is circular.
    /// </summary>
    [Description("circular")]
    Circular,
}
