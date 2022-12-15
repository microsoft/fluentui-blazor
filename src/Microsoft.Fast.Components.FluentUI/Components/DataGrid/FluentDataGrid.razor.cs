// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;
using Microsoft.Fast.Components.FluentUI.Infrastructure;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// A component that displays a grid.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public partial class FluentDataGrid<TGridItem> : FluentComponentBase, IHandleEvent, IAsyncDisposable where TGridItem : class
{
    #region Properties

    #region Parameters

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
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public TGridItem? SelectedItem { get; set; }

    [Parameter]
    public EventCallback<TGridItem> SelectedItemChanged { get; set; }

    [Parameter]
    public bool IsReadonly { get; set; }

    #region Columns

    /// <summary>
    /// Gets or sets the value that gets applied to the css gridTemplateColumns attribute of child rows
    /// </summary>
    [Parameter]
    public string? GridTemplateColumns { get; set; } = null;

    /// <summary>
    /// Whether the grid should automatically generate a header row and its type
    /// See <see cref="GenerateHeaderOption"/>
    /// </summary>
    [Parameter]
    public GenerateHeaderOption? GenerateHeader { get; set; } = GenerateHeaderOption.Default;

    /// <summary>
    /// If true, renders draggable handles around the column headers, allowing the user to resize the columns
    /// manually. Size changes are not persisted.
    /// </summary>
    [Parameter] public bool ResizableColumns { get; set; }

    #endregion

    #region Injected

    [Inject] private IServiceProvider Services { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    #endregion

    #endregion

    #region Events

    /// <summary>
    /// Gets or sets a callback when a row is focused
    /// </summary>
    [Parameter]
    public EventCallback<FluentDataGridCell<TGridItem>> OnCellFocus { get; set; }

    /// <summary>
    /// Gets or sets a callback when a row is focused
    /// </summary>
    [Parameter]
    public EventCallback<FluentDataGridRow<TGridItem>> OnRowFocus { get; set; }

    [Parameter]
    public EventCallback<DataGridBeginEditEventArgs<TGridItem>> CellBeginEdit { get; set; }

    [Parameter]
    public EventCallback<DataGridCellEditEndingEventArgs<TGridItem>> CellEditEnding { get; set; }

    [Parameter]
    public EventCallback<DataGridCellEditEndedEventArgs<TGridItem>> CellEditEnded { get; set; }

    [Parameter]
    public EventCallback<DataGridRowEditEndingEventArgs<TGridItem>> RowEditEnding { get; set; }

    [Parameter]
    public EventCallback<DataGridRowEditEndedEventArgs<TGridItem>> RowEditEnded { get; set; }

    [Parameter]
    public EventCallback<TGridItem> RowDelete { get; set; }

    #endregion

    #endregion

    #region Fields

    private IEnumerable<TGridItem>? _internalItemsSource;

    private ElementReference _gridReference;
    private Virtualize<(int, TGridItem)>? _virtualizeComponent;
    private int _ariaBodyRowCount;
    //private ICollection<TGridItem> _currentNonVirtualizedViewItems = Array.Empty<TGridItem>();

    // IQueryable only exposes synchronous query APIs. IAsyncQueryExecutor is an adapter that lets us invoke any
    // async query APIs that might be available. We have built-in support for using EF Core's async query APIs.
    private IAsyncQueryExecutor? _asyncQueryExecutor;

    // We cascade the InternalGridContext to descendants, which in turn call it to add themselves to _columns
    // This happens on every render so that the column list can be updated dynamically
    private InternalGridContext<TGridItem> _internalGridContext;
    private List<ColumnBase<TGridItem>> _columns;
    private List<ColumnBase<TGridItem>> _sortableColumns;
    private List<ColumnBase<TGridItem>> _filterableColumns;
    private bool _collectingColumns; // Columns might re-render themselves arbitrarily. We only want to capture them at a defined time.

    // Tracking state for options and sorting
    private ColumnBase<TGridItem>? _displayOptionsForColumn;
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

    private FluentDataGridRow<TGridItem>? _currentRow;

    bool ImplementedIEditableObject = false;

    #endregion

    #region Initialization

    /// <summary>
    /// Constructs an instance of <see cref="FluentDataGrid{TGridItem}"/>.
    /// </summary>
    public FluentDataGrid()
    {
        _columns = new();
        _sortableColumns = new();
        _filterableColumns = new();
        _internalGridContext = new(this);
        _currentPageItemsChanged = new(EventCallback.Factory.Create<PaginationState>(this, RefreshDataCoreAsync));
        _renderColumnHeaders = RenderColumnHeaders;
        _renderNonVirtualizedRows = RenderNonVirtualizedRows;
        ImplementedIEditableObject = typeof(IEditableObject).IsAssignableFrom(typeof(TGridItem));
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

    #endregion

    #region Functions

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import",
                "./_content/Microsoft.Fast.Components.FluentUI/Components/DataGrid/FluentDataGrid.razor.js");
            _jsEventDisposable = await _jsModule.InvokeAsync<IJSObjectReference>("init", _gridReference);
        }

        if (_checkColumnOptionsPosition && _displayOptionsForColumn is not null)
        {
            _checkColumnOptionsPosition = false;
            _ = _jsModule?.InvokeVoidAsync("checkColumnOptionsPosition", _gridReference);
        }
    }

    #region Columns

    private void StartCollectingColumns()
    {
        _columns.Clear();
        _sortableColumns.Clear();
        _filterableColumns.Clear();
        _collectingColumns = true;
    }

    private void FinishCollectingColumns()
    {
        _collectingColumns = false;
    }

    // Invoked by descendant columns at a special time during rendering
    internal void AddColumn(ColumnBase<TGridItem> column)
    {
        if (_collectingColumns)
        {
            _columns.Add(column);

            if (column.IsSortableByDefault() && column.Sortable.HasValue && column.Sortable.Value)
            {
                _sortableColumns.Add(column);
                if (column.DefaultSort.HasValue)
                    ((ISortableColumn<TGridItem>)column).SortOrder = _sortableColumns.Any() ? (short)(_sortableColumns.Max(m => ((ISortableColumn<TGridItem>)m).SortOrder) + 1) : (short)1;
            }
            if (column.IsFilterableByDefault() && column.Filterable.HasValue && column.Filterable.Value)
                _filterableColumns.Add(column);
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

    private string? ColumnHeaderClass(ColumnBase<TGridItem> column)
    {
        Nullable<System.ComponentModel.ListSortDirection> sortDirection = null;
        if (_sortableColumns.Any(a => a == column && ((ISortableColumn<TGridItem>)a).SortDirection.HasValue))
        {
            sortDirection = ((ISortableColumn<TGridItem>)column).SortDirection!.Value;
        }
        return sortDirection.HasValue
       ? $"{ColumnClass(column)} {(sortDirection.Value == ListSortDirection.Ascending ? "col-sort-asc" : "col-sort-desc")}"
       : ColumnClass(column);
    }

    private static string? ColumnClass(ColumnBase<TGridItem> column) => column.Align switch
    {
        Align.Center => $"col-justify-center {column.Class}",
        Align.Right => $"col-justify-end {column.Class}",
        _ => column.Class,
    };

    private string AriaSortValue(ColumnBase<TGridItem> column)
    {
        Nullable<System.ComponentModel.ListSortDirection> sortDirection = null;
        if (_sortableColumns.Any(a => a == column && ((ISortableColumn<TGridItem>)a).SortDirection.HasValue))
        {
            sortDirection = ((ISortableColumn<TGridItem>)column).SortDirection!.Value;
        }
        return sortDirection.HasValue ? (sortDirection.Value == ListSortDirection.Ascending ? "ascending" : "descending") : "none";
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

    #endregion

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
            startIndex, count, _sortableColumns, _filterableColumns, request.CancellationToken);
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

    private async Task HandleOnRowFocus(DataGridRowFocusEventArgs args)
    {
        string? rowId = args.RowId;
        if (_internalGridContext.Rows.TryGetValue(rowId!, out FluentDataGridRow<TGridItem>? row))
        {
            if (row is null)
                return;
            if (_currentRow is null || IsReadonly)
            {
                SetCurrentRow(row);
                return;
            }
            if (_currentRow.RowId.Equals(row.RowId))
                return;
            if (_currentRow.Mode == DataGridItemMode.Edit)
            {
                var canCommit = await EndEdit(_currentRow, EditActionEnum.Commit);
                if (canCommit)
                    SetCurrentRow(row);
            }
            else
                SetCurrentRow(row);
        }
    }

    #region Filter and Sort

    /// <summary>
    /// apply sort to the specified <paramref name="column"/>.
    /// </summary>
    /// <param name="column">The column that defines the new sort order.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    internal Task ApplySort(ISortableColumn<TGridItem> column)
    {
        if (column.SortDirection.HasValue)
        {
            if (_sortableColumns.Any())
                column.SortOrder = (short)(_sortableColumns.Max(m => ((ISortableColumn<TGridItem>)m).SortOrder) + 1);
            else
                column.SortOrder = 1;
        }
        return RefreshDataAsync();
    }

    /// <summary>
    /// recalculate filters
    /// </summary>
    /// <param name="column"></param>
    /// <returns></returns>
    internal Task ApplyFilter(IFilterableColumn<TGridItem> column)
    {
        return RefreshDataAsync();
    }

    #endregion


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
                startIndex, Pagination?.ItemsPerPage, _sortableColumns, _filterableColumns, thisLoadCts.Token);
            var result = await ResolveItemsRequestAsync(request);
            if (!thisLoadCts.IsCancellationRequested)
            {
                _internalItemsSource = result.Items;
                _ariaBodyRowCount = _internalItemsSource.Count();
                Pagination?.SetTotalItemCountAsync(result.TotalItemCount);
                _pendingDataLoadCancellationTokenSource = null;
            }
        }
        StateHasChanged();
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
            var result = request.ApplyFilterAndSorting(RowsData)?.Skip(request.StartIndex);
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

    private string? GridClass()
    {
        string? value = $"{Class} {(_pendingDataLoadCancellationTokenSource is null ? null : "loading")}".Trim();
        if (string.IsNullOrEmpty(value))
            return null;
        else
            return value;
    }

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

    Task IHandleEvent.HandleEventAsync(
      EventCallbackWorkItem callback, object? arg) => callback.InvokeAsync(arg);

    #region Edit functions


    internal bool BeginItemEdit(FluentDataGridRow<TGridItem> row)
    {
        if (_currentRow is null)
            return false;
        //row must be selected befor edit
        if (!_currentRow.RowId.Equals(row.RowId))
            return false;

        if (SelectedItem is null || row.Item is null)
            return false;

        if (ImplementedIEditableObject)
            ((IEditableObject)row.Item).BeginEdit();

        return true;
    }

    internal bool BeginPropertyEdit(TGridItem Item, string ProeprtyName)
    {
        if (_currentRow is null || _currentRow.Mode == DataGridItemMode.Readonly)
            return false;
        var arg = new DataGridBeginEditEventArgs<TGridItem>(Item, ProeprtyName);
        if (CellBeginEdit.HasDelegate)
            CellBeginEdit.InvokeAsync(arg);
        return !arg.Cancel;
    }

    internal async Task<bool> EndEdit(FluentDataGridRow<TGridItem> row, EditActionEnum EditAction)
    {
        if (_currentRow is null || row is null || !row.RowId.Equals(_currentRow.RowId))
            return false;
        if (EditAction == EditActionEnum.Commit)
        {
            var canCommit = await row.CanCommit();
            if (!canCommit)
                return false;
            var arg = new DataGridRowEditEndingEventArgs<TGridItem>(row.Item!, EditAction);
            if (RowEditEnding.HasDelegate)
                await RowEditEnding.InvokeAsync(arg);
            if (arg.Cancel)
                return false;
            _currentRow.EndEdit();
        }
        var arg2 = new DataGridRowEditEndedEventArgs<TGridItem>(row.Item!, EditAction);
        if (RowEditEnded.HasDelegate)
            await RowEditEnded.InvokeAsync(arg2);
        return true;
    }

    internal void CancelEdit(FluentDataGridRow<TGridItem> row)
    {
        if (_currentRow is null || row is null || !row.RowId.Equals(_currentRow.RowId))
            return;
        if (_currentRow.Mode == DataGridItemMode.Readonly)
            return;
        row.CancelEdit();
    }

    private async void OnKeyPress(KeyboardEventArgs e)
    {
        if (_currentRow is null || IsReadonly)
            return;
        if (e.Key == "Enter" && _currentRow.Mode == DataGridItemMode.Edit)
            await EndEdit(_currentRow, EditActionEnum.Commit);
        else if (e.Key == "Escape" && _currentRow.Mode == DataGridItemMode.Edit)
            CancelEdit(_currentRow);
        else if (e.Key == "Delete" && _currentRow is not null && _currentRow.Item is not null && _currentRow.Mode == DataGridItemMode.Readonly)
        {
            if (RowDelete.HasDelegate)
                await RowDelete.InvokeAsync(_currentRow.Item);
            Console.WriteLine($"Item {_currentRow.RowId} is deleted");
        }
    }

    internal bool EditEndingForCell(TGridItem Item, string PropertyName, object? newValue, EditActionEnum EditAction)
    {
        var arg = new DataGridCellEditEndingEventArgs<TGridItem>(Item, PropertyName, newValue, EditAction);
        if (CellEditEnding.HasDelegate)
            CellEditEnding.InvokeAsync(arg);
        return !arg.Cancel;
    }

    internal void EditEndedForCell(TGridItem Item, string PropertyName, EditActionEnum EditAction)
    {
        var arg = new DataGridCellEditEndedEventArgs<TGridItem>(Item, PropertyName, EditAction);
        if (CellEditEnded.HasDelegate)
            CellEditEnded.InvokeAsync(arg);
    }

    internal void setCellEditableConfig(FluentDataGridRow<TGridItem> row)
    {
        _jsModule?.InvokeVoidAsync("setCellEditableConfig", _gridReference, row.RowId);
    }

    internal void removeCellEditableConfig(FluentDataGridRow<TGridItem> row)
    {
        _jsModule?.InvokeVoidAsync("removeCellEditableConfig", _gridReference, row.RowId);
    }

    #endregion


    private async void SetCurrentRow(FluentDataGridRow<TGridItem> row)
    {
        if (row.Item is not null)
            SelectedItem = row.Item;
        else
            SelectedItem = null;
        _currentRow = row;
        await OnRowFocus.InvokeAsync(row);
    }

    #endregion
}
