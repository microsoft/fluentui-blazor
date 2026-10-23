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
    private string? _clientActiveTabId;

    /// <summary>
    /// When a programmatic selection requests a value the render tree already
    /// holds, the diff would emit nothing even though the client may have
    /// changed the actual DOM selection meanwhile (client-originated selections
    /// are deliberately not written back to the <c>activeid</c> attribute to
    /// avoid feedback loops, see #5468). In that case <see cref="OnParametersSet"/>
    /// first writes the value the client currently displays (stored in
    /// <see cref="_pendingRewriteBase"/>) and stores the requested value here so
    /// <see cref="OnAfterRenderAsync"/> can reapply it: the web component then
    /// processes a regular old-to-new <c>activeid</c> change, which is what
    /// updates the previously selected tab and its panel.
    /// </summary>
    private string? _pendingAttributeRewrite;

    /// <summary>
    /// The client-displayed value written as the first phase of a forced
    /// <c>activeid</c> rewrite (see <see cref="_pendingAttributeRewrite"/>).
    /// The second phase only runs while the attribute still holds this value.
    /// </summary>
    private string? _pendingRewriteBase;
    private bool _overflowInitialized;
    private bool? _previousOverflowValue;
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
    internal string TabListId => $"{Id}-tablist";

    /// <summary />
    internal string IdMoreButton => $"{Id}-more";

    /// <summary />
    private string? ActiveTabIdAttribute { get; set; }

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
    public IReadOnlyList<FluentTab> OverflowTabs { get; private set; } = [];

    /// <summary>
    /// Gets the number of tabs that are currently displayed in the overflow menu.
    /// </summary>
    public int OverflowCount => OverflowTabs.Count;

    /// <summary />
    protected override void OnParametersSet()
    {
        var activeTabId = ActiveTabId ?? ActiveTab?.Id;

        // A forced `activeid` rewrite is in flight (see _pendingAttributeRewrite):
        // keep it pointed at the latest requested value and let the two-phase
        // sequence complete undisturbed.
        if (_pendingAttributeRewrite is not null)
        {
            if (string.Equals(activeTabId, _clientActiveTabId, StringComparison.Ordinal))
            {
                // The latest request matches the tab the client already shows
                // (e.g. the client selection changed again while the rewrite
                // was in flight): cancel the rewrite and restore the attribute
                // instead of reapplying the now-stale value afterwards.
                _pendingAttributeRewrite = null;
                _pendingRewriteBase = null;
                ActiveTabIdAttribute = activeTabId;
            }
            else
            {
                _pendingAttributeRewrite = activeTabId;
            }

            return;
        }

        if (string.Equals(activeTabId, _clientActiveTabId, StringComparison.Ordinal))
        {
            return;
        }

        var clientActiveTabId = _clientActiveTabId;
        _clientActiveTabId = null;
        _pendingAttributeRewrite = null;
        _pendingRewriteBase = null;

        // The render tree can already hold the requested value while the client
        // has since changed the actual DOM selection (see the field docs). A
        // plain assignment would then produce no diff and the programmatic
        // reselection would never reach the browser, so first write the value
        // the client currently displays and let OnAfterRenderAsync reapply the
        // requested value as a regular attribute change.
        if (clientActiveTabId is not null
            && activeTabId is not null
            && string.Equals(ActiveTabIdAttribute, activeTabId, StringComparison.Ordinal))
        {
            _pendingAttributeRewrite = activeTabId;
            _pendingRewriteBase = clientActiveTabId;
            ActiveTabIdAttribute = clientActiveTabId;
        }
        else
        {
            ActiveTabIdAttribute = activeTabId;
        }
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        // Check if the overflow mode has changed since the last render
        var overflowModeChanged = _previousOverflowValue != Overflow;

        // Dispose the overflow observer if the Overflow property is false and it was previously initialized
        if (!Overflow && _overflowInitialized)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Tabs.DisposeOverflow", TabListId);
            _overflowInitialized = false;
            OverflowTabs = [];
        }

        // Observe the tabs for changes
        if (firstRender || overflowModeChanged)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Tabs.ObserveTabsChanged", Id);
            _tabsObserverInitialized = true;
        }

        // Initialize the overflow observer
        if (Overflow && !_overflowInitialized)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Tabs.InitializeOverflow", TabListId);
            _overflowInitialized = true;
        }

        // Refresh the overflow after render if needed
        if (_overflowInitialized && _refreshOverflowAfterRender)
        {
            _refreshOverflowAfterRender = false;
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Tabs.RefreshOverflow", TabListId);
        }

        _previousOverflowValue = Overflow;

        // Complete a forced `activeid` rewrite requested by OnParametersSet:
        // the first render wrote the client-displayed value so a diff was
        // emitted; now reapply the requested value so the web component
        // processes the programmatic selection again.
        if (_pendingAttributeRewrite is not null)
        {
            var rewriteValue = _pendingAttributeRewrite;
            var rewriteBase = _pendingRewriteBase;
            _pendingAttributeRewrite = null;
            _pendingRewriteBase = null;

            // Only reapply when the attribute still holds the first-phase
            // value: a newer selection wrote the attribute meanwhile and
            // supersedes the queued rewrite.
            if (string.Equals(ActiveTabIdAttribute, rewriteBase, StringComparison.Ordinal))
            {
                ActiveTabIdAttribute = rewriteValue;
                await InvokeAsync(StateHasChanged);
            }
        }
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
                var isCurrentActive = !string.IsNullOrEmpty(ActiveTabId)
                    ? string.Equals(ActiveTabId, tab.Id, StringComparison.Ordinal)
                    : ReferenceEquals(ActiveTab, tab);
                if (isCurrentActive)
                {
                    var firstTab = Tabs.FirstOrDefault();
                    var firstTabId = firstTab?.Id;

                    ActiveTab = firstTab;
                    ActiveTabId = firstTabId;
                    ActiveTabIdAttribute = firstTabId;
                    _clientActiveTabId = null;

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
        if (await SetActiveTabAsync(tab, isClientSelection: true))
        {
            // Only record the client selection once it has been accepted. A
            // rejected event (disabled or invisible tab, disabled container)
            // must not pollute the marker, otherwise a later valid parameter
            // update for the same id would be wrongly suppressed in
            // OnParametersSet.
            _clientActiveTabId = tab?.Id;
        }
    }

    /// <summary>
    /// Selects a tab by its identifier.
    /// </summary>
    /// <param name="tabId">The identifier of the tab to select.</param>
    public async Task SelectTabAsync(string tabId)
    {
        var tab = Tabs.FirstOrDefault(t => t.Visible && !t.Disabled && string.Equals(t.Id, tabId, StringComparison.Ordinal));
        var clientActiveTabId = _clientActiveTabId;
        _clientActiveTabId = null;
        var attributeBefore = ActiveTabIdAttribute;
        if (await SetActiveTabAsync(tab, isClientSelection: false))
        {
            // Same staleness case as in OnParametersSet: the render tree may
            // already hold the requested value while the client changed the
            // actual DOM selection, so force the web component to reapply it
            // (first the client-displayed value, then the requested value in
            // OnAfterRenderAsync).
            if (clientActiveTabId is not null
                && ActiveTabIdAttribute is not null
                && string.Equals(attributeBefore, ActiveTabIdAttribute, StringComparison.Ordinal))
            {
                _pendingAttributeRewrite = ActiveTabIdAttribute;
                _pendingRewriteBase = clientActiveTabId;
                ActiveTabIdAttribute = clientActiveTabId;
            }

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
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Tabs.RefreshOverflow", TabListId);
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
            await JSRuntime.InvokeFluentVoidAsync("Microsoft.FluentUI.Blazor.Components.Tabs.DisposeOverflow", TabListId);
            _overflowInitialized = false;
        }

        if (_tabsObserverInitialized)
        {
            await JSRuntime.InvokeFluentVoidAsync("Microsoft.FluentUI.Blazor.Components.Tabs.Dispose", Id);
            _tabsObserverInitialized = false;
        }

        await base.DisposeAsync();
    }

    /// <summary>
    /// Sets the specified tab as the active tab.
    /// </summary>
    private async Task<bool> SetActiveTabAsync(FluentTab? tab, bool isClientSelection = false)
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

        // Only a programmatic selection writes the `activeid` attribute back to
        // the client. For a client-originated change the web component already
        // displays the selected tab; round-tripping the value through the
        // rendered attribute makes the component raise `tabchange` again, and a
        // stale event (e.g. raised while a programmatic value was still being
        // applied) can then flip the selection back and forth (#5468).
        if (!isClientSelection)
        {
            _clientActiveTabId = null;
            ActiveTabIdAttribute = tab.Id;
        }

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

    /// <summary>
    /// Handles the event when the overflow state of the tabs changes.
    /// </summary>
    /// <param name="args"></param>
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
            .Where(item => !string.IsNullOrEmpty(item.Id))
            .Select(item => item.Id!)
            .ToHashSet(StringComparer.Ordinal) ?? new HashSet<string>(StringComparer.Ordinal);

        var overflowTabs = Tabs
            .OrderBy(tab => tab.Index)
            .Where(tab => tab.Id is not null && overflowTabIds.Contains(tab.Id))
            .ToArray();

        if (OverflowTabs.SequenceEqual(overflowTabs))
        {
            return;
        }

        OverflowTabs = overflowTabs;
    }
}
