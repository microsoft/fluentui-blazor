using System.Reflection;
using FluentUI.Demo.AssetExplorer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.AssetExplorer.Components.Pages; 

public partial class IconExplorer
{
    private const int ITEMS_PER_PAGE = 24;
    private const string JAVASCRIPT_FILE = "./_content/FluentUI.Demo.AssetExplorer/Components/Pages/IconExplorer.razor.js";

    private bool SearchInProgress = false;

    private readonly IconSearchCriteria Criteria = new();
    private IconInfo[] IconsFound = Array.Empty<IconInfo>();    
    private PaginationState PaginationState = new PaginationState { ItemsPerPage = ITEMS_PER_PAGE };

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private IJSObjectReference? JSModule { get; set; }

    [Inject]
    public IToastService ToastService { get; set; } = default!;

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

    public async void CopyToClipboardAsync(IconInfo icon)
    {
        string value = $"Value=\"@(new Icons.{icon.Variant}.{icon.Size}.{icon.Name}())\"";
        string color = Criteria.Color == Color.Accent ? string.Empty : $" Color=\"@Color.{Criteria.Color}\"";
        
        string code = $"<FluentIcon {value}{color} />";

        //JSModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
        //var error = await JSModule.InvokeAsync<string>("copyToClipboard", code);

        ToastService.ShowSuccess($"FluentIcon `{icon.Name}` component declaration copied to clipboard.");
    }
}
