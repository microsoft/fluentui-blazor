using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTabs : FluentComponentBase
{
    /// <summary>
    /// Gets or sets if the tab is active
    /// </summary>
    [Parameter]
    public bool? ActiveIndicator { get; set; }

    /// <summary>
    /// Gets or sets the tab's orentation. See <see cref="FluentUI.Orientation"/>
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; }

    [Parameter]
    public string? ActiveId { get; set; }

    /// <summary>
    /// Gets or sets a callback that updates the bound value.
    /// </summary>
    [Parameter]
    public EventCallback<string?> ActiveIdChanged { get; set; }

    private async Task OnTabChange(TabChangeEventArgs args)
    {
        await ActiveIdChanged.InvokeAsync(args.ActiveId);
    }
}