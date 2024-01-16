using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentSortableList<TItem> : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/SortableList/FluentSortableList.razor.js";
    private DotNetObjectReference<FluentSortableList<TItem>>? _selfReference;

    public FluentSortableList()
    {
        Id = Identifier.NewId();
    }

    [Parameter, EditorRequired]
    public RenderFragment<TItem>? SortableItemTemplate { get; set; }

    [Parameter, AllowNull]
    public IEnumerable<TItem> Items { get; set; }

    [Parameter]
    public EventCallback<(int oldIndex, int newIndex)> OnUpdate { get; set; }

    [Parameter]
    public EventCallback<(int oldIndex, int newIndex)> OnRemove { get; set; }

    [Parameter]
    public string? Group { get; set; }

    [Parameter]
    public string? Pull { get; set; }

    [Parameter]
    public bool Put { get; set; } = true;

    [Parameter]
    public bool Sort { get; set; } = true;

    [Parameter]
    public string Handle { get; set; } = string.Empty;

    [Parameter]
    public string Filter { get; set; } = string.Empty;

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-sortable-list")
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _selfReference = DotNetObjectReference.Create(this);
            IJSObjectReference? module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            await module.InvokeAsync<string>("init", Id, Group, Pull, Put, Sort, Handle, Filter, _selfReference);
        }
    }

    [JSInvokable]
    public void OnUpdateJS(int oldIndex, int newIndex)
    {
        // invoke the OnUpdate event passing in the oldIndex and the newIndex
        OnUpdate.InvokeAsync((oldIndex, newIndex));
    }

    [JSInvokable]
    public void OnRemoveJS(int oldIndex, int newIndex)
    {
        // remove the item from the list
        OnRemove.InvokeAsync((oldIndex, newIndex));
    }

    public void Dispose() => _selfReference?.Dispose();
}