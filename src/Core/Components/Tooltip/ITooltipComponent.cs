// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for components that support a Tooltip parameter.
/// </summary>
internal interface ITooltipComponent
{
    /// <summary>
    /// Gets or sets the tooltip text (supports HTML tags).
    /// This parameter requires the presence of a 'FluentTooltipProvider' in your main/layout page.
    /// </summary>
    /// <remarks>
    /// This parameter cannot be updated after the component has been rendered.
    /// </remarks>
    string? Tooltip { get; set; }
}
