using Microsoft.Fast.Components.FluentUI;

namespace FluentUI.Demo.Shared.Pages.Icon;

public partial class IconExplorer
{
    private bool SearchInProgress = true;
    private string SearchTerm = string.Empty;
    private IEnumerable<IconInfo> IconsFound = Array.Empty<IconInfo>();

    private async Task HandleSearchField(string searchTerm)
    {
        SearchInProgress = true;
        await Task.Delay(1);

        IconsFound = Icons.AllIcons
                          .Where(i => i.Name.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase)
                                   && i.Variant == IconVariant.Regular
                                   && i.Size == IconSize.Size24)
                          .Take(100)
                          .ToArray();

        SearchInProgress = false;
        await Task.Delay(1);
    }
}
