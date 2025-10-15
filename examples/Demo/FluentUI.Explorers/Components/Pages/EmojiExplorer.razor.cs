// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Explorers.Components.Pages;

public partial class EmojiExplorer : ExplorerBase
{
    private EmojiInfo[] EmojisFound = Array.Empty<EmojiInfo>();
    private readonly EmojiSearchCriteria Criteria = new();

    protected override async Task StartSearchAsync()
    {
        SearchInProgress = true;
        StateHasChanged();

        await Task.Delay(10);   // To display the Spinner

        EmojisFound =
        [
            .. EmojiExtensions
                    .AllEmojis
                    .Where(i => i.Style == Criteria.Style
                             && (Criteria.Group == GetEmptyEnum<EmojiGroup>() ? true : i.Group == Criteria.Group)
                             && (Criteria.Size == GetEmptyEnum<EmojiSize>() ? true : i.Size == Criteria.Size)
                             && (Criteria.Skintone == GetEmptyEnum<EmojiSkintone>() ? true : i.Skintone == Criteria.Skintone)
                             && (string.IsNullOrWhiteSpace(Criteria.SearchTerm) ||
                                i.Name.Contains(Criteria.SearchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                                i.KeyWords.Contains(Criteria.SearchTerm, StringComparison.InvariantCultureIgnoreCase))                             )
                    .OrderBy(i => i.Name)
        ];

        SearchInProgress = false;
        StateHasChanged();
    }

    private class EmojiSearchCriteria
    {
        public string SearchTerm { get; set; } = string.Empty;
        public EmojiStyle Style { get; set; }
        public EmojiGroup Group { get; set; } = GetEmptyEnum<EmojiGroup>();
        public EmojiSkintone Skintone { get; set; } = GetEmptyEnum<EmojiSkintone>();
        public EmojiSize Size { get; set; } = GetEmptyEnum<EmojiSize>();
    }
}
