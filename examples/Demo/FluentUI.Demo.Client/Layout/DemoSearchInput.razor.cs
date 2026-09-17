// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Client.Layout;

public partial class DemoSearchInput
{
    private const string SearchInputId = "DemoSearchInput";
    private static readonly KeyCode[] SearchInputKeyCodes = [KeyCode.Slash, KeyCode.NumpadDivide];
    private string _currentRoute = string.Empty;
    private readonly List<DemoSearchInputSearchEntry> _searchEntries = [];
    private string _searchText = string.Empty;
    private DemoSearchInputSearchResult? _selectedItem;

    [Inject]
    public required DocViewerService DocViewerService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    [Inject]
    public required IKeyCodeService KeyCodeService { get; set; }

    [Inject]
    public required IJSRuntime JSRuntime { get; set; }

    protected override void OnInitialized()
    {
        _searchEntries.AddRange(DocViewerService.Pages
                                                .Where(page => !page.Hidden)
                                                .Select(page => new DemoSearchInputSearchEntry(page)));
    }

    private void Search(OptionsSearchEventArgs<DemoSearchInputSearchResult> args)
    {
        _searchText = args.Text.Trim();
        if (string.IsNullOrEmpty(_searchText))
        {
            args.Items = DemoSearchInputSearchResult.CreateDefaultResults(_searchEntries);
            return;
        }

        args.Items = DemoSearchInputSearchResult.CreateSearchResults(_searchEntries, _searchText);
    }

    private void NavigateToSelectedPage(DemoSearchInputSearchResult? result)
    {
        if (result is null)
        {
            return;
        }

        _currentRoute = result.Route;
        _selectedItem = null;
        _searchText = string.Empty;
        NavigationManager.NavigateTo(result.Route);
    }

    public async Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
    {
        if (SearchInputKeyCodes.Contains(args.Key))
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Autocomplete.setFocus", SearchInputId);
        }
    }
}