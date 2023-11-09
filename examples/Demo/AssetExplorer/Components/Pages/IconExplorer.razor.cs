using FluentUI.Demo.AssetExplorer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.AssetExplorer.Components.Pages; 

public partial class IconExplorer
{
    private bool SearchInProgress = false;

    private readonly IconSearchCriteria Criteria = new();
    private IconInfo[] IconsFound = Array.Empty<IconInfo>();
    
    private IJSObjectReference? _jsModule;
    private PaginationState PaginationState = new PaginationState { ItemsPerPage = 25 };

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private IEnumerable<IconInfo> IconsForCurrentPage
    {
        get
        {
            return IconsFound.Skip(PaginationState.CurrentPageIndex * PaginationState.ItemsPerPage)
                             .Take(PaginationState.ItemsPerPage);
        }
    }

    /// <summary>
    /// Start a new search.
    /// </summary>
    /// <returns></returns>
    private async Task HandleSearchAsync()
    {
        Console.WriteLine($"HandleSearchAsync {Criteria.Size}");

        SearchInProgress = true;

        IconsFound = Icons.AllIcons
                          .Where(i => i.Variant == Criteria.Variant
                                   && i.Size == Criteria.Size
                                   && (string.IsNullOrWhiteSpace(Criteria.SearchTerm) ? true : i.Name.Contains(Criteria.SearchTerm, StringComparison.InvariantCultureIgnoreCase)))
                          .ToArray();
        
        await PaginationState.SetTotalItemCountAsync(IconsFound.Length);

        SearchInProgress = false;
    }

    private void HandleCurrentPageIndexChanged()
    {
        StateHasChanged();
    }

    //public async void HandleClick(IconInfo icon)
    //{
    //    string Text = $$"""<FluentIcon Value="@(new Icons.{{icon.Variant}}.{{icon.Size}}.{{icon.Name}}())" Color="@Color.{{IconColor}}"/>""";

    //    if (_jsModule is not null)
    //    {
    //        await _jsModule.InvokeVoidAsync("copyText", Text);
    //    }
    //}

}
