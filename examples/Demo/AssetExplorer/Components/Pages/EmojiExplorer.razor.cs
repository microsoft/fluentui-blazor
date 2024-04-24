using FluentUI.Demo.AssetExplorer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.AssetExplorer.Components.Pages;

public partial class EmojiExplorer
{
    private bool SearchInProgress = false;

    private readonly EmojiSearchCriteria Criteria = new();
    private EmojiInfo[] EmojisFound = Array.Empty<EmojiInfo>();
    private PaginationState PaginationState = new() { ItemsPerPage = 4 * 12 };

    [Parameter]
    public string Title { get; set; } = "FluentUI Blazor - Emoji Explorers";

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public string? Height { get; set; }

    [Parameter]
    public int ItemsPerPage { get; set; }

    private IEnumerable<EmojiInfo> EmojisForCurrentPage
    {
        get
        {
            return EmojisFound.Skip(PaginationState.CurrentPageIndex * PaginationState.ItemsPerPage)
                              .Take(PaginationState.ItemsPerPage);
        }
    }

    protected override void OnInitialized()
    {
        if (ItemsPerPage > 0)
        {
            PaginationState = new PaginationState { ItemsPerPage = ItemsPerPage };
        }
    }

    private async Task StartNewSearchAsync()
    {
        SearchInProgress = true;
        await Task.Delay(1); // Display spinner

        EmojisFound =
        [
            .. Emojis.AllEmojis
                                        .Where(i => i.Style == Criteria.Style
                                                 && (string.IsNullOrWhiteSpace(Criteria.SearchTerm) ? true : i.Name.Contains(Criteria.SearchTerm, StringComparison.InvariantCultureIgnoreCase)))
                                        .OrderBy(i => i.Name)
,
        ];

        await PaginationState.SetTotalItemCountAsync(EmojisFound.Length);

        SearchInProgress = false;
    }

    private void HandleCurrentPageIndexChanged()
    {
        StateHasChanged();
    }
}
