// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A sortable list component that allows users to reorder items via drag-and-drop.
/// <typeparam name="TItem">The type of the items in the list.</typeparam>
/// </summary>
public partial class FluentSortableList<TItem> : FluentComponentBase, IAsyncDisposable
{
    //private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "SortableList/FluentSortableList.razor.js";
    private ElementReference? _element;
    private DotNetObjectReference<FluentSortableList<TItem>>? _selfReference;
    private IJSObjectReference? _jsHandle;
    private bool _disposed;

    /// <summary />
    public FluentSortableList(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-sortable-list")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
    .AddStyle("--fluent-sortable-list-filtered", ListItemFilteredColor, !string.IsNullOrEmpty(ListItemFilteredColor))
    .AddStyle("--fluent-sortable-list-border-width", ListBorderWidth, !string.IsNullOrEmpty(ListBorderWidth))
    .AddStyle("--fluent-sortable-list-border-color", ListBorderColor, !string.IsNullOrEmpty(ListBorderColor))
    .AddStyle("--fluent-sortable-list-padding", ListPadding, !string.IsNullOrEmpty(ListPadding))
    .AddStyle("--fluent-sortable-list-background-color", ListItemBackgroundColor, !string.IsNullOrEmpty(ListItemBackgroundColor))
    .AddStyle("--fluent-sortable-list-item-height", ListItemHeight, !string.IsNullOrEmpty(ListItemHeight))
    .AddStyle("--fluent-sortable-list-item-border-width", ListItemBorderWidth, !string.IsNullOrEmpty(ListItemBorderWidth))
    .AddStyle("--fluent-sortable-list-item-border-color", ListItemBorderColor, !string.IsNullOrEmpty(ListItemBorderColor))
    .AddStyle("--fluent-sortable-list-item-drop-border-color", ListItemDropBorderColor, !string.IsNullOrEmpty(ListItemDropBorderColor))
    .AddStyle("--fluent-sortable-list-item-drop-color", ListItemDropColor, !string.IsNullOrEmpty(ListItemDropColor))
    .AddStyle("--fluent-sortable-list-item-padding", ListItemPadding, !string.IsNullOrEmpty(ListItemPadding))
    .AddStyle("--fluent-sortable-list-item-spacing", ListItemSpacing, !string.IsNullOrEmpty(ListItemSpacing))
    .Build();

    /// <summary>
    /// Gets or sets the text used on `aria-label` attribute.
    /// </summary>
    [Parameter]
    public string? AriaLabel { get; set; }

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
    public IEnumerable<TItem>? Items { get; set; }

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
    /// Gets or sets wether to ignore the HTML5 DnD behaviour and force the fallback to kick in
    /// </summary>
    [Parameter]
    public bool Fallback { get; set; } = false;

    /// <summary>
    /// Gets or sets the color of filtered list items.
    /// </summary>
    [Parameter]
    public string? ListItemFilteredColor { get; set; }

    /// <summary>
    /// Gets or sets the border width on the list. Must be a valid CSS measurement.
    /// </summary>
    [Parameter]
    public string? ListBorderWidth { get; set; }

    /// <summary>
    /// Gets or sets the color of the border on the list.
    /// </summary>
    [Parameter]
    public string? ListBorderColor { get; set; }

    /// <summary>
    /// Gets or sets the padding on the list. Must be a valid CSS measurement.
    /// </summary>
    [Parameter]
    public string? ListPadding { get; set; }

    /// <summary>
    /// Gets or sets the background color of the list items.
    /// </summary>
    [Parameter]
    public string? ListItemBackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the height of the list items. Must be a valid CSS measurement.
    /// </summary>
    [Parameter]
    public string? ListItemHeight { get; set; }

    /// <summary>
    /// Gets or sets the border width on the list items. Must be a valid CSS measurement.
    /// </summary>
    [Parameter]
    public string? ListItemBorderWidth { get; set; }

    /// <summary>
    /// Gets or sets the border color of the list items.
    /// </summary>
    [Parameter]
    public string? ListItemBorderColor { get; set; }

    /// <summary>
    /// Gets or sets the border color of the list items during repositioning.
    /// </summary>
    [Parameter]
    public string? ListItemDropBorderColor { get; set; }

    /// <summary>
    /// Gets or sets the background color of the list items during repositioning.
    /// </summary>
    [Parameter]
    public string? ListItemDropColor { get; set; }

    /// <summary>
    /// Gets or sets the padding on the list items. Must be a valid CSS measurement.
    /// </summary>
    [Parameter]
    public string? ListItemPadding { get; set; }

    /// <summary>
    /// Gets or sets the spacing between list items. Must be a valid CSS measurement.
    /// </summary>
    [Parameter]
    public string? ListItemSpacing { get; set; }

    private string? Filter => (Items?.Any(GetItemFiltered) ?? false) ? ".filtered" : string.Empty;

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _selfReference = DotNetObjectReference.Create(this);

            if (!_disposed)
            {
                // list, group, pull: any, put:  sort: boolean, handle: string | null, filter: string | null, fallback: boolean, component: any
                _jsHandle = await JSRuntime.InvokeAsync<IJSObjectReference>("Microsoft.FluentUI.Blazor.Components.SortableList.Initialize", _element, Group, Clone ? "clone" : null, Drop, Sort, Handle ? ".sortable-grab" : null, Filter, Fallback, _selfReference);
            }
        }
    }

    /// <summary />
    protected bool GetItemFiltered(TItem item)
    {
        if (ItemFilter != null)
        {
            return ItemFilter(item);
        }

        return false;
    }

    /// <summary>
    /// Invoked from JavaScript when an item is updated.
    /// </summary>
    [JSInvokable]
    public void OnUpdateJS(int oldIndex, int newIndex, string fromListId, string toListId)
    {
        if (OnUpdate.HasDelegate)
        {
            // invoke the OnUpdate event passing in the oldIndex, the newIndex, the fromId and the toId
            _ = OnUpdate.InvokeAsync(new FluentSortableListEventArgs(oldIndex, newIndex, fromListId, toListId));
        }
    }

    /// <summary>
    /// Invoked from JavaScript when an item is removed.
    /// </summary>
    [JSInvokable]
    public void OnRemoveJS(int oldIndex, int newIndex, string fromListId, string toListId)
    {
        if (OnRemove.HasDelegate)
        {
            // remove the item from the list
            _ = OnRemove.InvokeAsync(new FluentSortableListEventArgs(oldIndex, newIndex, fromListId, toListId));
        }
    }

    /// <summary />
    public override async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        if (_jsHandle is not null)
        {
            try
            {
                await _jsHandle.InvokeVoidAsync("stop");
                await _jsHandle.DisposeAsync();
            }
            catch (Exception)
            {
                /* Ignore */
            }
        }

        _selfReference?.Dispose();
        _disposed = true;
        await base.DisposeAsync();
    }
}
