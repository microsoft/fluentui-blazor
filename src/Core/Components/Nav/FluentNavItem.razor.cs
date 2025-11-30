// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a navigation menu item that renders content within a Fluent UI styled navigation link.
/// </summary>
public partial class FluentNavItem : FluentComponentBase, INavDrawerItem
{
    /// <summary />
    public FluentNavItem(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-navitem")
        .AddClass("active", Active)
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the icon of the nav menu item.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Get or sets the href of the link.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Get or sets wether the link is active
    /// </summary>
    [Parameter]
    public bool Active { get; set; }

    /// <summary>
    /// Gets or sets the content of the nav menu item.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

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
    protected override void OnParametersSet()
    {
        // Validate that this component is used within a FluentNav
        if (Owner == null)
        {
            throw new InvalidOperationException(
                $"{nameof(FluentNavItem)} must be used as a child of {nameof(FluentNav)}.");
        }
    }
}
