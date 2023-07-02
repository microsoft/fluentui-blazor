using System.Linq;
using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared.Pages.Icon;

public partial class IconExplorer
{
    private const int MAX_ICONS = 200;
    private bool SearchInProgress = false;
    private SearchCriteria Criteria = new ();
    private IEnumerable<IconInfo> IconsFound = Array.Empty<IconInfo>();
    private int IconsCount = 0;

    private async Task HandleSearchField()
    {
        SearchInProgress = true;
        await Task.Delay(1);

        var icons = Icons.AllIcons
                         .Where(i => i.Name.Contains(Criteria.SearchTerm, StringComparison.InvariantCultureIgnoreCase)
                                  && i.Variant == Criteria.Variant
                                  && i.Size == Criteria.Size);

        IconsCount = icons.Count();
        IconsFound = icons.Take(MAX_ICONS).ToArray();

        SearchInProgress = false;
        await Task.Delay(1);
    }

    private class SearchCriteria
    {
        public string SearchTerm { get; set; } = string.Empty;
        public IconVariant Variant { get; set; } = IconVariant.Regular;
        public IconSize Size { get; set; } = IconSize.Size24;
    }
}
