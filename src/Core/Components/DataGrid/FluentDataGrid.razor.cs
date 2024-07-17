// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Infrastructure;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A component that displays a grid.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public partial class FluentDataGrid<TGridItem> : FluentComponentBase, IHandleEvent, IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/DataGrid/FluentDataGrid.razor.js";

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    [Inject]
    private IServiceProvider Services { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private IKeyCodeService KeyCodeService { get; set; } = default!;

    /// <summary>
    /// Gets or sets a queryable source of data for the grid.
    ///
    /// This could be in-memory data converted to queryable using the
    /// <see cref="System.Linq.Queryable.AsQueryable(System.Collections.IEnumerable)"/> extension method,
    /// or an EntityFramework DataSet or an <see cref="IQueryable"/> derived from it.
    ///
    /// You should supply either <see cref="Items"/> or <see cref="ItemsProvider"/>, but not both.
    /// </summary>
    [Parameter]
    public IQueryable<TGridItem>? Items { get; set; }

    /// <summary>
    /// Gets or sets a callback that supplies data for the rid.
    ///
    /// You should supply either <see cref="Items"/> or <see cref="ItemsProvider"/>, but not both.
    /// </summary>
    [Parameter]
    public GridItemsProvider<TGridItem>? ItemsProvider { get; set; }

    /// <summary>
    /// Gets or sets the child components of this instance. For example, you may define columns by adding
    /// components derived from the <see cref="ColumnBase{TGridItem}"/> base class.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// If true, the grid will be rendered with virtualization. This is normally used in conjunction with
    /// scrolling and causes the grid to fetch and render only the data around the current scroll viewport.
    /// This can greatly improve the performance when scrolling through large data sets.
    ///
    /// If you use <see cref="Virtualize"/>, you should supply a value for <see cref="ItemSize"/> and must
    /// ensure that every row renders with the same constant height.
    ///
    /// Generally it's preferable not to use <see cref="Virtualize"/> if the amount of data being rendered
    /// is small or if you are using pagination.
    /// </summary>
    [Parameter]
    public bool Virtualize { get; set; }

    /// <summary>
    /// This is applicable only when using <see cref="Virtualize"/>. It defines an expected height in pixels for
    /// each row, allowing the virtualization mechanism to fetch the correct number of items to match the display
    /// size and to ensure accurate scrolling.
    /// </summary>
    [Parameter]
    public float ItemSize { get; set; } = 32;

    /// <summary>
    /// If true, renders draggable handles around the column headers, allowing the user to resize the columns
    /// manually. Size changes are not persisted.
    /// </summary>
    [Parameter]
    public bool ResizableColumns { get; set; }

    /// <summary>
    /// To comply with WCAG 2.2, a one-click option should be offered to change column widths. We provide such an option through the
    /// ColumnOptions UI. This parameter allows you to enable or disable this resize UI.Enable it by setting the type of resize to perform
    /// Discrete: resize by a 10 pixels at a time
    /// Exact: resize to the exact width specified (in pixels)
    /// </summary>
    [Parameter]
    public DataGridResizeType? ResizeType { get; set; }

    [Parameter]
    public string ResizeLabel { get; set; } = "Column width (in pixels)";

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
    [Parameter]
    public Func<TGridItem, object> ItemKey { get; set; } = x => x!;

    /// <summary>
    /// Optionally links this <see cref="FluentDataGrid{TGridItem}"/> instance with a <see cref="PaginationState"/> model,
    /// causing the grid to fetch and render only the current page of data.
    ///
    /// This is normally used in conjunction with a <see cref="FluentPaginator"/> component or some other UI logic
    /// that displays and updates the supplied <see cref="PaginationState"/> instance.
    /// </summary>
    [Parameter]
    public PaginationState? Pagination { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component will not add itself to the tab queue.
    /// Default is false.
    /// </summary>
    [Parameter]
    public bool NoTabbing { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the grid should automatically generate a header row and its type.
    /// See <see cref="GenerateHeaderOption"/>
    /// </summary>
    [Parameter]
    public GenerateHeaderOption? GenerateHeader { get; set; } = GenerateHeaderOption.Default;

    /// <summary>
    /// Gets or sets the value that gets applied to the css gridTemplateColumns attribute of child rows.
    /// Can be specified here or on the column level with the Width parameter but not both.
    /// Needs to be a valid CSS string of space-separated values, such as "auto 1fr 2fr 100px".
    /// </summary>
    [Parameter]
    public string? GridTemplateColumns { get; set; } = null;

    /// <summary>
    /// Gets or sets a callback when a row is focused.
    /// </summary>
    [Parameter]
    public EventCallback<FluentDataGridRow<TGridItem>> OnRowFocus { get; set; }

    /// <summary>
    /// Gets or sets a callback when a row is focused.
    /// </summary>
    [Parameter]
    public EventCallback<FluentDataGridCell<TGridItem>> OnCellFocus { get; set; }

    /// <summary>
    /// Gets or sets a callback when a cell is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<FluentDataGridCell<TGridItem>> OnCellClick { get; set; }

    /// <summary>
    /// Gets or sets a callback when a row is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<FluentDataGridRow<TGridItem>> OnRowClick { get; set; }

    /// <summary>
    /// Gets or sets a callback when a row is double-clicked.
    /// </summary>
    [Parameter]
    public EventCallback<FluentDataGridRow<TGridItem>> OnRowDoubleClick { get; set; }

    /// <summary>
    /// Optionally defines a class to be applied to a rendered row.
    /// </summary>
    [Parameter]
    public Func<TGridItem, string>? RowClass { get; set; }

    /// <summary>
    /// Optionally defines a style to be applied to a rendered row.
    /// Do not use to dynamically update a row style after rendering as this will interfere with the script that use this attribute. Use <see cref="RowClass"/> instead.
    /// </summary>
    [Parameter]
    public Func<TGridItem, string>? RowStyle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the grid should show a hover effect on rows.
    /// </summary>
    [Parameter]
    public bool ShowHover { get; set; }

    /// <summary>
    /// If specified, grids render this fragment when there is no content.
    /// </summary>
    [Parameter]
    public RenderFragment? EmptyContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the grid is in a loading data state.
    /// </summary>
    [Parameter]
    public bool Loading { get; set; }

    /// <summary>
    /// Gets or sets the content to render when <see cref="Loading"/> is true.
    /// A default fragment is used if loading content is not specified.
    /// </summary>
    [Parameter]
    public RenderFragment? LoadingContent { get; set; }

    private ElementReference? _gridReference;
    private Virtualize<(int, TGridItem)>? _virtualizeComponent;

    // IQueryable only exposes synchronous query APIs. IAsyncQueryExecutor is an adapter that lets us invoke any
    // async query APIs that might be available. We have built-in support for using EF Core's async query APIs.
    private IAsyncQueryExecutor? _asyncQueryExecutor;

    // We cascade the InternalGridContext to descendants, which in turn call it to add themselves to _columns
    // This happens on every render so that the column list can be updated dynamically
    private readonly InternalGridContext<TGridItem> _internalGridContext;
    internal readonly List<ColumnBase<TGridItem>> _columns;
    private bool _collectingColumns; // Columns might re-render themselves arbitrarily. We only want to capture them at a defined time.

    // Tracking state for options and sorting
    private ColumnBase<TGridItem>? _displayOptionsForColumn;
    private ColumnBase<TGridItem>? _sortByColumn;
    private bool _sortByAscending;
    private bool _checkColumnOptionsPosition;
    private bool _manualGrid;

    // The associated ES6 module, which uses document-level event listeners
    private IJSObjectReference? Module;
    private IJSObjectReference? _jsEventDisposable;

    // Caches of method->delegate conversions
    private readonly RenderFragment _renderColumnHeaders;
    private readonly RenderFragment _renderNonVirtualizedRows;

    private readonly RenderFragment _renderEmptyContent;
    private readonly RenderFragment _renderLoadingContent;

    private string? _internalGridTemplateColumns;

    // We try to minimize the number of times we query the items provider, since queries may be expensive
    // We only re-query when the developer calls RefreshDataAsync, or if we know something's changed, such
    // as sort order, the pagination state, or the data source itself. These fields help us detect when
    // things have changed, and to discard earlier load attempts that were superseded.
    private int? _lastRefreshedPaginationStateHash;
    private object? _lastAssignedItemsOrProvider;
    private CancellationTokenSource? _pendingDataLoadCancellationTokenSource;

    // If the PaginationState mutates, it raises this event. We use it to trigger a re-render.
    private readonly EventCallbackSubscriber<PaginationState> _currentPageItemsChanged;
    public bool? SortByAscending => _sortByAscending;

    /// <summary>
    /// Constructs an instance of <see cref="FluentDataGrid{TGridItem}"/>.
    /// </summary>
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DataGridCellFocusEventArgs))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DataGridRowFocusEventArgs))]
    public FluentDataGrid()
    {
        _columns = [];
        _internalGridContext = new(this);
        _currentPageItemsChanged = new(EventCallback.Factory.Create<PaginationState>(this, RefreshDataCoreAsync));
        _renderColumnHeaders = RenderColumnHeaders;
        _renderNonVirtualizedRows = RenderNonVirtualizedRows;
        _renderEmptyContent = RenderEmptyContent;
        _renderLoadingContent = RenderLoadingContent;

        // As a special case, we don't issue the first data load request until we've collected the initial set of columns
        // This is so we can apply default sort order (or any future per-column options) before loading data
        // We use EventCallbackSubscriber to safely hook this async operation into the synchronous rendering flow
        EventCallbackSubscriber<object?>? columnsFirstCollectedSubscriber = new(
            EventCallback.Factory.Create<object?>(this, RefreshDataCoreAsync));
        columnsFirstCollectedSubscriber.SubscribeOrMove(_internalGridContext.ColumnsFirstCollected);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        KeyCodeService.RegisterListener(OnKeyDownAsync);
    }

    /// <inheritdoc />
    protected override Task OnParametersSetAsync()
    {
        _internalGridTemplateColumns = GridTemplateColumns;

        // The associated pagination state may have been added/removed/replaced
        _currentPageItemsChanged.SubscribeOrMove(Pagination?.CurrentPageItemsChanged);

        if (Items is not null && ItemsProvider is not null)
        {
            throw new InvalidOperationException($"FluentDataGrid requires one of {nameof(Items)} or {nameof(ItemsProvider)}, but both were specified.");
        }

        // Perform a re-query only if the data source or something else has changed
        var _newItemsOrItemsProvider = Items ?? (object?)ItemsProvider;
        var dataSourceHasChanged = _newItemsOrItemsProvider != _lastAssignedItemsOrProvider;
        if (dataSourceHasChanged)
        {
            _lastAssignedItemsOrProvider = _newItemsOrItemsProvider;
            _asyncQueryExecutor = AsyncQueryExecutorSupplier.GetAsyncQueryExecutor(Services, Items);
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
        if (firstRender && _gridReference is not null)
        {
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
            try
            {
                _jsEventDisposable = await Module.InvokeAsync<IJSObjectReference>("init", _gridReference);
            }
            catch (JSException ex)
            {
                Console.WriteLine("[FluentDataGrid] " + ex.Message);
            }
        }

        if (_checkColumnOptionsPosition && _displayOptionsForColumn is not null)
        {
            _checkColumnOptionsPosition = false;
            _ = Module?.InvokeVoidAsync("checkColumnOptionsPosition", _gridReference).AsTask();
        }
    }

    // Invoked by descendant columns at a special time during rendering
    internal void AddColumn(ColumnBase<TGridItem> column, SortDirection? initialSortDirection, bool isDefaultSortColumn)
    {
        if (_collectingColumns)
        {
            _columns.Add(column);

            if (isDefaultSortColumn && _sortByColumn is null && initialSortDirection.HasValue)
            {
                _sortByColumn = column;
                _sortByAscending = initialSortDirection.Value != SortDirection.Descending;
                _internalGridContext.DefaultSortColumn = (column, initialSortDirection.Value);
            }
        }
    }

    private void StartCollectingColumns()
    {
        _columns.Clear();
        _collectingColumns = true;
    }

    private void FinishCollectingColumns()
    {
        _collectingColumns = false;
        _manualGrid = _columns.Count == 0;

        if (!string.IsNullOrWhiteSpace(GridTemplateColumns) && _columns.Where(x => x is not SelectColumn<TGridItem>).Any(x => !string.IsNullOrWhiteSpace(x.Width)))
        {
            throw new Exception("You can use either the 'GridTemplateColumns' parameter on the grid or the 'Width' property at the column level, not both.");
        }

        if (string.IsNullOrWhiteSpace(_internalGridTemplateColumns) && _columns.Any(x => !string.IsNullOrWhiteSpace(x.Width)))
        {
            _internalGridTemplateColumns = string.Join(" ", _columns.Select(x => x.Width ?? "1fr"));
        }

        if (ResizableColumns)
        {
            _ = Module?.InvokeVoidAsync("enableColumnResizing", _gridReference).AsTask();
        }
    }

    /// <summary>
    /// Sets the grid's current sort column to the specified <paramref name="column"/>.
    /// </summary>
    /// <param name="column">The column that defines the new sort order.</param>
    /// <param name="direction">The direction of sorting. If the value is <see cref="SortDirection.Auto"/>, then it will toggle the direction on each call.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task SortByColumnAsync(ColumnBase<TGridItem> column, SortDirection direction = SortDirection.Auto)
    {
        _sortByAscending = direction switch
        {
            SortDirection.Ascending => true,
            SortDirection.Descending => false,
            SortDirection.Auto => _sortByColumn != column || !_sortByAscending,
            _ => throw new NotSupportedException($"Unknown sort direction {direction}"),
        };

        _sortByColumn = column;

        StateHasChanged(); // We want to see the updated sort order in the header, even before the data query is completed
        return RefreshDataAsync();
    }

    /// <summary>
    /// Sorts the grid by the specified column <paramref name="title"/> found first. If the title is not found, nothing happens.
    /// </summary>
    /// <param name="title">The title of the column to sort by.</param>
    /// <param name="direction">The direction of sorting. The default is <see cref="SortDirection.Auto"/>. If the value is <see cref="SortDirection.Auto"/>, then it will toggle the direction on each call.</param>
    public Task SortByColumnAsync(string title, SortDirection direction = SortDirection.Auto)
    {
        var column = _columns.FirstOrDefault(c => c.Title?.Equals(title, StringComparison.InvariantCultureIgnoreCase) ?? false);

        if (column is not null)
        {
            return SortByColumnAsync(column, direction);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Sorts the grid by the specified column <paramref name="index"/>. If the index is out of range, nothing happens.
    /// </summary>
    /// <param name="index">The index of the column to sort by.</param>
    /// <param name="direction">The direction of sorting. The default is <see cref="SortDirection.Auto"/>. If the value is <see cref="SortDirection.Auto"/>, then it will toggle the direction on each call.</param>
    public Task SortByColumnAsync(int index, SortDirection direction = SortDirection.Auto)
    {
        if (index >= 0 && index < _columns.Count)
        {
            return SortByColumnAsync(_columns[index], direction);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Removes the grid's sort on double click if this is specified <paramref name="column"/> currently sorted on.
    /// </summary>
    /// <param name="column">The column to check against the current sorted on column.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task RemoveSortByColumnAsync(ColumnBase<TGridItem> column)
    {
        if (_sortByColumn == column && !column.IsDefaultSortColumn)
        {
            _sortByColumn = _internalGridContext.DefaultSortColumn.Column ?? null;
            _sortByAscending = _internalGridContext.DefaultSortColumn.Direction != SortDirection.Descending;
        }

        StateHasChanged(); // We want to see the updated sort order in the header, even before the data query is completed
        return RefreshDataCoreAsync();
    }

    /// <summary>
    /// Displays the <see cref="ColumnBase{TGridItem}.ColumnOptions"/> UI for the specified column, closing any other column
    /// options UI that was previously displayed.
    /// </summary>
    /// <param name="column">The column whose options are to be displayed, if any are available.</param>
    public Task ShowColumnOptionsAsync(ColumnBase<TGridItem> column)
    {
        _displayOptionsForColumn = column;
        _checkColumnOptionsPosition = true; // Triggers a call to JSRuntime to position the options element, apply autofocus, and any other setup
        StateHasChanged();
        return Task.CompletedTask;
    }

    public void SetLoadingState(bool loading)
    {
        Loading = loading;
    }

    /// <summary>
    /// Instructs the grid to re-fetch and render the current data from the supplied data source
    /// (either <see cref="Items"/> or <see cref="ItemsProvider"/>).
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the completion of the operation.</returns>
    public async Task RefreshDataAsync()
    {
        await RefreshDataCoreAsync();
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
            var startIndex = Pagination is null ? 0 : (Pagination.CurrentPageIndex * Pagination.ItemsPerPage);
            GridItemsProviderRequest<TGridItem> request = new(
                startIndex, Pagination?.ItemsPerPage, _sortByColumn, _sortByAscending, thisLoadCts.Token);
            var result = await ResolveItemsRequestAsync(request);
            if (!thisLoadCts.IsCancellationRequested)
            {
                _internalGridContext.Items = result.Items;
                _internalGridContext.TotalItemCount = result.TotalItemCount;
                Pagination?.SetTotalItemCountAsync(_internalGridContext.TotalItemCount);
                _pendingDataLoadCancellationTokenSource = null;
            }
            _internalGridContext.ResetRowIndexes(startIndex);
            _lastRefreshedPaginationStateHash = Pagination?.GetHashCode();
        }

        StateHasChanged();
    }

    // Gets called both by RefreshDataCoreAsync and directly by the Virtualize child component during scrolling
    private async ValueTask<ItemsProviderResult<(int, TGridItem)>> ProvideVirtualizedItemsAsync(ItemsProviderRequest request)
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

        GridItemsProviderRequest<TGridItem> providerRequest = new(
            startIndex, count, _sortByColumn, _sortByAscending, request.CancellationToken);
        var providerResult = await ResolveItemsRequestAsync(providerRequest);

        if (!request.CancellationToken.IsCancellationRequested)
        {
            // ARIA's rowcount is part of the UI, so it should reflect what the human user regards as the number of rows in the table,
            // not the number of physical <tr> elements. For virtualization this means what's in the entire scrollable range, not just
            // the current viewport. In the case where you're also paginating then it means what's conceptually on the current page.
            // TODO: This currently assumes we always want to expand the last page to have ItemsPerPage rows, but the experience might
            //       be better if we let the last page only be as big as its number of actual rows.
            _internalGridContext.TotalItemCount = Pagination is null ? providerResult.TotalItemCount : Pagination.ItemsPerPage;

            Pagination?.SetTotalItemCountAsync(_internalGridContext.TotalItemCount);
            if (_internalGridContext.TotalItemCount > 0)
            {
                Loading = false;
            }

            // We're supplying the row _index along with each row's data because we need it for aria-rowindex, and we have to account for
            // the virtualized start _index. It might be more performant just to have some _latestQueryRowStartIndex field, but we'd have
            // to make sure it doesn't get out of sync with the rows being rendered.
            return new ItemsProviderResult<(int, TGridItem)>(
                 items: providerResult.Items.Select((x, i) => ValueTuple.Create(i + request.StartIndex + 2, x)),
                 totalItemCount: _internalGridContext.TotalItemCount);
        }

        return default;
    }

    // Normalizes all the different ways of configuring a data source so they have common GridItemsProvider-shaped API
    private async ValueTask<GridItemsProviderResult<TGridItem>> ResolveItemsRequestAsync(GridItemsProviderRequest<TGridItem> request)
    {
        if (ItemsProvider is not null)
        {
            var gipr = await ItemsProvider(request);
            if (gipr.Items is not null)
            {
                Loading = false;
            }
            return gipr;
        }
        else if (Items is not null)
        {
            var totalItemCount = _asyncQueryExecutor is null ? Items.Count() : await _asyncQueryExecutor.CountAsync(Items);
            _internalGridContext.TotalItemCount = totalItemCount;
            var result = request.ApplySorting(Items).Skip(request.StartIndex);
            if (request.Count.HasValue)
            {
                result = result.Take(request.Count.Value);
            }
            var resultArray = _asyncQueryExecutor is null ? [.. result] : await _asyncQueryExecutor.ToArrayAsync(result);
            return GridItemsProviderResult.From(resultArray, totalItemCount);
        }
        else
        {
            return GridItemsProviderResult.From(Array.Empty<TGridItem>(), 0);
        }
    }

    private string AriaSortValue(ColumnBase<TGridItem> column)
         => _sortByColumn == column
             ? (_sortByAscending ? "ascending" : "descending")
             : "none";

    private string? ColumnHeaderClass(ColumnBase<TGridItem> column)
        => _sortByColumn == column
        ? $"{ColumnClass(column)} {(_sortByAscending ? "col-sort-asc" : "col-sort-desc")}"
        : ColumnClass(column);

    private string? GridClass()
    {
        var value = $"{Class} {(_pendingDataLoadCancellationTokenSource is null ? null : "loading")}".Trim();
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }
        else
        {
            return value;
        }
    }

    private static string? ColumnClass(ColumnBase<TGridItem> column) => column.Align switch
    {
        Align.Start => $"col-justify-start {column.Class}",
        Align.Center => $"col-justify-center {column.Class}",
        Align.End => $"col-justify-end {column.Class}",
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

            if (Module is not null)
            {
                await Module.DisposeAsync();
            }
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }

    private void CloseColumnOptions()
    {
        _displayOptionsForColumn = null;
        StateHasChanged();
    }

    private async Task HandleOnRowFocusAsync(DataGridRowFocusEventArgs args)
    {
        var rowId = args.RowId;
        if (_internalGridContext.Rows.TryGetValue(rowId!, out var row))
        {
            if (row != null && row.RowType == DataGridRowType.Default)
            {
                await OnRowFocus.InvokeAsync(row);
            }
        }
    }

    public async Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
    {
        if (args.ShiftKey == true && args.Key == KeyCode.KeyR)
        {
            await ResetColumnWidthsAsync();
        }

        if (args.Value == "-")
        {
            await SetColumnWidthDiscreteAsync(null, -10);
        }
        if (args.Value == "+")
        {
            //  Resize column up
            await SetColumnWidthDiscreteAsync(null, 10);
        }
        //return Task.CompletedTask;
    }

    internal async Task SetColumnWidthDiscreteAsync(int? columnIndex, float widthChange)
    {
        if (_gridReference is not null && Module is not null)
        {
            await Module.InvokeVoidAsync("resizeColumnDiscrete", _gridReference, columnIndex, widthChange);
        }
    }

    internal async Task SetColumnWidthExactAsync(int columnIndex, int width)
    {
        if (_gridReference is not null && Module is not null)
        {
            await Module.InvokeVoidAsync("resizeColumnExact", _gridReference, columnIndex, width);
        }
    }

    internal async Task ResetColumnWidthsAsync()
    {
        if (_gridReference is not null && Module is not null)
        {
            await Module.InvokeVoidAsync("resetColumnWidths", _gridReference);
        }
    }

}
