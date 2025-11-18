// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A divider for use within a <see cref="FluentNavDrawer"/>
/// </summary>
public partial class FluentNavDivider : INavDrawerItem
{
    /// <summary>
    /// Gets or sets the parent <see cref="FluentNavDrawer"/> component for this instance.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a <see cref="FluentNavDrawer"/>. It enables the component to access shared state or functionality from
    /// its parent navigation menu.</remarks>
    [CascadingParameter]
    public required FluentNavDrawer Owner { get; set; }

    /// <summary>
    /// Validates that this component is used within a FluentNavDrawer.
    /// </summary>
    protected override void OnParametersSet()
    {
        // Validate that this component is used within a FluentNavDrawer
        if (Owner == null)
        {
            throw new InvalidOperationException(
                $"{nameof(FluentNavDivider)} must be used as a child of {nameof(FluentNavDrawer)}.");
        }
    }
}
