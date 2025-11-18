// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A container for a <see cref="FluentNavDrawer"/> subgroup
/// </summary>
public partial class FluentNavSubItem : FluentComponentBase
{

    /// <summary />
    public FluentNavSubItem(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-navsubitem")
        .AddClass("active", Active)
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

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
        // Validate that this component is used within a FluentNavCategory inside a FluentNavDrawer
        if (Owner == null)
        {
            throw new InvalidOperationException(
                $"{nameof(FluentNavSubItem)} must be used as a child of {nameof(FluentNavCategory)} within a {nameof(FluentNavDrawer)}.");
        }
    }
}
