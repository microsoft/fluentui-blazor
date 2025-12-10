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
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "Nav/FluentNavCategory.razor.js";
    private bool _isActive;
    private readonly List<FluentNavSubItem> _subitems = [];

    /// <summary />
    public FluentNavCategory(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-navcategoryitem")
        .AddClass("active", _isActive)
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
            await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);
        }
    }

    /// <summary>
    /// Validates that this component is used within a FluentNav.
    /// </summary>
    protected override void OnParametersSet()
    {
        // Validate that this component is used within a FluentNav
        if (Owner == null || Owner.GetType() != typeof(FluentNav))
        {
            throw new InvalidOperationException(
                $"{nameof(FluentNavCategory)} must be used as a child of {nameof(FluentNav)}.");
        }

        UpdateActiveState();
    }

    /// <summary>
    /// Toggles the expanded state of the nav group.
    /// </summary>
    internal async Task ToggleExpandedAsync()
    {
        Expanded = !Expanded;
        await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.NavCategory.ToggleCategory", Id, Owner.UseSingleExpanded);

        UpdateActiveState();
    }

    /// <summary>
    /// Called by subitems to notify the category when their active state changes.
    /// Updates the category's active state and auto-expands if a subitem becomes active.
    /// </summary>
    internal void OnSubitemActiveStateChanged()
    {
        if (!Expanded && HasActiveSubitem())
        {
            Expanded = true;
            _ = InvokeExpandAsync();
        }

        UpdateActiveState();
    }

    /// <summary>
    /// Checks if any FluentNavSubItem is currently active.
    /// </summary>
    /// <returns>True if at least one subitem is active; otherwise, false.</returns>
    internal bool HasActiveSubitem()
    {
        return _subitems.Exists(item => item.Active);
    }

    /// <summary>
    /// Registers a subitem with this category.
    /// </summary>
    internal void RegisterSubitem(FluentNavSubItem subitem)
    {
        if (!_subitems.Contains(subitem))
        {
            _subitems.Add(subitem);
        }
    }

    /// <summary>
    /// Unregisters a subitem from this category.
    /// </summary>
    internal void UnregisterSubitem(FluentNavSubItem subitem)
    {
        _subitems.Remove(subitem);
    }

    /// <summary>
    /// Invokes the JavaScript expansion with UseSingleExpanded logic.
    /// This is fire-and-forget as it's triggered by navigation and doesn't need to block.
    /// </summary>
    private async Task InvokeExpandAsync()
    {
        await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.NavCategory.ExpandCategory", Id, Owner.UseSingleExpanded);
    }

    /// <summary>
    /// Updates the active state based on whether any subitem is active and the category is collapsed.
    /// </summary>
    private void UpdateActiveState()
    {
        // Only show active state when category is collapsed and has an active subitem
        _isActive = !Expanded && HasActiveSubitem();
    }
}
