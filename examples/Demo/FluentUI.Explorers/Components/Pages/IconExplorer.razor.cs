// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Explorers.Components.Pages;

public partial class IconExplorer : ExplorerBase
{
    private IconInfo[] IconsFound = Array.Empty<IconInfo>();
    private readonly IconSearchCriteria Criteria = new();

    protected override async Task StartSearchAsync()
    {
        SearchInProgress = true;
        StateHasChanged();

        await Task.Delay(10);   // To display the Spinner

        IconsFound =
        [
            .. IconsExtensions
                    .AllIcons
                    .Where(i => i.Variant == Criteria.Variant
                             && i.Size == Criteria.Size
                             && (string.IsNullOrWhiteSpace(Criteria.SearchTerm) || i.Name.Contains(Criteria.SearchTerm, StringComparison.InvariantCultureIgnoreCase)))
                    .OrderBy(i => i.Name)
        ];

        SearchInProgress = false;
        StateHasChanged();
    }

    private string NumberOfIconsFound
    {
        get
        {
            if (IconsFound.Length == 0)
            {
                return "no icons found.";
            }

            if (IconsFound.Length <= MaximumOfItems)
            {
                return $"{IconsFound.Length} icons found.";
            }

            return $"{Math.Min(MaximumOfItems, IconsFound.Length)} / {IconsFound.Length} icons found.";
        }
    }

    private class IconSearchCriteria
    {
        public string SearchTerm { get; set; } = string.Empty;
        public IconVariant Variant { get; set; } = IconVariant.Regular;
        public IconSize Size { get; set; } = IconSize.Size20;
        public Color Color { get; set; } = Color.Default;
    }
}
