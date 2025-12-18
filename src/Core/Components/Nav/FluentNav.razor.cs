// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a navigation menu component that displays a hamburger icon and provides navigation functionality within
/// a Fluent UI application.
/// </summary>
/// <remarks>
/// <para>Use the FluentNav to present a collapsible navigation menu, typically accessed via a hamburger icon.
/// The component supports customization of the displayed icon and its tooltip text and is designed for integration
/// into Fluent UI layouts.</para>
///
/// <para><strong>Allowed Child Components:</strong></para>
/// <list type="bullet">
/// <item><description><see cref="FluentNavItem"/> - A simple navigation item</description></item>
/// <item><description><see cref="FluentNavCategory"/> - A grouped set of navigation items</description></item>
/// <item><description><see cref="FluentNavSectionHeader"/> - A section header to organize navigation</description></item>
/// <item><description><see cref="FluentNavDivider"/> - A visual divider between sections</description></item>
/// </list>
///
/// <para><strong>Nested Navigation:</strong></para>
/// <para><see cref="FluentNavSubItem"/> components can only be used as direct children of <see cref="FluentNavCategory"/>,
/// not directly in the navigation.</para>
/// </remarks>
public partial class FluentNav : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "Nav/FluentNav.razor.js";
    internal bool _navOpen = true;
    private readonly List<FluentNavCategory> _categories = [];
    private bool _previousUseSingleExpanded;

    /// <summary />
    public FluentNav(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
        _previousUseSingleExpanded = UseSingleExpanded;
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-nav")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the icon to display besides the app title.
    /// </summary>
    [Parameter]
    public Icon? AppIcon { get; set; }

    /// <summary>
    /// Gets or sets the title to display at the top of the menu .
    /// </summary>
    [Parameter]
    public string? AppTitle { get; set; }

    /// <summary>
    /// Gets or sets the link to use for the app item.
    /// Defaults to homepage ("/").
    /// </summary>
    [Parameter]
    public string AppLink { get; set; } = "/";

    /// <summary>
    /// Gets or sets whether to use the header with the hamburger icon.
    /// Defaults to false until we have a good way to make this work with the LayoutHamburger component.
    /// </summary>
    [Parameter]
    public bool UseHeader { get; set; } = false;

    /// <summary>
    /// Gets or sets the icon to display for collapsing/expanding the nav menu.
    /// By default, this icon is a hamburger icon.
    /// </summary>
    [Parameter]
    public Icon ToggleIcon { get; set; } = new CoreIcons.Regular.Size20.LineHorizontal3();

    /// <summary>
    /// Gets or sets wether to enable using icons in the nav items.
    /// </summary>
    [Parameter]
    public bool UseIcons { get; set; } = true;

    /// <summary>
    /// Gets or sets wether to allow just one expanded category or multiple
    /// </summary>
    [Parameter]
    public bool UseSingleExpanded { get; set; } = true;

    /// <summary>
    /// Gets or sets the density of the nav menu item.
    /// </summary>
    [Parameter]
    public NavDrawerDensity? Density { get; set; }

    /// <summary>
    /// Gets or sets the content of the nav menu item.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Event callback invoked when the nav menu is toggled.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnToggleNav { get; set; }

    /// <summary />
    protected override async Task OnParametersSetAsync()
    {
        if (UseSingleExpanded && !_previousUseSingleExpanded)
        {
            var expandedCategories = _categories.Where(c => c.Expanded).Skip(1).ToList();
            foreach (var category in expandedCategories)
            {
                await category.SetExpandedAsync(expanded: false);
            }
        }

        _previousUseSingleExpanded = UseSingleExpanded;
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);
        }
    }

    /// <summary>
    /// Toggles the nav menu open or closed.
    /// </summary>
    public async Task ToggleNavAsync()
    {
        _navOpen = !_navOpen;
        await InvokeAsync(StateHasChanged);

        // Animate the transition
        if (JSModule.Imported)
        {
            if (_navOpen)
            {
                await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Nav.AnimateNavOpen", Id);
            }
            else
            {
                await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Nav.AnimateNavClose", Id);
            }
        }

        if (OnToggleNav.HasDelegate)
        {
            await OnToggleNav.InvokeAsync(_navOpen);
        }
    }

    /// <summary>
    /// Expands a specific category by its ID.
    /// </summary>
    /// <param name="categoryId">The ID of the category to expand.</param>
    public async Task ExpandCategoryAsync(string categoryId)
    {
        var category = _categories.FirstOrDefault(c => string.Equals(c.Id, categoryId, StringComparison.OrdinalIgnoreCase));
        if (category != null)
        {
            if (UseSingleExpanded)
            {
                var expandedCategories = _categories.Where(c => c != category && c.Expanded).ToList();
                foreach (var otherCategory in expandedCategories)
                {
                    await otherCategory.SetExpandedAsync(expanded: false);
                }
            }

            await category.SetExpandedAsync(expanded: true);
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Collapses a specific category by its ID.
    /// </summary>
    /// <param name="categoryId">The ID of the category to collapse.</param>
    public async Task CollapseCategoryAsync(string categoryId)
    {
        var category = _categories.FirstOrDefault(c => string.Equals(c.Id, categoryId, StringComparison.OrdinalIgnoreCase));
        if (category != null)
        {
            await category.SetExpandedAsync(expanded: false);
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Collapses all categories in the navigation menu.
    /// </summary>
    public async Task CollapseAllCategoriesAsync()
    {
        // Update all category states and animate
        foreach (var category in _categories)
        {
            await category.SetExpandedAsync(expanded: false);
        }
    }

    /// <summary>
    /// Expands all categories in the navigation menu.
    /// When <see cref="UseSingleExpanded" /> is true, only the first category will be expanded.
    /// </summary>
    public async Task ExpandAllCategoriesAsync()
    {
        foreach (var category in _categories)
        {
            await category.SetExpandedAsync(expanded: true);
            if (UseSingleExpanded)
            {
                break; // Only expand the first category if single expanded is enabled
            }
        }
    }

    /// <summary>
    /// Registers a category with this nav component.
    /// </summary>
    internal void RegisterCategory(FluentNavCategory category)
    {
        if (!_categories.Contains(category))
        {
            _categories.Add(category);
        }
    }

    /// <summary>
    /// Unregisters a category from this nav component.
    /// </summary>
    internal void UnregisterCategory(FluentNavCategory category)
    {
        _categories.Remove(category);
    }

    /// <summary>
    /// Gets all registered categories.
    /// </summary>
    internal IEnumerable<FluentNavCategory> GetCategories()
    {
        return _categories;
    }
}
