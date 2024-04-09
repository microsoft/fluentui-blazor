using System.Data;
using FluentUI.Demo.AssetExplorer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.AssetExplorer.Components.Pages;

public partial class IconExplorer
{
    private bool SearchInProgress = false;

    private readonly IconSearchCriteria Criteria = new();
    private IconInfo[] IconsFound = Array.Empty<IconInfo>();
    private PaginationState PaginationState = new() { ItemsPerPage = 4 * 12 };

    [Parameter]
    public string Title { get; set; } = "FluentUI Blazor - Icon Explorers";

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public string? Height { get; set; }

    [Parameter]
    public int ItemsPerPage { get; set; }

    private IEnumerable<IconInfo> IconsForCurrentPage
    {
        get
        {
            return IconsFound.Skip(PaginationState.CurrentPageIndex * PaginationState.ItemsPerPage)
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

        IconsFound =
        [
            .. Icons.AllIcons
                    .Where(i => i.Variant == Criteria.Variant
                             && (Criteria.Size > 0 ? (int)i.Size == Criteria.Size : true)
                             && (string.IsNullOrWhiteSpace(Criteria.SearchTerm) ? true : i.Name.Contains(Criteria.SearchTerm, StringComparison.InvariantCultureIgnoreCase)))
                    .OrderBy(i => i.Name)
,
        ];

        await PaginationState.SetTotalItemCountAsync(IconsFound.Length);

        SearchInProgress = false;
    }

    private void HandleCurrentPageIndexChanged()
    {
        StateHasChanged();
    }

    private IEnumerable<int> AllAvailableSizes
    {
        get
        {
            var sizes = Enum.GetValues<IconSize>().Select(i => (int)i).ToList();
            var empty = new int[] { 0 };
            return empty.Concat(sizes);
        }
    }
}
