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
/// <item><description><see cref="FluentNavItem"/> - A simple navigation item. When used as a sub item in a <see cref="FluentNavCategory"/>, no icon will be rendered</description></item>
/// <item><description><see cref="FluentNavCategory"/> - A grouped set of navigation items</description></item>
/// <item><description><see cref="FluentNavSectionHeader"/> - A section header to organize navigation</description></item>
/// <item><description><see cref="FluentDivider"/> - A visual divider between sections</description></item>
/// </list>
/// </remarks>
public partial class FluentNav : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "Nav/FluentNav.razor.js";
    internal bool _navOpen = true;
    private readonly List<FluentNavCategory> _categories = [];
    private readonly List<FluentNavBase> _items = [];
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
        .AddStyle("width", Width)
        .AddStyle("min-width", Width)
        .AddStyle("--nav-bg-color", BackgroundColor)
        .AddStyle("--nav-bg-color-hover", BackgroundColorHover)
        .Build();

    /// <summary />
    [Inject]
    internal NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Gets or sets the parent layout component.
    /// </summary>
    [CascadingParameter]
    private FluentLayout? LayoutContainer { get; set; }

    /// <summary>
    /// Gets or sets whether to enable using icons in the nav items.
    /// </summary>
    [Parameter]
    public bool UseIcons { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to allow just one expanded category or multiple
    /// </summary>
    [Parameter]
    public bool UseSingleExpanded { get; set; }

    /// <summary>
    /// Gets or sets the density of the nav menu item.
    /// </summary>
    [Parameter]
    public NavDensity? Density { get; set; }

    /// <summary>
    /// Gets or sets the content of the nav menu item.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    /// <summary>
    /// Gets or sets the background color of the component.
    /// The default value is "var(--colorNeutralBackground4)".
    /// </summary>
    [Parameter]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the background color of the component when hovered.
    /// The default value is "var(--colorNeutralBackground4Hover)".
    /// </summary>
    [Parameter]
    public string? BackgroundColorHover { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a nav item is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<FluentNavItem> OnItemClick { get; set; }

    /// <summary>
    /// Gets or sets the CSS width value to apply to the component.
    /// Default value is 260px.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

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
            // Set background color for hamburgers in the layout
            if (LayoutContainer is not null && !string.IsNullOrEmpty(BackgroundColor))
            {
                foreach (var hamburger in LayoutContainer?.Hamburgers ?? [])
                {
                    hamburger.SetBackGroundColor(BackgroundColor);
                }
            }

            await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);
            await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Nav.Initialize", Id);
        }
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;

        if (JSModule.ObjectReference is not null)
        {
            await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Nav.Dispose", Id);
        }

        await base.DisposeAsync();
        GC.SuppressFinalize(this);
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
                // Fire-and-forget: start collapse animations for other categories without waiting
                var expandedCategories = _categories.Where(c => c != category && c.Expanded).ToList();
                foreach (var otherCategory in expandedCategories)
                {
                    otherCategory.CollapseWithoutAwait();
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

    /// <summary />
    internal FluentLayoutHamburger[] OpenedHamburgers => LayoutContainer?.Hamburgers.Where(i => i.IsOpened).ToArray() ?? [];

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
    /// Registers an item with this nav component.
    /// </summary>
    internal void Register(FluentNavBase item)
    {
        if (!_items.Contains(item))
        {
            _items.Add(item);
        }
    }

    /// <summary>
    /// Unregisters an item from this nav component.
    /// </summary>
    internal void Unregister(FluentNavBase item)
    {
        _items.Remove(item);
    }

    private void OnLocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs args)
    {
        foreach (var item in _items)
        {
            item.UpdateActiveState(args.Location);
        }
    }

    /// <summary>
    /// Gets all registered categories.
    /// </summary>
    internal IEnumerable<FluentNavCategory> GetCategories()
    {
        return _categories;
    }
}
