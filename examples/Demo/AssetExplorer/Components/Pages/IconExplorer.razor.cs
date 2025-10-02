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
    private string _searchResultMessage = "Start search...";

    [Parameter]
    public string Title { get; set; } = "FluentUI Blazor - Icon Explorers";

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public string? Height { get; set; }

    private async Task StartNewSearchAsync(string property)
    {
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

        _searchResultMessage = IconsFound.Length == 0 ? "No icons found." : string.Empty;
        SearchInProgress = false;

        StateHasChanged();
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
