using FluentUI.Demo.AssetExplorer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.AssetExplorer.Components.Pages;

public partial class EmojiExplorer
{
    private const int ITEMS_PER_PAGE = 4 * 12;

    private bool SearchInProgress = false;

    private readonly EmojiSearchCriteria Criteria = new();
    private EmojiInfo[] EmojisFound = Array.Empty<EmojiInfo>();
    private PaginationState PaginationState = new PaginationState { ItemsPerPage = ITEMS_PER_PAGE };

    [Parameter]
    public string Width { get; set; } = "95%";

    [Parameter]
    public string Height { get; set; } = "100%";

    private IEnumerable<EmojiInfo> EmojisForCurrentPage
    {
        get
        {
            return EmojisFound.Skip(PaginationState.CurrentPageIndex * PaginationState.ItemsPerPage)
                              .Take(PaginationState.ItemsPerPage);
        }
    }

    private async Task StartNewSearchAsync()
    {
        SearchInProgress = true;
        await Task.Delay(1); // Display spinner

        EmojisFound = Emojis.AllEmojis
                            .Where(i => i.Skintone == Criteria.Skintone
                                     && i.Style == Criteria.Style
                                     && (string.IsNullOrWhiteSpace(Criteria.SearchTerm) ? true : i.Name.Contains(Criteria.SearchTerm, StringComparison.InvariantCultureIgnoreCase)))
                            .OrderBy(i => i.Name)
                            .ToArray();

        await PaginationState.SetTotalItemCountAsync(EmojisFound.Length);

        SearchInProgress = false;
    }

    private void HandleCurrentPageIndexChanged()
    {
        StateHasChanged();
    }
}
