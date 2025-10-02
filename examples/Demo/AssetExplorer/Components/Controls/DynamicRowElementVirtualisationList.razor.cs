// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using FluentUI.Demo.AssetExplorer.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.AssetExplorer.Components.Controls;

public partial class DynamicRowElementVirtualisationList<TItem>: ComponentBase
{
    private const string JAVASCRIPT_FILE = "./Components/Controls/DynamicRowElementVirtualisationList.razor.js";
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;
    /// <summary>
    /// The component js module
    /// </summary>
    private IJSObjectReference? Module { get; set; }
    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;
    private ElementReference resultList;
    /// <summary>
    /// Gets or sets the function providing items to the list.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IQueryable<TItem> ItemsProvider { get; set; }

    [Parameter]
    [EditorRequired]
    public RenderFragment<TItem>? ItemTemplate { get; set; }

    private async ValueTask<ItemsProviderResult<ItemRow>> GetRows(ItemsProviderRequest request)
    {
        var widthOfList = await Module!.InvokeAsync<double>("getElementProperty", resultList, "offsetWidth");
        var elementPerRow = (int)Math.Floor(widthOfList / 120);
        var rows = ItemsProvider.Select((i, x) => new { i, x })
            .GroupBy(x => x.x / elementPerRow)
            .Select(g => new ItemRow(g.Select(x => x.i)))
            .ToArray();
        var totalCount = rows.Length;
        var rowsOnPage = rows.Skip(request.StartIndex).Take(request.Count);
        return new ItemsProviderResult<ItemRow>(
            rowsOnPage,
            totalCount);
    }

    protected override async Task OnInitializedAsync()
    {
        Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
        await base.OnInitializedAsync();
    }

    record ItemRow(IEnumerable<TItem> Items);
}
