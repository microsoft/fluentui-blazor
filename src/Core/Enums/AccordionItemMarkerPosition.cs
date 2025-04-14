// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The position of the expanded/collapsed marker
/// </summary>
public enum AccordionItemMarkerPosition
{
    /// <summary>
    /// Start
    /// </summary>
    [Description("start")]
    Start,

    /// <summary>
    /// End
    /// </summary>
    [Description("end")]
    End,
}
