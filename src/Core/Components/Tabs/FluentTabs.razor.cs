// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Each tab typically contains a text header and often includes an icon.
/// </summary>
public partial class FluentTabs : FluentComponentBase
{
    private bool _overflowInitialized;
    private bool? _observedOverflow;
    private IReadOnlyList<FluentTab> _overflowTabs = [];
    private bool _refreshOverflowAfterRender;
    private bool _tabsObserverInitialized;

    private List<FluentTab> Tabs { get; } = [];

    /// <summary />
    [DynamicDependency(nameof(TabChangeHandlerAsync))]
    [DynamicDependency(nameof(OverflowChangedHandler))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TabChangeEventArgs))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(OverflowChangedEventArgs))]
    public FluentTabs(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-tabs")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width, when: !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, when: !string.IsNullOrEmpty(Height))
        .Build();

    /// <summary />
    protected string? MoreButtonStyleValue => new StyleBuilder()
        .AddStyle("visibility", "hidden", when: OverflowCount == 0)
        .Build();

    /// <summary />
    protected string MoreButtonLabel => Localizer[Localization.LanguageResource.Tabs_MoreItems, OverflowCount];

    /// <summary />
    protected string OverflowMenuId => $"{Id}-overflow-menu";

    /// <summary>
    /// Gets or sets the visual appearance applied to each contained tab (e.g., <c>Appearance="TabsAppearance.Subtle"</c>).
    /// </summary>
    [Parameter]
    public TabsAppearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets whether the tabs are disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the size of the tabs. The default is medium.
    /// </summary>
    [Parameter]
    public TabsSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the tabs. The default is horizontal.
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; }

    /// <summary>
    /// Gets or sets whether tabs that do not fit in the available space are displayed in an overflow menu.
    /// </summary>
    [Parameter]
    public bool Overflow { get; set; }

    /// <summary>
    /// Gets or sets the height of the tabs.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the width of the tabs.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the ID of the currently active tab. Use <c>@bind-ActiveTabId</c> for two-way binding.
    /// See also <see cref="ActiveTab"/> to work with the <see cref="FluentTab"/> instance directly.
    /// </summary>
    [Parameter]
    public string? ActiveTabId { get; set; }

    /// <summary>
    /// Represents a callback for when the active tab id changes. It can handle a nullable FluentTab parameter.
    /// </summary>
    [Parameter]
    public EventCallback<string?> ActiveTabIdChanged { get; set; }

    /// <summary>
    /// Gets or sets the currently active <see cref="FluentTab"/> instance. Use <c>@bind-ActiveTab</c> for two-way binding.
    /// See also <see cref="ActiveTabId"/> to work with the tab ID string directly.
    /// </summary>
    [Parameter]
    public FluentTab? ActiveTab { get; set; }

    /// <summary>
    /// Represents a callback for when the active tab changes. It can handle a nullable FluentTab parameter.
    /// </summary>
    [Parameter]
    public EventCallback<FluentTab?> ActiveTabChanged { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the content rendered inside the overflow menu trigger.
    /// </summary>
    [Parameter]
    public RenderFragment<FluentTabs>? MoreTemplate { get; set; }

    /// <summary>
    /// Gets or sets the content rendered in place of the default overflow menu.
    /// </summary>
    [Parameter]
    public RenderFragment<FluentTabs>? OverflowTemplate { get; set; }

    /// <summary>
    /// Gets the tabs that are currently displayed in the overflow menu.
    /// </summary>
    public IReadOnlyList<FluentTab> OverflowTabs => _overflowTabs;

    /// <summary>
    /// Gets the number of tabs that are currently displayed in the overflow menu.
    /// </summary>
    public int OverflowCount { get; private set; }

    /// <summary>
    /// Gets the identifier of the overflow menu trigger.
    /// </summary>
    public string IdMoreButton => $"{Id}-more-button";

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var overflowModeChanged = _observedOverflow != Overflow;

        if (!Overflow && _overflowInitialized)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Overflow.Dispose", TabListId);
            _overflowInitialized = false;
            _overflowTabs = [];
            OverflowCount = 0;
        }

        if (firstRender || overflowModeChanged)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Tabs.ObserveTabsChanged", Id);
            _tabsObserverInitialized = true;
        }

        if (Overflow && !_overflowInitialized)
        {
            await JSRuntime.InvokeVoidAsync(
                "Microsoft.FluentUI.Blazor.Components.Overflow.Initialize",
                TabListId,
                "fluent-tab",
                0,
                0,
                "activeid",
                true,
                true);
            _overflowInitialized = true;
        }

        if (_overflowInitialized && _refreshOverflowAfterRender)
        {
            _refreshOverflowAfterRender = false;
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Overflow.Refresh", TabListId);
        }

        _observedOverflow = Overflow;
    }

    /// <summary />
    internal async Task<int> AddTabAsync(FluentTab? tab)
    {
        if (tab is not null && !string.IsNullOrEmpty(tab.Id))
        {
            Tabs.Add(tab);

            // Set the default ActiveTab
            if (!string.IsNullOrEmpty(ActiveTabId) && string.Equals(ActiveTabId, tab.Id, StringComparison.Ordinal))
            {
                ActiveTab = tab;

                if (ActiveTabChanged.HasDelegate)
                {
                    await ActiveTabChanged.InvokeAsync(ActiveTab);
                }
            }

            // Set the default ActiveTabId
            else if (ActiveTabId is null && string.Equals(ActiveTab?.Id, tab.Id, StringComparison.Ordinal))
            {
                ActiveTabId = tab.Id;

                if (ActiveTabIdChanged.HasDelegate)
                {
                    await ActiveTabIdChanged.InvokeAsync(ActiveTabId);
                }
            }

            await InvokeAsync(StateHasChanged);

            return Tabs.Count;
        }

        return 0;
    }

    /// <summary />
    internal async Task<int> RemoveTabAsync(FluentTab? tab)
    {
        if (tab is not null && !string.IsNullOrEmpty(tab.Id))
        {
            if (Tabs.Remove(tab))
            {
                var firstTab = Tabs.FirstOrDefault();
                var firstTabId = firstTab?.Id;

                // Set the first ActiveTab and ActiveTabId
                if (!string.Equals(firstTabId, tab.Id, StringComparison.Ordinal))
                {
                    ActiveTab = firstTab;
                    ActiveTabId = firstTabId;

                    if (ActiveTabChanged.HasDelegate)
                    {
                        await ActiveTabChanged.InvokeAsync(firstTab);
                    }

                    if (ActiveTabIdChanged.HasDelegate)
                    {
                        await ActiveTabIdChanged.InvokeAsync(firstTabId);
                    }
                }

                await InvokeAsync(StateHasChanged);

                return Tabs.Count;
            }
        }

        return 0;
    }

    /// <summary />
    internal async Task TabChangeHandlerAsync(TabChangeEventArgs args)
    {
        // Only for the current FluentTabs
        if (!string.Equals(args.Id, TabListId, StringComparison.Ordinal))
        {
            return;
        }

        // Search for the tab
        var tab = Tabs.FirstOrDefault(t => string.Equals(t.Id, args.ActiveId, StringComparison.Ordinal));
        await SetActiveTabAsync(tab);
    }

    /// <summary>
    /// Selects a tab by its identifier.
    /// </summary>
    /// <param name="tabId">The identifier of the tab to select.</param>
    public async Task SelectTabAsync(string tabId)
    {
        var tab = Tabs.FirstOrDefault(t => t.Visible && !t.Disabled && string.Equals(t.Id, tabId, StringComparison.Ordinal));
        if (await SetActiveTabAsync(tab))
        {
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Recalculates which tabs fit in the available space.
    /// </summary>
    public async Task RefreshOverflowAsync()
    {
        if (_overflowInitialized)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Overflow.Refresh", TabListId);
        }
        else if (Overflow)
        {
            _refreshOverflowAfterRender = true;
        }
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        if (_overflowInitialized)
        {
            await JSRuntime.InvokeFluentVoidAsync("Microsoft.FluentUI.Blazor.Components.Overflow.Dispose", TabListId);
            _overflowInitialized = false;
        }

        if (_tabsObserverInitialized)
        {
            await JSRuntime.InvokeFluentVoidAsync("Microsoft.FluentUI.Blazor.Components.Tabs.Dispose", Id);
            _tabsObserverInitialized = false;
        }

        await base.DisposeAsync();
    }

    /// <summary />
    internal string TabListId => $"{Id}-tablist";

    private async Task<bool> SetActiveTabAsync(FluentTab? tab)
    {
        if (tab is null || Disabled || tab.Disabled || !tab.Visible)
        {
            return false;
        }

        var activeTabIdChanged = !string.Equals(ActiveTabId, tab.Id, StringComparison.Ordinal);
        var activeTabChanged = !ReferenceEquals(ActiveTab, tab);
        if (!activeTabIdChanged && !activeTabChanged)
        {
            return false;
        }

        ActiveTabId = tab.Id;
        ActiveTab = tab;
        _refreshOverflowAfterRender = Overflow;

        if (activeTabIdChanged && ActiveTabIdChanged.HasDelegate)
        {
            await ActiveTabIdChanged.InvokeAsync(ActiveTabId);
        }

        if (activeTabChanged && ActiveTabChanged.HasDelegate)
        {
            await ActiveTabChanged.InvokeAsync(ActiveTab);
        }

        return true;
    }

    private void OverflowChangedHandler(OverflowChangedEventArgs args)
    {
        if (Overflow is false)
        {
            return;
        }

        if (!string.Equals(args.Id, TabListId, StringComparison.Ordinal))
        {
            return;
        }

        var overflowTabIds = args.Items?
            .Where(item => item.Overflow && !string.IsNullOrEmpty(item.Id))
            .Select(item => item.Id!)
            .ToHashSet(StringComparer.Ordinal) ?? [];
        var overflowTabs = Tabs
            .OrderBy(tab => tab.Index)
            .Where(tab => tab.Id is not null && overflowTabIds.Contains(tab.Id))
            .ToArray();

        if (OverflowCount == args.OverflowCount && _overflowTabs.SequenceEqual(overflowTabs))
        {
            return;
        }

        OverflowCount = args.OverflowCount;
        _overflowTabs = overflowTabs;
    }
}
