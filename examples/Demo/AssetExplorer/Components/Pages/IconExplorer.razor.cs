using System.Data;
using FluentUI.Demo.AssetExplorer.Extensions;
using FluentUI.Demo.AssetExplorer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.AssetExplorer.Components.Pages;

public partial class IconExplorer
{
    private const string JAVASCRIPT_FILE = "./Components/Pages/IconExplorer.razor.js";
    private bool SearchInProgress = false;

    private readonly IconSearchCriteria Criteria = new();
    private IconInfo[] IconsFound = Array.Empty<IconInfo>();
    private PaginationState PaginationState = new() { ItemsPerPage = 4 * 12 };
    private string _searchResultMessage = "Start search...";
    private ElementReference resultList;

    [Parameter]
    public string Title { get; set; } = "FluentUI Blazor - Icon Explorers";

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public string? Height { get; set; }

    [Parameter]
    public int ItemsPerPage { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;
    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    [Inject]
    private ILogger<IconExplorer> logger { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    protected override void OnInitialized()
    {
        if (ItemsPerPage > 0)
        {
            PaginationState = new PaginationState { ItemsPerPage = ItemsPerPage };
        }
    }

    private async Task StartNewSearchAsync(string property)
    {
        Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
        if (property == nameof(Criteria.Variant) && Criteria.Variant == IconVariant.Light && Criteria.Size != 32)
        {
            Criteria.Size = 32;
        }

        SearchInProgress = true;
        await Task.Delay(1); // Display spinner
        var allIconsPossible = Criteria.OnlyCoreIcons ? IconsExtensions.CoreIcons
            : IconsExtensions.AllIcons;
        IconsFound = 
        [
            .. allIconsPossible
                    .Where(i => i.Variant == Criteria.Variant
                             && (Criteria.Size > 0 ? (int)i.Size == Criteria.Size : true)
                             && (string.IsNullOrWhiteSpace(Criteria.SearchTerm) ? true : i.Name.Contains(Criteria.SearchTerm, StringComparison.InvariantCultureIgnoreCase)))
                    .OrderBy(i => i.Name)
,
        ];

        await PaginationState.SetCurrentPageIndexAsync(0);

        await PaginationState.SetTotalItemCountAsync(IconsFound.Length);

        _searchResultMessage = IconsFound.Length == 0 ? "No icons found." : string.Empty;
        SearchInProgress = false;
        
        StateHasChanged();
    }

    private async ValueTask<ItemsProviderResult<IconRow>> GetRows(ItemsProviderRequest request)
    {
        var widthOfList = await Module!.InvokeAsync<double>("getElementProperty", resultList, "offsetWidth");
        var elementPerRow = (int)Math.Floor(widthOfList / 120);
        //logger.LogInformation("Width of list: {widthOfList}, elements per row: {elementPerRow}", widthOfList, elementPerRow);
        var rows = IconsFound.Select((i, x) => new { i, x })
            .GroupBy(x => x.x / elementPerRow)
            .Select(g => new IconRow(g.Select(x => x.i)))
            .ToArray();
        var totalCount = rows.Length;
        var rowsOnPage = rows.Skip(request.StartIndex).Take(request.Count);
        logger.LogInformation("Request {startIndex}-{endIndex} of {totalCount} rows, returning {returnedCount} rows", request.StartIndex, request.StartIndex + request.Count, totalCount, rowsOnPage.Count());
        return new ItemsProviderResult<IconRow>(
            rowsOnPage,
            totalCount);
    }

    private IEnumerable<int> AllAvailableSizes
    {
        get
        {
            var sizes = Enum.GetValues<IconSize>().Where(i => i > 0).Select(i => (int)i).ToList();
            var empty = new int[] { 0 };
            return empty.Concat(sizes);
        }
    }

    record IconRow(IEnumerable<IconInfo> Icons);
}
