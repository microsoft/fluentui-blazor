// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Shared;

public partial class DemoSearch
{
    [Inject] protected DemoNavProvider NavProvider { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

    private string? _searchValue = string.Empty;

    private bool _visible;

    private List<string>? _searchResults = DefaultResults();
    private static List<string>? DefaultResults() => null;

    private void HandleSearchInput()
    {
        if (string.IsNullOrWhiteSpace(_searchValue))
        {
            _searchResults = DefaultResults();
            _searchValue = string.Empty;
        }
        else
        {
            var searchTerm = _searchValue.ToLower();

            if (searchTerm.Length > 0)
            {
                _searchResults = NavProvider.FlattenedMenuItems
                    .Where(x => x.Href != null) // Ignore Group headers
                    .Where(x => x.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.Title)
                    .ToList();

                if (_searchResults.Count == 0)
                {
                    _searchResults = DefaultResults();
                }
            }
        }

        _visible = _searchResults is not null;
    }

    private void HandleSearchClicked(NavItem item)
    {
        _searchValue = string.Empty;
        _searchResults = DefaultResults();
        _visible = false;
        InvokeAsync(StateHasChanged);

        NavigationManager.NavigateTo(item.Href ?? throw new UnreachableException("Item has no href"));
    }
}
