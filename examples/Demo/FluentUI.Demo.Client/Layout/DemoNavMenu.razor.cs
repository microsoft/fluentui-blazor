// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Client.Layout;

public partial class DemoNavMenu
{
    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    [Parameter]
    public FluentLayoutHamburger? Hamburger { get; set; }

    protected override void OnInitialized()
    {
        var pages = DocViewerService.Pages.Where(i => !i.Hidden);

        var navItems = pages
                .GroupBy(p => p.Category.Title)
                .Select(g => new NavItem(
                    Title: string.IsNullOrEmpty(g.Key) ? "Components" : g.Key,
                    Route: string.Empty,
                    Icon: string.Empty, // Assuming categories don't have icons
                    Order: string.IsNullOrEmpty(g.Key) ? "0099" : "0000",
                    Items: g.Select(p => new NavItem(
                        Title: p.Title,
                        Route: p.Route,
                        Icon: p.Icon,
                        Order: p.Order,
                        Items: Enumerable.Empty<NavItem>())
                    ).OrderBy(i => i.Order)))
                .OrderBy(i => i.Order);

        NavItems = navItems;
    }

    public IEnumerable<NavItem> NavItems { get; private set; } = Enumerable.Empty<NavItem>();

    public record NavItem(string Title, string Route, string Icon, string Order, IEnumerable<NavItem> Items);

    private async Task ItemClickAsync(NavItem item)
    {
        NavigationManager.NavigateTo(item.Route);

        if (Hamburger is not null)
        {
            await Hamburger.HideAsync();
        }
    }
}
