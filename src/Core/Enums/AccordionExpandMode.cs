// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The expand mode of the <see cref="FluentAccordion"/> component.
/// </summary>
public enum AccordionExpandMode
{
    /// <summary>
    /// The accordion only allows a single expanded item at a time.
    /// </summary>
    Single,

    /// <summary>
    /// The accordion allows multiple items expanded at a time.
    /// </summary>
    Multi,
}
