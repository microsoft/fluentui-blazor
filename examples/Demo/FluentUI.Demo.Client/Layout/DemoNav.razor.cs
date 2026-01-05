// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Client.Layout;

public partial class DemoNav
{
    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    [Parameter]
    public FluentLayoutHamburger? Hamburger { get; set; }

    public IEnumerable<NavItem> NavItems { get; private set; } = [];

    protected override void OnInitialized()
    {
        var pages = DocViewerService.Pages.Where(i => !i.Hidden);

        var navItems = pages
                .OrderBy(g => g.Category.Key)
                .GroupBy(p => p.Category.Title)
                .Select(categoryGroup =>
                {
                    // Group items within each category by PageGroup
                    var subGroups = categoryGroup
                        .GroupBy(p => p.PageGroup ?? string.Empty)
                        .Select(subGroup =>
                        {
                            // If PageGroup is empty, return items directly
                            if (string.IsNullOrEmpty(subGroup.Key))
                            {
                                return subGroup.Select(p => new NavItem(
                                    Title: p.Title,
                                    Route: p.Route,
                                    Icon: p.Icon,
                                    Order: p.Order,
                                    Items: []));
                            }

                            // If PageGroup has value, create a sub-category
                            else
                            {
                                return [ new NavItem(
                                    Title: subGroup.Key,
                                    Route: subGroup.FirstOrDefault(p => p.IsDefaultPageGroup)?.Route ?? subGroup.OrderBy(p => p.Order).First().Route,
                                    Icon: subGroup.FirstOrDefault(p => p.IsDefaultPageGroup)?.Icon ?? null,
                                    Order: subGroup.First().Order,
                                    Items: subGroup.Where(p => !p.IsDefaultPageGroup)
                                                   .Select(p => new NavItem(
                                                        Title: p.Title,
                                                        Route: p.Route,
                                                        Icon: p.Icon,
                                                        Order: p.Order,
                                                        Items: []))
                                    .OrderBy(i => i.Order)
                                    .ThenBy(i => i.Title))
                                ];
                            }
                        })
                        .SelectMany(x => x)
                        .OrderBy(i => i.Order)
                        .ThenBy(i => i.Title);

                    return new NavItem(
                        Title: string.IsNullOrEmpty(categoryGroup.Key) ? "Components" : categoryGroup.Key,
                        Route: string.Empty,
                        Icon: string.Empty,
                        Order: string.IsNullOrEmpty(categoryGroup.Key) ? "0099" : "0000",
                        Items: subGroups);
                })
                .OrderBy(i => i.Order);

        NavItems = navItems;
    }

    private async Task OnNavItemClickAsync(FluentNavItem item)
    {
        if (Hamburger is not null && !string.IsNullOrEmpty(item.Href))
        {
            NavigationManager.NavigateTo(item.Href);
            await Hamburger.HideAsync();
        }
    }

    public record NavItem(string Title, string Route, string? Icon, string Order, IEnumerable<NavItem> Items);

    /// <summary>
    /// Create an <see cref="Icon"/> instance from a friendly name (e.g. "Home").
    /// Uses the icons reflection helpers already present in the library so no switch/lookup table is needed.
    /// Returns null when name is empty or the icon cannot be found.
    /// </summary>
    private static CustomIcon? GetIconFromName(string? iconName)
    {
        if (string.IsNullOrWhiteSpace(iconName))
        {
            return null;
        }

        var iconInfo = new IconInfo
        {
            Name = iconName,
            Size = IconSize.Size20,
            Variant = IconVariant.Regular
        };

        if (iconInfo.TryGetInstance(out var customIcon))
        {
            return customIcon;
        }

        return null;
    }
}
