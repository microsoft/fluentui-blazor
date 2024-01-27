using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentTabs : FluentComponentBase
{
    private const string FLUENT_TAB_TAG = "fluent-tab";
    private readonly Dictionary<string, FluentTab> _tabs = [];
    //private string _activeId = string.Empty;
    private DotNetObjectReference<FluentTabs>? _dotNetHelper = null;
    private IJSObjectReference _jsModuleOverflow = default!;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    /// <summary />
    protected string? StyleValues => new StyleBuilder(Style)
        .AddStyle("padding", "6px", () => Size == TabSize.Small)
        .AddStyle("padding", "12px 10px", () => Size == TabSize.Small)
        .AddStyle("padding", "16px 10px", () => Size == TabSize.Small)
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, () => !string.IsNullOrEmpty(Height))
        .Build();

    /// <summary />
    protected string? StyleMoreValues => new StyleBuilder()
        .AddStyle("min-width: 32px")
        .AddStyle("max-width: 32px")
        .AddStyle("cursor: pointer")
        .AddStyle("display", "none", () => !TabsOverflow.Any())
        .Build();

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the tab's orentation. See <see cref="AspNetCore.Components.Orientation"/>
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Raised when a tab is selected.
    /// </summary>
    [Parameter]
    public EventCallback<FluentTab> OnTabSelect { get; set; }

    /// <summary>
    /// Raised when a tab is closed.
    /// </summary>
    [Parameter]
    public EventCallback<FluentTab> OnTabClose { get; set; }

    /// <summary>
    /// Determines if a dismiss icon is shown.
    /// When clicked the <see cref="OnTabClose"/> event is raised to remove this tab from the list.
    /// </summary>
    [Parameter]
    public bool ShowClose { get; set; } = false;

    /// <summary>
    /// Gets or sets the width of the tab items.
    /// </summary>
    [Parameter]
    public TabSize? Size { get; set; } = TabSize.Medium;

    /// <summary>
    /// Gets or sets the width of the tabs component.
    /// Needs to be a valid CSS value (e.g. 100px, 50%).
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the tabs component.
    /// Needs to be a valid CSS value (e.g. 100px, 50%).
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets the active selected tab.
    /// </summary>
    public FluentTab ActiveTab => _tabs.FirstOrDefault(i => i.Key == ActiveTabId).Value ?? _tabs.First().Value;

    [Parameter]
    public string ActiveTabId { get; set; } = default!;

    /// <summary>
    /// Gets or sets a callback when the bound value is changed.
    /// </summary>
    [Parameter]
    public EventCallback<string> ActiveTabIdChanged { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the active indicator is displayed.
    /// </summary>
    [Parameter]
    public bool ShowActiveIndicator { get; set; } = true;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a callback when a tab is changed.
    /// </summary>
    [Parameter]
    public EventCallback<FluentTab> OnTabChange { get; set; }

    /// <summary>
    /// Gets the unique identifier associated to the more button ([Id]-more).
    /// </summary>
    public string IdMoreButton => $"{Id}-more";

    /// <summary>
    /// Gets all tabs with <see cref="FluentTab.Overflow"/> assigned to True.
    /// </summary>
    public IEnumerable<FluentTab> TabsOverflow => _tabs.Where(i => i.Value.Overflow == true).Select(v => v.Value);

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TabChangeEventArgs))]

    public FluentTabs()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {

            _dotNetHelper = DotNetObjectReference.Create(this);
            // Overflow
            _jsModuleOverflow = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Overflow/FluentOverflow.razor.js");

            var horizontal = Orientation == Orientation.Horizontal;
            await _jsModuleOverflow.InvokeVoidAsync("FluentOverflowInitialize", _dotNetHelper, Id, horizontal, FLUENT_TAB_TAG);
        }
    }

    private async Task HandleOnTabChangedAsync(TabChangeEventArgs args)
    {
        var tabId = args?.ActiveId;
        if (tabId is not null && _tabs.TryGetValue(tabId, out FluentTab? tab))
        {
            await OnTabChange.InvokeAsync(tab);
            ActiveTabId = tabId;
            await ActiveTabIdChanged.InvokeAsync(tabId);
        }

    }

    internal int RegisterTab(FluentTab tab)
    {
        _tabs.Add(tab.Id!, tab);
        return _tabs.Count - 1;
    }

    internal async Task UnregisterTabAsync(FluentTab tab)
    {
        if (OnTabClose.HasDelegate)
        {
            await OnTabClose.InvokeAsync(tab);
        }

        if (_tabs.Count > 0)
        {
            _tabs.Remove(tab.Id!);
        }

        // Set the first tab active
        FluentTab? firstTab = _tabs.FirstOrDefault().Value;
        if (firstTab is not null)
        {
            await ResizeTabsForOverflowButtonAsync();

            await OnTabChangeHandlerAsync(new TabChangeEventArgs()
            {
                ActiveId = firstTab.Id,
            });
        }
    }

    /// <summary />
    internal async Task OnTabChangeHandlerAsync(TabChangeEventArgs e)
    {
        ActiveTabId = e.ActiveId!;

        if (ActiveTabIdChanged.HasDelegate)
        {
            await ActiveTabIdChanged.InvokeAsync(ActiveTabId);
        }

        if (OnTabSelect.HasDelegate)
        {
            await OnTabSelect.InvokeAsync(ActiveTab);
        }
    }

    /// <summary />
    [JSInvokable]
    public async Task OverflowRaisedAsync(string value)
    {
        var items = JsonSerializer.Deserialize<OverflowItem[]>(value);

        if (items == null)
        {
            return;
        }

        // Update Item components
        foreach (OverflowItem item in items)
        {
            FluentTab? tab = _tabs.FirstOrDefault(i => i.Value.Id == item.Id).Value;
            tab?.SetProperties(item.Overflow);
        }

        // Raise event
        await InvokeAsync(() => StateHasChanged());
    }

    /// <summary />
    private async Task ResizeTabsForOverflowButtonAsync()
    {
        var horizontal = Orientation == Orientation.Horizontal;
        await _jsModuleOverflow.InvokeVoidAsync("FluentOverflowResized", _dotNetHelper, Id, horizontal, FLUENT_TAB_TAG);
    }

    /// <summary />
    private async Task DisplayMoreTabAsync(FluentTab tab)
    {
        await OnTabChangeHandlerAsync(new TabChangeEventArgs
        {
            ActiveId = tab.Id,
        });
    }

    /// <summary>
    /// Go to a specific tab by specifying an id
    /// </summary>
    /// <param name="TabId">Id of the tab to goto</param>
    /// <returns></returns>
    public async Task GoToTabAsync(string TabId)
    {
        await OnTabChangeHandlerAsync(new TabChangeEventArgs()
        {
            ActiveId = TabId,
        });

    }
}
