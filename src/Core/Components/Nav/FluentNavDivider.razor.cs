// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A divider for use within a <see cref="FluentNav"/>
/// </summary>
public partial class FluentNavDivider 
{
    /// <summary>
    /// Gets or sets the parent <see cref="FluentNav"/> component for this instance.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a <see cref="FluentNav"/>. It enables the component to access shared state or functionality from
    /// its parent navigation menu.</remarks>
    [CascadingParameter]
    public required FluentNav Owner { get; set; }

    /// <summary>
    /// Validates that this component is used within a FluentNav.
    /// </summary>
    protected override void OnInitialized()
    {
        // Validate that this component is used within a FluentNav
        if (Owner.GetType() != typeof(FluentNav))
        {
            throw new InvalidOperationException(
                $"{nameof(FluentNavDivider)} can only be used as a direct child of {nameof(FluentNav)}.");
        }
    }
}
