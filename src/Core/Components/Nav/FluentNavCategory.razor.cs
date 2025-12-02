// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a group of related navigation items for use in a fluent navigation interface.
/// </summary>
/// <remarks>Use this class to organize navigation elements into logical groups when building fluent or
/// hierarchical navigation structures. Grouping navigation items can improve usability and clarity in user interfaces
/// that support complex navigation scenarios.</remarks>
public partial class FluentNavCategory : FluentComponentBase, INavItem
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "Nav/FluentNav.razor.js";

    /// <summary />
    public FluentNavCategory(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-navcategoryitem")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the title of the nav menu group.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the icon of the nav menu group.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; } = new CoreIcons.Regular.Size20.Folder();

    /// <summary>
    /// Gets or sets the expanded state of the nav menu group.
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; }

    /// <summary>
    /// Gets or sets the tooltip to display when the mouse is placed over the item.
    /// </summary>
    [Parameter]
    public string? Tooltip { get; set; }

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

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Import the JavaScript module
            await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);
        }
    }

    /// <summary>
    /// Validates that this component is used within a FluentNav.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        // Validate that this component is used within a FluentNav
        if (Owner == null || Owner.GetType() != typeof(FluentNav))
        {
            throw new InvalidOperationException(
                $"{nameof(FluentNavCategory)} must be used as a child of {nameof(FluentNav)}.");
        }
    }

    /// <summary>
    /// Toggles the expanded state of the nav group.
    /// </summary>
    internal async Task ToggleExpandedAsync()
    {
        Expanded = !Expanded;
        await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.NavDrawer.ToggleCategory", Id, Owner.UseSingleExpanded);
    }
}
