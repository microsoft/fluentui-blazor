// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.Client.Layout;

public partial class DemoNavMenu
{

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
}
