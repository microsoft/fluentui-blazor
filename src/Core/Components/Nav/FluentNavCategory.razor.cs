// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a group of related navigation items for use in a fluent navigation interface.
/// </summary>
/// <remarks>Use this class to organize navigation elements into logical groups when building fluent or
/// hierarchical navigation structures. Grouping navigation items can improve usability and clarity in user interfaces
/// that support complex navigation scenarios.</remarks>
public partial class FluentNavCategory : FluentNavBase
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "Nav/FluentNav.razor.js";
    private readonly List<FluentNavItem> _subitems = [];
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
    /// Gets or sets the icon to use when the item is not hovered/selected/active.
    /// </summary>
    [Parameter]
    public Icon? IconRest { get; set; } = new CoreIcons.Regular.Size20.Folder();

    /// <summary>
    /// Gets or sets the icon to use when the item is hovered/selected/active.
    /// </summary>
    [Parameter]
    public Icon? IconActive { get; set; }

    /// <summary>
    /// Gets or sets the expanded state of the nav menu group.
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the expanded state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

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
    /// Validates that this component is used within a FluentNav.
    /// </summary>
    protected override void OnParametersSet()
    {
        UpdateActiveState();
    }

    /// <summary>
    /// Called after the component has been initialized.
    /// </summary>
    protected override void OnInitialized()
    {
        if (Owner.GetType() != typeof(FluentNav))
        {
            throw new InvalidOperationException(
                $"{nameof(FluentNavCategory)} can only be used as a direct child of {nameof(FluentNav)}.");
        }

        Owner?.RegisterCategory(this);
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        Owner.UnregisterCategory(this);

        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }
    /// <summary>
    /// Called after the component has been rendered.
    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);

            if (HasActiveSubitem() && !Expanded && !_hasBeenManuallyCollapsed)
            {
                await UpdateExpandedStateAsync(expanded: true);
            }
        }
    }

    internal async Task SetExpandedAsync(bool expanded)
    {
        _hasBeenManuallyCollapsed = false;

        if (Expanded == expanded)
        {
            return;
        }

        await UpdateExpandedStateAsync(expanded);
    }

    /// <summary>
    /// Collapses this category without waiting for the animation to complete.
    /// Used for simultaneous collapse/expand animations when UseSingleExpanded is true.
    /// </summary>
    internal void CollapseWithoutAwait()
    {
        _hasBeenManuallyCollapsed = false;
        Expanded = false;
        UpdateActiveState();

        if (ExpandedChanged.HasDelegate)
        {
            _ = ExpandedChanged.InvokeAsync(false);
        }

        _ = InvokeAsync(StateHasChanged);

        // Fire animation without awaiting - let it run in parallel
        _ = AnimateCollapseAsync();
    }

    /// <summary>
    /// Animates the collapse and updates final state after animation completes.
    /// </summary>
    private async Task AnimateCollapseAsync()
    {
        if (JSModule.Imported)
        {
            var groupId = $"{Id}-group";
            var density = Owner.Density?.ToAttributeValue() ?? "medium";
            await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Nav.AnimateCollapse", groupId, density);
        }
    }

    /// <summary>
    /// Toggles the expanded state of the nav group.
    /// </summary>
    internal async Task ToggleExpandedAsync()
    {
        if (!Expanded && Owner.UseSingleExpanded)
        {
            foreach (var category in Owner.GetCategories().Where(c => c != this && c.Expanded))
            {
                category.CollapseWithoutAwait();
            }
        }

        _hasBeenManuallyCollapsed = Expanded && HasActiveSubitem();

        await UpdateExpandedStateAsync(!Expanded);
    }

    /// <summary>
    /// Called by subitems to notify the category when their active state changes.
    /// Updates the category's active state and auto-expands if a subitem becomes active.
    /// </summary>
    internal void OnSubitemActiveStateChanged()
    {
        if (HasActiveSubitem() && !Expanded && !_hasBeenManuallyCollapsed)
        {
            _ = InvokeAsync(async () => await UpdateExpandedStateAsync(expanded: true));
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
    /// Registers a subitem with this category.
    /// </summary>
    internal void RegisterSubitem(FluentNavItem subitem)
    {
        if (!_subitems.Contains(subitem))
        {
            _subitems.Add(subitem);
        }
    }

    /// <summary>
    /// Unregisters a subitem from this category.
    /// </summary>
    internal void UnregisterSubitem(FluentNavItem subitem)
    {
        _subitems.Remove(subitem);
    }

    private async Task UpdateExpandedStateAsync(bool expanded)
    {
        Expanded = expanded;

        if (expanded)
        {
            UpdateActiveState();
        }

        if (ExpandedChanged.HasDelegate)
        {
            await ExpandedChanged.InvokeAsync(expanded);
        }

        await AnimateCurrentStateAsync();

        if (!expanded)
        {
            UpdateActiveState();
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary />
    private async Task AnimateCurrentStateAsync()
    {
        if (JSModule.Imported)
        {
            var groupId = $"{Id}-group";
            var density = Owner.Density?.ToAttributeValue() ?? "medium";
            if (Expanded)
            {
                await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Nav.AnimateExpand", groupId, density);
            }
            else
            {
                await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Nav.AnimateCollapse", groupId, density);
            }
        }
    }

    /// <summary>
    /// Updates the active state based on whether any subitem is active and the category is collapsed.
    /// </summary>
    internal override void UpdateActiveState(string? location = null)
    {
        // Only show active state when category is NOT expanded and has an active subitem
        var shouldBeActiveNow = !Expanded && HasActiveSubitem();
        if (_isActive != shouldBeActiveNow)
        {
            _isActive = shouldBeActiveNow;
            _ = InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Gets the inline style for the subitem group to ensure visibility when it contains an active subitem.
    /// </summary>
    private string GetSubitemGroupStyle()
    {
        if (HasActiveSubitem())
        {
            return "height: auto; min-height: auto; opacity: 1; overflow: visible;";
        }

        return string.Empty;
    }
}
