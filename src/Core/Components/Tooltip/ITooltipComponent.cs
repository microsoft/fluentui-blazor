// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for components that support a Tooltip parameter.
/// </summary>
internal interface ITooltipComponent
{
    /// <summary>
    /// Gets or sets the tooltip text (supports HTML tags).
    /// </summary>
    /// <remarks>
    /// This parameter cannot be updated after the component has been rendered.
    /// </remarks>
    string? Tooltip { get; set; }
}
