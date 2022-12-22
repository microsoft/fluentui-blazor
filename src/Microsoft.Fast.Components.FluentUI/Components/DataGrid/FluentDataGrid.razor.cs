// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;
using Microsoft.Fast.Components.FluentUI.Infrastructure;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// A component that displays a grid.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public partial class FluentDataGrid<TGridItem> : FluentComponentBase, IHandleEvent, IAsyncDisposable
{
    /// <summary>
    /// A queryable source of data for the grid.
    ///
    /// This could be in-memory data converted to queryable using the
    /// <see cref="System.Linq.Queryable.AsQueryable(System.Collections.IEnumerable)"/> extension method,
    /// or an EntityFramework DataSet or an <see cref="IQueryable"/> derived from it.
    ///
    /// You should supply either <see cref="RowsData"/> or <see cref="RowsDataProvider"/>, but not both.
    /// </summary>
    [Parameter] public IQueryable<TGridItem>? RowsData { get; set; }

    /// <summary>
    /// A callback that supplies data for the rid.
    ///
    /// You should supply either <see cref="RowsData"/> or <see cref="RowsDataProvider"/>, but not both.
    /// </summary>
    [Parameter] public GridItemsProvider<TGridItem>? RowsDataProvider { get; set; }

    /// <summary>
    /// Defines the child components of this instance. For example, you may define columns by adding
    /// components derived from the <see cref="ColumnBase{TGridItem}"/> base class.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    
    /// <summary>
    /// If true, the grid will be rendered with virtualization. This is normally used in conjunction with
    /// scrolling and causes the grid to fetch and render only the data around the current scroll viewport.
    /// This can greatly improve the performance when scrolling through large data sets.
    ///
    /// If you use <see cref="Virtualize"/>, you should supply a value for <see cref="RowsDataSize"/> and must
    /// ensure that every row renders with the same constant height.
    ///
    /// Generally it's preferable not to use <see cref="Virtualize"/> if the amount of data being rendered
    /// is small or if you are using pagination.
    /// </summary>
    [Parameter] public bool Virtualize { get; set; }

    /// <summary>
    /// This is applicable only when using <see cref="Virtualize"/>. It defines an expected height in pixels for
    /// each row, allowing the virtualization mechanism to fetch the correct number of items to match the display
    /// size and to ensure accurate scrolling.
    /// </summary>
    [Parameter] public float RowsDataSize { get; set; } = 32;

    /// <summary>
    /// If true, renders draggable handles around the column headers, allowing the user to resize the columns
    /// manually. Size changes are not persisted.
    /// </summary>
    [Parameter] public bool ResizableColumns { get; set; }

    /// <summary>
    /// Optionally defines a value for @key on each rendered row. Typically this should be used to specify a
    /// unique identifier, such as a primary key value, for each data item.
    ///
    /// This allows the grid to preserve the association between row elements and data items based on their
    /// unique identifiers, even when the <typeparamref name="TGridItem"/> instances are replaced by new copies (for
    /// example, after a new query against the underlying data store).
    ///
    /// If not set, the @key will be the <typeparamref name="TGridItem"/> instance itself.
    /// </summary>
    [Parameter] public Func<TGridItem, object> RowsDataKey { get; set; } = x => x!;

    /// <summary>
    /// Optionally links this <see cref="FluentDataGrid{TGridItem}"/> instance with a <see cref="PaginationState"/> model,
    /// causing the grid to fetch and render only the current page of data.
    ///
    /// This is normally used in conjunction with a <see cref="FluentPaginator"/> component or some other UI logic
    /// that displays and updates the supplied <see cref="PaginationState"/> instance.
    /// </summary>
    [Parameter] public PaginationState? Pagination { get; set; }


    /// <summary>
    /// When true the component will not add itself to the tab queue. Default is false.
    /// </summary>
    [Parameter]
    public bool NoTabbing { get; set; }

    /// <summary>
    /// Whether the grid should automatically generate a header row and its type
    /// See <see cref="GenerateHeaderOption"/>
    /// </summary>
    [Parameter]
    public GenerateHeaderOption? GenerateHeader { get; set; } = GenerateHeaderOption.Default;

    /// <summary>
    /// Gets or sets the value that gets applied to the css gridTemplateColumns attribute of child rows
    /// </summary>
    [Parameter]
    public string? GridTemplateColumns { get; set; } = null;

    /// <summary>
    /// Gets or sets a callback when a row is focused
    /// </summary>
    [Parameter]
    public EventCallback<FluentDataGridRow<TGridItem>> OnRowFocus { get; set; }

    /// <summary>
    /// Gets or sets a callback when a row is focused
    /// </summary>
    [Parameter]
    public EventCallback<FluentDataGridCell<TGridItem>> OnCellFocus { get; set; }

    /// <summary>
    /// Optionally defines a class to be applied to a rendered row. 
    /// </summary>
    [Parameter] public Func<TGridItem, string>? RowClass { get; set; }

    [Inject] private IServiceProvider Services { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private ElementReference _gridReference;
    private Virtualize<(int, TGridItem)>? _virtualizeComponent;
    private int _ariaBodyRowCount;
    private ICollection<TGridItem> _currentNonVirtualizedViewItems = Array.Empty<TGridItem>();

    // IQueryable only exposes synchronous query APIs. IAsyncQueryExecutor is an adapter that lets us invoke any
    // async query APIs that might be available. We have built-in support for using EF Core's async query APIs.
    private IAsyncQueryExecutor? _asyncQueryExecutor;

    // We cascade the InternalGridContext to descendants, which in turn call it to add themselves to _columns
    // This happens on every render so that the column list can be updated dynamically
    private InternalGridContext<TGridItem> _internalGridContext;
    private List<ColumnBase<TGridItem>> _columns;
    private bool _collectingColumns; // Columns might re-render themselves arbitrarily. We only want to capture them at a defined time.

    // Tracking state for options and sorting
    private ColumnBase<TGridItem>? _displayOptionsForColumn;
    private List<ColumnBase<TGridItem>> _sortableColumns;
    private bool _checkColumnOptionsPosition;

    // The associated ES6 module, which uses document-level event listeners
    private IJSObjectReference? _jsModule;
    private IJSObjectReference? _jsEventDisposable;

    // Caches of method->delegate conversions
    private readonly RenderFragment _renderColumnHeaders;
    private readonly RenderFragment _renderNonVirtualizedRows;

    // We try to minimize the number of times we query the items provider, since queries may be expensive
    // We only re-query when the developer calls RefreshDataAsync, or if we know something's changed, such
    // as sort order, the pagination state, or the data source itself. These fields help us detect when
    // things have changed, and to discard earlier load attempts that were superseded.
    private int? _lastRefreshedPaginationStateHash;
    private object? _lastAssignedItemsOrProvider;
    private CancellationTokenSource? _pendingDataLoadCancellationTokenSource;

    // If the PaginationState mutates, it raises this event. We use it to trigger a re-render.
    private readonly EventCallbackSubscriber<PaginationState> _currentPageItemsChanged;

    /// <summary>
    /// Constructs an instance of <see cref="FluentDataGrid{TGridItem}"/>.
    /// </summary>
    public FluentDataGrid()
    {
        _columns = new();
        _sortableColumns = new();
        _internalGridContext = new(this);
        _currentPageItemsChanged = new(EventCallback.Factory.Create<PaginationState>(this, RefreshDataCoreAsync));
        _renderColumnHeaders = RenderColumnHeaders;
        _renderNonVirtualizedRows = RenderNonVirtualizedRows;

        // As a special case, we don't issue the first data load request until we've collected the initial set of columns
        // This is so we can apply default sort order (or any future per-column options) before loading data
        // We use EventCallbackSubscriber to safely hook this async operation into the synchronous rendering flow
        var columnsFirstCollectedSubscriber = new EventCallbackSubscriber<object?>(
            EventCallback.Factory.Create<object?>(this, RefreshDataCoreAsync));
        columnsFirstCollectedSubscriber.SubscribeOrMove(_internalGridContext.ColumnsFirstCollected);
    }

    /// <inheritdoc />
    protected override Task OnParametersSetAsync()
    {
        // The associated pagination state may have been added/removed/replaced
        _currentPageItemsChanged.SubscribeOrMove(Pagination?.CurrentPageItemsChanged);

        if (RowsData is not null && RowsDataProvider is not null)
        {
            throw new InvalidOperationException($"FluentDataGrid requires one of {nameof(RowsData)} or {nameof(RowsDataProvider)}, but both were specified.");
        }

        // Perform a re-query only if the data source or something else has changed
        var _newRowsDataOrRowsDataProvider = RowsData ?? (object?)RowsDataProvider;
        var dataSourceHasChanged = _newRowsDataOrRowsDataProvider != _lastAssignedItemsOrProvider;
        if (dataSourceHasChanged)
        {
            _lastAssignedItemsOrProvider = _newRowsDataOrRowsDataProvider;
            _asyncQueryExecutor = AsyncQueryExecutorSupplier.GetAsyncQueryExecutor(Services, RowsData);
        }

        var mustRefreshData = dataSourceHasChanged
            || (Pagination?.GetHashCode() != _lastRefreshedPaginationStateHash);

        // We don't want to trigger the first data load until we've collected the initial set of columns,
        // because they might perform some action like setting the default sort order, so it would be wasteful
        // to have to re-query immediately
        return (_columns.Count > 0 && mustRefreshData) ? RefreshDataCoreAsync() : Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Microsoft.Fast.Components.FluentUI/Components/DataGrid/FluentDataGrid.razor.js");
            _jsEventDisposable = await _jsModule.InvokeAsync<IJSObjectReference>("init", _gridReference);
        }

        if (_checkColumnOptionsPosition && _displayOptionsForColumn is not null)
        {
            _checkColumnOptionsPosition = false;
            _ = _jsModule?.InvokeVoidAsync("checkColumnOptionsPosition", _gridReference);
        }
    }

    // Invoked by descendant columns at a special time during rendering
    internal void AddColumn(ColumnBase<TGridItem> column)
    {
        if (_collectingColumns)
        {
            _columns.Add(column);

            if (column.SortingIsEnable())
            {
                _sortableColumns.Add(column);
                if (column.DefaultSort.HasValue && !column.DefaultSortApplied)
                {
                    column.DefaultSortApplied = true;
                    ISortableColumn<TGridItem> ccol = (column as ISortableColumn<TGridItem>)!;
                    if (!ccol.SortDirection.HasValue)
                    {
                        ccol.SortOrder = _sortableColumns.Any() ? (short)(_sortableColumns.Max(m => ((ISortableColumn<TGridItem>)m).SortOrder) + 1) : (short)1;
                        ccol.SortDirection = column.DefaultSort.Value;
                    }
                }
            }
        }
    }

    private void StartCollectingColumns()
    {
        _columns.Clear();
        _sortableColumns.Clear();
        _collectingColumns = true;
    }

    private void FinishCollectingColumns()
    {
        _collectingColumns = false;
    }

    /// <summary>
    /// Displays the <see cref="ColumnBase{TGridItem}.ColumnOptions"/> UI for the specified column, closing any other column
    /// options UI that was previously displayed.
    /// </summary>
    /// <param name="column">The column whose options are to be displayed, if any are available.</param>
    public void ShowColumnOptions(ColumnBase<TGridItem> column)
    {
        _displayOptionsForColumn = column;
        _checkColumnOptionsPosition = true; // Triggers a call to JSRuntime to position the options element, apply autofocus, and any other setup
        StateHasChanged();
    }

    /// <summary>
    /// Instructs the grid to re-fetch and render the current data from the supplied data source
    /// (either <see cref="RowsData"/> or <see cref="RowsDataProvider"/>).
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the completion of the operation.</returns>
    public async Task RefreshDataAsync()
    {
        await RefreshDataCoreAsync();
        StateHasChanged();
    }

    // Same as RefreshDataAsync, except without forcing a re-render. We use this from OnParametersSetAsync
    // because in that case there's going to be a re-render anyway.
    private async Task RefreshDataCoreAsync()
    {
        // Move into a "loading" state, cancelling any earlier-but-still-pending load
        _pendingDataLoadCancellationTokenSource?.Cancel();
        var thisLoadCts = _pendingDataLoadCancellationTokenSource = new CancellationTokenSource();

        if (_virtualizeComponent is not null)
        {
            // If we're using Virtualize, we have to go through its RefreshDataAsync API otherwise:
            // (1) It won't know to update its own internal state if the provider output has changed
            // (2) We won't know what slice of data to query for
            await _virtualizeComponent.RefreshDataAsync();
            _pendingDataLoadCancellationTokenSource = null;
        }
        else
        {
            // If we're not using Virtualize, we build and execute a request against the items provider directly
            _lastRefreshedPaginationStateHash = Pagination?.GetHashCode();
            var startIndex = Pagination is null ? 0 : (Pagination.CurrentPageIndex * Pagination.ItemsPerPage);
            var request = new GridItemsProviderRequest<TGridItem>(
                startIndex, Pagination?.ItemsPerPage, _sortableColumns, thisLoadCts.Token);
            var result = await ResolveItemsRequestAsync(request);
            if (!thisLoadCts.IsCancellationRequested)
            {
                _currentNonVirtualizedViewItems = result.Items;
                _ariaBodyRowCount = _currentNonVirtualizedViewItems.Count;
                Pagination?.SetTotalItemCountAsync(result.TotalItemCount);
                _pendingDataLoadCancellationTokenSource = null;
            }
        }
        StateHasChanged();
    }

    // Gets called both by RefreshDataCoreAsync and directly by the Virtualize child component during scrolling
    private async ValueTask<ItemsProviderResult<(int, TGridItem)>> ProvideVirtualizedItems(ItemsProviderRequest request)
    {
        _lastRefreshedPaginationStateHash = Pagination?.GetHashCode();

        // Debounce the requests. This eliminates a lot of redundant queries at the cost of slight lag after interactions.
        // TODO: Consider making this configurable, or smarter (e.g., doesn't delay on first call in a batch, then the amount
        // of delay increases if you rapidly issue repeated requests, such as when scrolling a long way)
        await Task.Delay(100);
        if (request.CancellationToken.IsCancellationRequested)
        {
            return default;
        }

        // Combine the query parameters from Virtualize with the ones from PaginationState
        var startIndex = request.StartIndex;
        var count = request.Count;
        if (Pagination is not null)
        {
            startIndex += Pagination.CurrentPageIndex * Pagination.ItemsPerPage;
            count = Math.Min(request.Count, Pagination.ItemsPerPage - request.StartIndex);
        }

        var providerRequest = new GridItemsProviderRequest<TGridItem>(
            startIndex, count, _sortableColumns, request.CancellationToken);
        var providerResult = await ResolveItemsRequestAsync(providerRequest);

        if (!request.CancellationToken.IsCancellationRequested)
        {
            // ARIA's rowcount is part of the UI, so it should reflect what the human user regards as the number of rows in the table,
            // not the number of physical <tr> elements. For virtualization this means what's in the entire scrollable range, not just
            // the current viewport. In the case where you're also paginating then it means what's conceptually on the current page.
            // TODO: This currently assumes we always want to expand the last page to have ItemsPerPage rows, but the experience might
            //       be better if we let the last page only be as big as its number of actual rows.
            _ariaBodyRowCount = Pagination is null ? providerResult.TotalItemCount : Pagination.ItemsPerPage;

            Pagination?.SetTotalItemCountAsync(providerResult.TotalItemCount);

            // We're supplying the row index along with each row's data because we need it for aria-rowindex, and we have to account for
            // the virtualized start index. It might be more performant just to have some _latestQueryRowStartIndex field, but we'd have
            // to make sure it doesn't get out of sync with the rows being rendered.
            return new ItemsProviderResult<(int, TGridItem)>(
                 items: providerResult.Items.Select((x, i) => ValueTuple.Create(i + request.StartIndex + 2, x)),
                 totalItemCount: _ariaBodyRowCount);
        }

        return default;
    }

    // Normalizes all the different ways of configuring a data source so they have common GridItemsProvider-shaped API
    private async ValueTask<GridItemsProviderResult<TGridItem>> ResolveItemsRequestAsync(GridItemsProviderRequest<TGridItem> request)
    {
        if (RowsDataProvider is not null)
        {
            return await RowsDataProvider(request);
        }
        else if (RowsData is not null)
        {
            var totalItemCount = _asyncQueryExecutor is null ? RowsData.Count() : await _asyncQueryExecutor.CountAsync(RowsData);
            var result = request.ApplySorting(RowsData)?.Skip(request.StartIndex);
            if (result is null)
                return GridItemsProviderResult.From(Array.Empty<TGridItem>(), 0);
            if (request.Count.HasValue)
            {
                result = result.Take(request.Count.Value);
            }
            var resultArray = _asyncQueryExecutor is null ? result.ToArray() : await _asyncQueryExecutor.ToArrayAsync(result);
            return GridItemsProviderResult.From(resultArray, totalItemCount);
        }
        else
        {
            return GridItemsProviderResult.From(Array.Empty<TGridItem>(), 0);
        }
    }

    /// <summary>
    /// apply sort to the specified <paramref name="column"/>.
    /// </summary>
    /// <param name="column">The column that defines the new sort order.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    internal Task ApplySort(ISortableColumn<TGridItem> column)
    {
        if (column.SortDirection.HasValue)
        {
            var otherSortedColumns = _sortableColumns.Cast<ISortableColumn<TGridItem>>().Where(w => w.SortDirection.HasValue);
            if (otherSortedColumns.Any())
                column.SortOrder = (short)(otherSortedColumns.Max(m => m.SortOrder) + 1);
            else
                column.SortOrder = 1;
        }
        return RefreshDataAsync();
    }

    private string AriaSortValue(ColumnBase<TGridItem> column)
    {
        ListSortDirection? sortDirection = null;
        if (_sortableColumns.Any(a => a == column && ((ISortableColumn<TGridItem>)a).SortDirection.HasValue))
        {
            sortDirection = ((ISortableColumn<TGridItem>)column).SortDirection!.Value;
        }
        return sortDirection.HasValue ? (sortDirection.Value == ListSortDirection.Ascending ? "ascending" : "descending") : "none";
    }

    private string? ColumnHeaderClass(ColumnBase<TGridItem> column)
    {
        ListSortDirection? sortDirection = null;
        if (_sortableColumns.Any(a => a == column && ((ISortableColumn<TGridItem>)a).SortDirection.HasValue))
        {
            sortDirection = ((ISortableColumn<TGridItem>)column).SortDirection!.Value;
        }
        return sortDirection.HasValue
       ? $"{ColumnClass(column)} {(sortDirection.Value == ListSortDirection.Ascending ? "col-sort-asc" : "col-sort-desc")}"
       : ColumnClass(column);
    }

    private string? GridClass()
    {
        string? value = $"{Class} {(_pendingDataLoadCancellationTokenSource is null ? null : "loading")}".Trim();
        if (string.IsNullOrEmpty(value))
            return null;
        else
            return value;
    }

    private static string? ColumnClass(ColumnBase<TGridItem> column) => column.Align switch
    {
        Align.Center => $"col-justify-center {column.Class}",
        Align.Right => $"col-justify-end {column.Class}",
        _ => column.Class,
    };

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        _currentPageItemsChanged.Dispose();

        try
        {
            if (_jsEventDisposable is not null)
            {
                await _jsEventDisposable.InvokeVoidAsync("stop");
                await _jsEventDisposable.DisposeAsync();
            }

            if (_jsModule is not null)
            {
                await _jsModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }

    internal void CloseColumnOptions()
    {
        if (_displayOptionsForColumn is not null)
        {
            _displayOptionsForColumn.CloseFilter();
            _displayOptionsForColumn = null;
        }
    }

    private async Task HandleOnRowFocus(DataGridRowFocusEventArgs args)
    {
        string? rowId = args.RowId;
        if (_internalGridContext.Rows.TryGetValue(rowId!, out FluentDataGridRow<TGridItem>? row))
        {
            await OnRowFocus.InvokeAsync(row);
        }
    }

    Task IHandleEvent.HandleEventAsync(
      EventCallbackWorkItem callback, object? arg) => callback.InvokeAsync(arg);
}
