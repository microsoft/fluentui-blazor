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

    /// <summary>
    /// Gets or sets the template to be used to define each sortable item in the list.
    /// Use the @context parameter to access the item and its properties.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment<TItem>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the list of items to be displayed in a sortable list.
    /// </summary>
    [Parameter, AllowNull]
    public IEnumerable<TItem> Items { get; set; }

    /// <summary>
    /// Event callback for when the list is updated.
    /// </summary>
    [Parameter]
    public EventCallback<FluentSortableListEventArgs> OnUpdate { get; set; }

    /// <summary>
    /// Event callback for when an item is removed from the list.
    /// </summary>
    [Parameter]
    public EventCallback<FluentSortableListEventArgs> OnRemove { get; set; }

    /// <summary>
    /// Gets or sets the name of the Group used for dragging between lists. Set the group to the same value on both lists to enable.
    /// You can only have 1 group with 2 lists.
    /// </summary>
    [Parameter]
    public string? Group { get; set; }

    /// <summary>
    /// Gets or sets whether elements are cloned instead of moved. Set Pull to "clone" to enable this.
    /// </summary>
    [Parameter]
    public bool Clone { get; set; } = false;

    /// <summary>
    /// Gets or sets wether it is possible to drop items into the current list from another list in the same group. 
    /// Set to false to disable dropping from another list onto the current list.
    /// </summary>
    [Parameter]
    public bool Drop { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the list is sortable.
    /// Default is true
    /// Disable sorting within a list by setting to false.
    /// </summary>
    [Parameter]
    public bool Sort { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the whole item acts as drag handle.
    /// Set to true to use an element with classname `.sortable-grab` as the handle.
    /// </summary>
    [Parameter]
    public bool Handle { get; set; } = false;

    /// <summary>
    /// Gets or sets the function to filter out elements that cannot be sorted or moved.
    /// </summary>
    [Parameter]
    public Func<TItem, bool>? ItemFilter { get; set; }

    /// <summary>
    /// Gets or sets wether ro ignore the HTML5 DnD behaviour and force the fallback to kick in
    /// </summary>
    [Parameter]
    public bool Fallback { get; set; } = false;

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-sortable-list")
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    private string Filter => Items.Any(GetItemFiltered) ? ".filtered" : string.Empty;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _selfReference = DotNetObjectReference.Create(this);
            IJSObjectReference? module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            await module.InvokeAsync<string>("init", Id, Group, Clone ? "clone" : null, Drop, Sort, Handle ? ".sortable-grab" : null, Filter, Fallback, _selfReference);
        }
    }

    protected bool GetItemFiltered(TItem item)
    {
        if (ItemFilter != null)
        {
            return ItemFilter(item);
        }
        else
        {
            return false;
        }
    }

    [JSInvokable]
    public void OnUpdateJS(int oldIndex, int newIndex)
    {
        if (OnUpdate.HasDelegate)
        {
            // invoke the OnUpdate event passing in the oldIndex and the newIndex
            OnUpdate.InvokeAsync(new FluentSortableListEventArgs(oldIndex, newIndex));
        }
    }

    [JSInvokable]
    public void OnRemoveJS(int oldIndex, int newIndex)
    {
        if (OnRemove.HasDelegate)
        {
            // remove the item from the list
            OnRemove.InvokeAsync(new FluentSortableListEventArgs(oldIndex, newIndex));
        }
    }

    public void Dispose() => _selfReference?.Dispose();
}
