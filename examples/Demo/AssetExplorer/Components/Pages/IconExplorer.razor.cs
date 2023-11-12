using System.Data;
using FluentUI.Demo.AssetExplorer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.AssetExplorer.Components.Pages; 

public partial class IconExplorer
{
    private const int ITEMS_PER_PAGE = 4 * 12;
    
    private bool SearchInProgress = false;

    private readonly IconSearchCriteria Criteria = new();
    private IconInfo[] IconsFound = Array.Empty<IconInfo>();    
    private PaginationState PaginationState = new PaginationState { ItemsPerPage = ITEMS_PER_PAGE };

    [Parameter]
    public string Title { get; set; } = "FluentUI Blazor - Icon Explorers";

    [Parameter]
    public string Width { get; set; } = "95%";

    [Parameter]
    public string Height { get; set; } = "100%";

    private IEnumerable<IconInfo> IconsForCurrentPage
    {
        get
        {
            return IconsFound.Skip(PaginationState.CurrentPageIndex * PaginationState.ItemsPerPage)
                             .Take(PaginationState.ItemsPerPage);
        }
    }

    private async Task StartNewSearchAsync()
    {
        SearchInProgress = true;
        await Task.Delay(1); // Display spinner

        IconsFound = Icons.AllIcons
                          .Where(i => i.Variant == Criteria.Variant
                                   && i.Size == Criteria.Size
                                   && (string.IsNullOrWhiteSpace(Criteria.SearchTerm) ? true : i.Name.Contains(Criteria.SearchTerm, StringComparison.InvariantCultureIgnoreCase)))
                          .OrderBy(i => i.Name)
                          .ToArray();

        await PaginationState.SetTotalItemCountAsync(IconsFound.Length);

        SearchInProgress = false;
    }

    private void HandleCurrentPageIndexChanged()
    {
        StateHasChanged();
    }
}
