using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTabs : FluentComponentBase
{
    private readonly Dictionary<string, FluentTab> tabs = new();

    /// <summary>
    /// Gets or sets if the active tab is marked 
    /// </summary>
    [Parameter]
    public bool ActiveIndicator { get; set; } = true;

    /// <summary>
    /// Gets or sets the tab's orentation. See <see cref="FluentUI.Orientation"/>
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; }

    [Parameter]
    public string? ActiveId { get; set; }

    /// <summary>
    /// Gets or sets a callback when the bound value is changed .
    /// </summary>
    [Parameter]
    public EventCallback<string?> ActiveIdChanged { get; set; }

    /// <summary>
    /// Gets or sets a callback when a tab is changed .
    /// </summary>
    [Parameter]
    public EventCallback<FluentTab> OnTabChange { get; set; }

    private async Task HandleOnTabChanged(TabChangeEventArgs args)
    {
        string? tabId = args.AffectedId;
        if (tabs.TryGetValue(tabId!, out FluentTab? tab))
        {
            await OnTabChange.InvokeAsync(tab);
            await ActiveIdChanged.InvokeAsync(args.ActiveId);
        }
    }

    internal void Register(FluentTab tab)
    {
        tabs.Add(tab.TabId, tab);
    }

    internal void Unregister(FluentTab tab)
    {
        tabs.Remove(tab.TabId);
    }
}