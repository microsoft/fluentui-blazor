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
public partial class FluentNavCategory : FluentComponentBase, INavItem, IDisposable
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "Nav/FluentNav.razor.js";
    private bool _isActive;
    private readonly List<FluentNavSubItem> _subitems = [];
    private bool _hasBeenManuallyCollapsed;

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
    /// Called after the component has been initialized.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Owner.RegisterCategory(this);
    }

    /// <summary>
    /// Disposes of the component and unregisters from the owner.
    /// </summary>
    public void Dispose()
    {
        Owner.UnregisterCategory(this);
    }

    /// <summary>
    /// Called after the component has been rendered.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);

            // Ensure category is expanded when any subitem is active on initial load
            if (HasActiveSubitem() && !Expanded && !_hasBeenManuallyCollapsed)
            {
                Expanded = true;
                StateHasChanged();
            }
        }
    }

    internal async Task SetExpandedAsync(bool expanded)
    {
        if (Expanded == expanded)
            return;

        Expanded = expanded;
        
        // Clear manual collapse flag when programmatically setting expanded state
        // This allows programmatic control to override user's manual collapse
        if (!expanded)
        {
            _hasBeenManuallyCollapsed = false;
        }
        
        UpdateActiveState();
        StateHasChanged();

        // Animate after state change
        await AnimateCurrentStateAsync();
    }

    /// <summary>
    /// Toggles the expanded state of the nav group.
    /// </summary>
    internal async Task ToggleExpandedAsync()
    {
        // Toggle the state
        Expanded = !Expanded;

        // Track manual collapse to prevent auto-re-expansion
        if (!Expanded && HasActiveSubitem())
        {
            _hasBeenManuallyCollapsed = true;
        }
        else if (Expanded)
        {
            // Clear the manual collapse flag when manually expanding
            _hasBeenManuallyCollapsed = false;
            
            // If single expand mode, collapse other categories
            if (Owner.UseSingleExpanded)
            {
                foreach (var category in Owner.GetCategories())
                {
                    if (category != this && category.Expanded)
                    {
                        category.SetExpanded(false);
                    }
                }
            }
        }

        UpdateActiveState();
        StateHasChanged();

        // After Blazor re-renders with new state, animate the result
        await AnimateCurrentStateAsync();
    }

    /// <summary>
    /// Called by subitems to notify the category when their active state changes.
    /// Updates the category's active state and auto-expands if a subitem becomes active.
    /// </summary>
    internal void OnSubitemActiveStateChanged()
    {
        var hasActiveSubitem = HasActiveSubitem();

        // Always ensure the category is expanded when any subitem is active
        if (hasActiveSubitem && !Expanded && !_hasBeenManuallyCollapsed)
        {
            Expanded = true;
            UpdateActiveState();
            StateHasChanged();
            _ = AnimateCurrentStateAsync();
        }
        else
        {
            UpdateActiveState();
            StateHasChanged();
        }
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
    /// Gets the effective expanded state - returns the manual expansion state.
    /// </summary>
    /// <returns>True if the category should be considered expanded; otherwise, false.</returns>
    internal bool IsEffectivelyExpanded()
    {
        return Expanded;
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
    /// Animates the category group to match the current Expanded state.
    /// Pure animation - no state changes.
    /// </summary>
    private async Task AnimateCurrentStateAsync()
    {
        try
        {
            var groupId = $"{Id}-group";
            if (Expanded)
            {
                await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Nav.AnimateExpand", groupId);
            }
            else
            {
                await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Nav.AnimateCollapse", groupId);
            }
        }
        catch
        {
            // JS might not be loaded yet
        }
    }

    /// <summary>
    /// Updates the active state based on whether any subitem is active and the category is collapsed.
    /// </summary>
    private void UpdateActiveState()
    {
        // Only show active state when category is NOT expanded and has an active subitem
        _isActive = !Expanded && HasActiveSubitem();
    }
}
