// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Explorers.Components.Pages;

public partial class EmojiExplorer : ExplorerBase
{
    private IconInfo[] EmojisFound = Array.Empty<IconInfo>();
    private readonly EmojiSearchCriteria Criteria = new();

    protected override async Task StartSearchAsync()
    {
        SearchInProgress = true;
        StateHasChanged();

        await Task.Delay(10);   // To display the Spinner

        EmojisFound =
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

    private class EmojiSearchCriteria
    {
        public string SearchTerm { get; set; } = string.Empty;
        public IconVariant Variant { get; set; } = IconVariant.Regular;
        public IconSize Size { get; set; } = IconSize.Size20;
        public Color Color { get; set; } = Color.Default;
    }
}
