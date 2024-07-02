// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Shared;

public partial class DemoSearch : IAsyncDisposable
{
    [Inject]
    public required DemoNavProvider NavProvider { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    [Inject]
    public required IKeyCodeService KeyCodeService { get; set; }

    private FluentAutocomplete<NavItem>? _searchAutocomplete = default!;
    private string? _searchTerm = "";
    private IEnumerable<NavItem>? _selectedOptions = [];

    protected override void OnInitialized()
    {
        KeyCodeService.RegisterListener(OnKeyDownAsync);
    }

    public Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
    {
        if (args is not null && args.Key == KeyCode.Slash)
        {
            _searchAutocomplete?.Element?.FocusAsync();
        }
        //StateHasChanged();
        return Task.CompletedTask;
    }

    private void HandleSearchInput(OptionsSearchEventArgs<NavItem> e)
    {
        var searchTerm = e.Text;

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            e.Items = null;
        }
        else
        {
            e.Items = NavProvider.FlattenedMenuItems
                .Where(x => x.Href != null) // Ignore Group headers
                .Where(x => x.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }

    private void HandleSearchClicked()
    {
        _searchTerm = null;
        var targetHref = _selectedOptions?.SingleOrDefault()?.Href;
        _selectedOptions = [];
        InvokeAsync(StateHasChanged);

        // Ignore clearing the search bar
        if (targetHref is null)
        {
            return;
        }

        NavigationManager.NavigateTo(targetHref ?? throw new UnreachableException("Item has no href"));
    }

    public ValueTask DisposeAsync()
    {
        KeyCodeService.UnregisterListener(OnKeyDownAsync, OnKeyDownAsync);
        return ValueTask.CompletedTask;
    }

}
