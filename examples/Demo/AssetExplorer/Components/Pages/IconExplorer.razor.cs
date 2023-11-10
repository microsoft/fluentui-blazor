using System.Data;
using FluentUI.Demo.AssetExplorer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.AssetExplorer.Components.Pages; 

public partial class IconExplorer
{
    private const int ITEMS_PER_PAGE = 4 * 12;
    private const string JAVASCRIPT_FILE = "./_content/FluentUI.Demo.AssetExplorer/Components/Pages/IconExplorer.razor.js";

    private bool SearchInProgress = false;

    private readonly IconSearchCriteria Criteria = new();
    private IconInfo[] IconsFound = Array.Empty<IconInfo>();    
    private PaginationState PaginationState = new PaginationState { ItemsPerPage = ITEMS_PER_PAGE };

    private IJSObjectReference? JSModule { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    public IToastService ToastService { get; set; } = default!;

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
