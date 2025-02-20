// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Infrastructure;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A component that displays a grid.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public partial class FluentDataGrid<TGridItem> : FluentComponentBase, IHandleEvent, IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/DataGrid/FluentDataGrid.razor.js";
    public const string EMPTY_CONTENT_ROW_CLASS = "empty-content-row";
    public const string LOADING_CONTENT_ROW_CLASS = "loading-content-row";

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IServiceScopeFactory ScopeFactory { get; set; } = default!;

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
    /// This is applicable only when using <see cref="Virtualize"/>. It defines how many additional items will be rendered
    /// before and after the visible region to reduce rendering frequency during scrolling. While higher values can improve
    /// scroll smoothness by rendering more items off-screen, they can also increase initial load times. Finding a balance
    /// based on your data set size and user experience requirements is recommended. The default value is 3.
    /// </summary>
    [Parameter]
    public int OverscanCount { get; set; } = 3;

    /// <summary>
    /// This is applicable only when using <see cref="Virtualize"/>. It defines an expected height in pixels for
    /// each row, allowing the virtualization mechanism to fetch the correct number of items to match the display
    /// size and to ensure accurate scrolling.
    /// </summary>
    [Parameter]
    public float ItemSize { get; set; } = 32;

    /// <summary>
    /// If true, renders draggable handles around the column headers and adds a button to invoke a resize UI.
    /// This allows the user to resize columns manually. Size changes are not persisted.
    /// </summary>
    [Parameter]
    public bool ResizableColumns { get; set; }

    /// <summary>
    /// To comply with WCAG 2.2, a one-click option should be offered to change column widths. We provide such an option through the
    /// ColumnOptions UI. This parameter allows you to enable or disable this resize UI.Enable it by setting the type of resize to perform
    /// Discrete: resize by a 10 pixels at a time
    /// Exact: resize to the exact width specified (in pixels)
    /// Note: This does not affect resizing by mouse dragging, just the keyboard driven resize.
    /// </summary>
    [Parameter]
    public DataGridResizeType? ResizeType { get; set; }

    /// <summary>
    /// (Aria) Labels used in the column resize UI.
    /// </summary>
    [Parameter]
    public ColumnResizeLabels ColumnResizeLabels { get; set; } = ColumnResizeLabels.Default;

    /// <summary>
    /// Labels used in the column sort UI.
    /// </summary>
    [Parameter]
    public ColumnSortLabels ColumnSortLabels { get; set; } = ColumnSortLabels.Default;

    /// <summary>
    /// Labels used in the column options UI.
    /// </summary>
    [Parameter]
    public ColumnOptionsLabels ColumnOptionsLabels { get; set; } = ColumnOptionsLabels.Default;

    /// <summary>
    ///  If true, enables the new style of header cell that includes a button to display all column options through a menu.
    /// </summary>
    [Parameter]
    public bool HeaderCellAsButtonWithMenu { get; set; }

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
    /// As of 4.11 a row is a tr element with a 'display: contents'. Browsers can not focus such elements currently, but work is underway to fix that.
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
    /// Gets or sets a value to indicate the grid loading data state.
    /// If not set and a <see cref="ItemsProvider"/> is present, the grid will show <see cref="LoadingContent"/> until the provider's first return.
    /// </summary>
    [Parameter]
    public bool? Loading { get; set; }

    /// <summary>
    /// Gets or sets the content to render when <see cref="Loading"/> is true.
    /// A default fragment is used if loading content is not specified.
    /// </summary>
    [Parameter]
    public RenderFragment? LoadingContent { get; set; }

    /// <summary>
    /// Sets <see cref="GridTemplateColumns"/> to automatically fit the columns to the available width as best it can.
    /// </summary>
    [Parameter]
    public bool AutoFit { get; set; }

    /// <summary>
    /// Automatically fit the number of items per page to the available height.
    /// </summary>
    [Parameter]
    public bool AutoItemsPerPage { get; set; }

    [Parameter]
    public DataGridDisplayMode DisplayMode { get; set; } = DataGridDisplayMode.Grid;

    /// <summary>
    /// Gets or sets the size of each row in the grid based on the <see cref="DataGridRowSize"/> enum.
    /// </summary>
    [Parameter]
    public DataGridRowSize RowSize { get; set; } = DataGridRowSize.Small;

    /// <summary>
    /// Gets or sets a value indicating whether the grid should allow multiple lines of text in cells.
    /// </summary>
    [Parameter]
    public bool MultiLine { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the grid should save its paging state in the URL.
    /// <para>This is an experimental feature, which might cause unwanted jumping in the page when you change something in the grid.</para>
    /// </summary>
    [Parameter]
    public bool SaveStateInUrl { get; set; }

    /// <summary>
    /// Gets or sets a prefix to use when saving the grid state in the URL.
    /// </summary>
    /// <remarks>Only relevant when <see cref="SaveStateInUrl"/> is set to <see langword="true"/> on multiple grids on a single page.</remarks>
    [Parameter]
    public string? SaveStatePrefix { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the grids' first cell should be focused.
    /// </summary>
    [Parameter]
    public bool AutoFocus { get; set; } = false;

    // Returns Loading if set (controlled). If not controlled,
    // we assume the grid is loading until the next data load completes
    internal bool EffectiveLoadingValue => Loading ?? ItemsProvider is not null;

    private ElementReference? _gridReference;
    //private DotNetObjectReference<Type>? _dotNetObjectReference;
    private Virtualize<(int, TGridItem)>? _virtualizeComponent;

    // IQueryable only exposes synchronous query APIs. IAsyncQueryExecutor is an adapter that lets us invoke any
    // async query APIs that might be available. We have built-in support for using EF Core's async query APIs.
    private IAsyncQueryExecutor? _asyncQueryExecutor;
    private AsyncServiceScope? _scope;

    // We cascade the InternalGridContext to descendants, which in turn call it to add themselves to _columns
    // This happens on every render so that the column list can be updated dynamically
    private readonly InternalGridContext<TGridItem> _internalGridContext;
    internal readonly List<ColumnBase<TGridItem>> _columns;
    private bool _collectingColumns;// Columns might re-render themselves arbitrarily. We only want to capture them at a defined time.

    // Tracking state for options and sorting
    private ColumnBase<TGridItem>? _displayOptionsForColumn;
    private ColumnBase<TGridItem>? _displayResizeForColumn;
    private ColumnBase<TGridItem>? _sortByColumn;
    private bool _sortByAscending;
    private bool _checkColumnOptionsPosition;
    private bool _checkColumnResizePosition;
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
    private PaginationState? _lastRefreshedPaginationState;
    private IQueryable<TGridItem>? _lastAssignedItems;
    private GridItemsProvider<TGridItem>? _lastAssignedItemsProvider;
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
        Id = Identifier.NewId();
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
        if (SaveStateInUrl)
        {
            LoadStateFromQueryString(new Uri(NavigationManager.Uri).Query);
        }
    }

    /// <inheritdoc />
    protected override Task OnParametersSetAsync()
    {
        // The associated pagination state may have been added/removed/replaced
        _currentPageItemsChanged.SubscribeOrMove(Pagination?.CurrentPageItemsChanged);

        if (Items is not null && ItemsProvider is not null)
        {
            throw new InvalidOperationException($"FluentDataGrid requires one of {nameof(Items)} or {nameof(ItemsProvider)}, but both were specified.");
        }

        // Perform a re-query only if the data source or something else has changed
        var dataSourceHasChanged = !Equals(Items, _lastAssignedItems) || !Equals(ItemsProvider, _lastAssignedItemsProvider);
        if (dataSourceHasChanged)
        {
            _scope?.Dispose();
            _scope = ScopeFactory.CreateAsyncScope();
            _lastAssignedItemsProvider = ItemsProvider;
            _lastAssignedItems = Items;
            _asyncQueryExecutor = AsyncQueryExecutorSupplier.GetAsyncQueryExecutor(_scope.Value.ServiceProvider, Items);
        }

        var paginationStateHasChanged =
            Pagination?.ItemsPerPage != _lastRefreshedPaginationState?.ItemsPerPage
            || Pagination?.CurrentPageIndex != _lastRefreshedPaginationState?.CurrentPageIndex;

        var mustRefreshData = dataSourceHasChanged || paginationStateHasChanged || EffectiveLoadingValue;

        // We don't want to trigger the first data load until we've collected the initial set of columns,
        // because they might perform some action like setting the default sort order, so it would be wasteful
        // to have to re-query immediately
        return (_columns.Count > 0 && mustRefreshData) ? RefreshDataCoreAsync() : Task.CompletedTask;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && _gridReference is not null)
        {
            Element = _gridReference.Value;
            Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
            try
            {
                _jsEventDisposable = await Module.InvokeAsync<IJSObjectReference>("init", _gridReference, AutoFocus);
                if (AutoItemsPerPage)
                {
                    await Module.InvokeVoidAsync("dynamicItemsPerPage", _gridReference, DotNetObjectReference.Create(this), (int)RowSize);
                }
            }
            catch (JSException ex)
            {
                Console.WriteLine("[FluentDataGrid] " + ex.Message);
            }
        }

        SaveStateToQueryString();

        if (_checkColumnOptionsPosition && _displayOptionsForColumn is not null)
        {
            _checkColumnOptionsPosition = false;
            Module?.InvokeVoidAsync("checkColumnPopupPosition", _gridReference, ".col-options").AsTask();
        }

        if (_checkColumnResizePosition && _displayResizeForColumn is not null)
        {
            _checkColumnResizePosition = false;
            _ = Module?.InvokeVoidAsync("checkColumnPopupPosition", _gridReference, ".col-resize").AsTask();
        }

        if (AutoFit && _gridReference is not null)
        {
            _ = Module?.InvokeVoidAsync("autoFitGridColumns", _gridReference, _columns.Count).AsTask();
        }
    }

    // Invoked by descendant columns at a special time during rendering
    internal void AddColumn(ColumnBase<TGridItem> column, SortDirection? initialSortDirection, bool isDefaultSortColumn)
    {
        if (_collectingColumns)
        {
            column.Index = _columns.Count + 1;
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

        // Always re-evaluate after collecting columns when using displaymode grid. A column might be added or hidden and the _internalGridTemplateColumns needs to reflect that.
        if (DisplayMode == DataGridDisplayMode.Grid)
        {
            if (!AutoFit)
            {
                _internalGridTemplateColumns = GridTemplateColumns ?? string.Join(" ", Enumerable.Repeat("1fr", _columns.Count));
            }

            if (_columns.Any(x => !string.IsNullOrWhiteSpace(x.Width)))
            {
                _internalGridTemplateColumns = GridTemplateColumns ?? string.Join(" ", _columns.Select(x => x.Width ?? "auto"));
            }
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
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task SortByColumnAsync(string title, SortDirection direction = SortDirection.Auto)
    {
        var column = _columns.FirstOrDefault(c => c.Title?.Equals(title, StringComparison.InvariantCultureIgnoreCase) ?? false);

        return column is not null ? SortByColumnAsync(column, direction) : Task.CompletedTask;
    }

    /// <summary>
    /// Sorts the grid by the specified column <paramref name="index"/>. If the index is out of range, nothing happens.
    /// </summary>
    /// <param name="index">The index of the column to sort by.</param>
    /// <param name="direction">The direction of sorting. The default is <see cref="SortDirection.Auto"/>. If the value is <see cref="SortDirection.Auto"/>, then it will toggle the direction on each call.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task SortByColumnAsync(int index, SortDirection direction = SortDirection.Auto)
    {
        return index >= 0 && index < _columns.Count ? SortByColumnAsync(_columns[index], direction) : Task.CompletedTask;
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

            StateHasChanged(); // We want to see the updated sort order in the header, even before the data query is completed
            return RefreshDataCoreAsync();
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Removes the grid's sort on double click for the currently sorted column if it's not a default sort column.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task RemoveSortByColumnAsync() => (_sortByColumn != null) ? RemoveSortByColumnAsync(_sortByColumn) : Task.CompletedTask;

    /// <summary>
    /// Displays the <see cref="ColumnBase{TGridItem}.ColumnOptions"/> UI for the specified column, closing any other column
    /// options UI that was previously displayed.
    /// </summary>
    /// <param name="column">The column whose options are to be displayed, if any are available.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task ShowColumnOptionsAsync(ColumnBase<TGridItem> column)
    {
        _displayOptionsForColumn = column;
        _checkColumnOptionsPosition = true; // Triggers a call to JSRuntime to position the options element, apply autofocus, and any other setup
        StateHasChanged();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Displays the <see cref="ColumnBase{TGridItem}.ColumnOptions"/> UI for the specified column <paramref name="title"/> found first,
    /// closing any other column options UI that was previously displayed. If the title is not found, nothing happens.
    /// </summary>
    /// <param name="title">The column title whose options UI is to be displayed.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task ShowColumnOptionsAsync(string title)
    {
        var column = _columns.FirstOrDefault(c => c.Title?.Equals(title, StringComparison.InvariantCultureIgnoreCase) ?? false);
        return (column is not null) ? ShowColumnOptionsAsync(column) : Task.CompletedTask;
    }

    /// <summary>
    /// Displays the <see cref="ColumnBase{TGridItem}.ColumnOptions"/> UI for the specified column <paramref name="index"/>,
    /// closing any other column options UI that was previously displayed. If the index is out of range, nothing happens.
    /// </summary>
    /// <param name="index">The column index whose options UI is to be displayed.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task ShowColumnOptionsAsync(int index)
    {
        return (index >= 0 && index < _columns.Count) ? ShowColumnOptionsAsync(_columns[index]) : Task.CompletedTask;
    }

    /// <summary>
    /// Displays the column resize UI for the specified column, closing any other column
    /// resize UI that was previously displayed.
    /// </summary>
    /// <param name="column">The column whose resize UI is to be displayed.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task ShowColumnResizeAsync(ColumnBase<TGridItem> column)
    {
        _displayResizeForColumn = column;
        _checkColumnResizePosition = true; // Triggers a call to JSRuntime to position the options element, apply autofocus, and any other setup
        StateHasChanged();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Displays the column resize UI for the specified column, closing any other column
    /// resize UI that was previously displayed.
    /// </summary>
    /// <param name="title">The column title whose resize UI is to be displayed.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task ShowColumnResizeAsync(string title)
    {
        var column = _columns.FirstOrDefault(c => c.Title?.Equals(title, StringComparison.InvariantCultureIgnoreCase) ?? false);
        return (column is not null) ? ShowColumnResizeAsync(column) : Task.CompletedTask;
    }

    /// <summary>
    /// Displays the column resize UI for the specified column, closing any other column
    /// resize UI that was previously displayed.
    /// </summary>
    /// <param name="index">The column index whose resize UI is to be displayed.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task ShowColumnResizeAsync(int index)
    {
        return (index >= 0 && index < _columns.Count) ? ShowColumnResizeAsync(_columns[index]) : Task.CompletedTask;
    }

    public void SetLoadingState(bool? loading)
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
            _lastRefreshedPaginationState = Pagination;
            var result = await ResolveItemsRequestAsync(request);
            if (!thisLoadCts.IsCancellationRequested)
            {
                _internalGridContext.Items = result.Items;
                _internalGridContext.TotalItemCount = result.TotalItemCount;
                Pagination?.SetTotalItemCountAsync(_internalGridContext.TotalItemCount);
                _pendingDataLoadCancellationTokenSource = null;
            }
            _internalGridContext.ResetRowIndexes(startIndex);
        }

        StateHasChanged();
    }

    // Gets called both by RefreshDataCoreAsync and directly by the Virtualize child component during scrolling
    private async ValueTask<ItemsProviderResult<(int, TGridItem)>> ProvideVirtualizedItemsAsync(ItemsProviderRequest request)
    {
        _lastRefreshedPaginationState = Pagination;

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
            _internalGridContext.TotalItemCount = providerResult.TotalItemCount;
            _internalGridContext.TotalViewItemCount = Pagination?.ItemsPerPage ?? providerResult.TotalItemCount;

            Pagination?.SetTotalItemCountAsync(_internalGridContext.TotalItemCount);
            if (_internalGridContext.TotalItemCount > 0 && Loading is null)
            {
                Loading = false;
                StateHasChanged();
            }

            // We're supplying the row _index along with each row's data because we need it for aria-rowindex, and we have to account for
            // the virtualized start _index. It might be more performant just to have some _latestQueryRowStartIndex field, but we'd have
            // to make sure it doesn't get out of sync with the rows being rendered.
            return new ItemsProviderResult<(int, TGridItem)>(
                 items: providerResult.Items.Select((x, i) => ValueTuple.Create(i + request.StartIndex + 2, x)),
                 totalItemCount: _internalGridContext.TotalViewItemCount);
        }

        return default;
    }

    // Normalizes all the different ways of configuring a data source so they have common GridItemsProvider-shaped API
    private async ValueTask<GridItemsProviderResult<TGridItem>> ResolveItemsRequestAsync(GridItemsProviderRequest<TGridItem> request)
    {
        try
        {
            if (ItemsProvider is not null)
            {
                var gipr = await ItemsProvider(request);
                if (gipr.Items is not null && Loading is null)
                {
                    Loading = false;
                    StateHasChanged();
                }
                return gipr;
            }
            else if (Items is not null)
            {
                var totalItemCount = _asyncQueryExecutor is null ? Items.Count() : await _asyncQueryExecutor.CountAsync(Items, request.CancellationToken);
                _internalGridContext.TotalItemCount = totalItemCount;
                var result = request.ApplySorting(Items).Skip(request.StartIndex);
                if (request.Count.HasValue)
                {
                    result = result.Take(request.Count.Value);
                }
                var resultArray = _asyncQueryExecutor is null ? [.. result] : await _asyncQueryExecutor.ToArrayAsync(result, request.CancellationToken);
                return GridItemsProviderResult.From(resultArray, totalItemCount);
            }
        }
        catch (OperationCanceledException oce) when (oce.CancellationToken == request.CancellationToken)
        {
            // No-op; we canceled the operation, so it's fine to suppress this exception.
        }
        return GridItemsProviderResult.From(Array.Empty<TGridItem>(), 0);
    }

    private string AriaSortValue(ColumnBase<TGridItem> column)
         => _sortByColumn == column
             ? (_sortByAscending ? "ascending" : "descending")
             : "none";

    private string? StyleValue => new StyleBuilder(Style)
        .AddStyle("grid-template-columns", _internalGridTemplateColumns, !string.IsNullOrWhiteSpace(_internalGridTemplateColumns) && DisplayMode == DataGridDisplayMode.Grid)
        .AddStyle("grid-template-rows", "auto 1fr", _internalGridContext.Items.Count == 0 || Items is null)
        .AddStyle("height", $"calc(100% - {(int)RowSize}px)", _internalGridContext.TotalItemCount == 0 || EffectiveLoadingValue)
        .AddStyle("border-collapse", "separate", GenerateHeader == GenerateHeaderOption.Sticky)
        .AddStyle("border-spacing", "0", GenerateHeader == GenerateHeaderOption.Sticky)
        .Build();

    private string? ColumnHeaderClass(ColumnBase<TGridItem> column)
    {
        return new CssBuilder(Class)
           .AddClass(ColumnJustifyClass(column))
           .AddClass("col-sort-asc", _sortByAscending && column.IsActiveSortColumn)
           .AddClass("col-sort-desc", !_sortByAscending && column.IsActiveSortColumn)
           .Build();
    }

    private string? GridClass()
    {
        return new CssBuilder(Class)
            .AddClass("fluent-data-grid")
            .AddClass("grid", DisplayMode == DataGridDisplayMode.Grid)
            .AddClass("auto-fit", AutoFit)
            .AddClass("loading", _pendingDataLoadCancellationTokenSource is not null)
            .Build();
    }

    private static string? ColumnJustifyClass(ColumnBase<TGridItem> column)
    {
        return new CssBuilder(column.Class)
            .AddClass("col-justify-start", column.Align == Align.Start)
            .AddClass("col-justify-center", column.Align == Align.Center)
            .AddClass("col-justify-end", column.Align == Align.End)
            .Build();
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        _currentPageItemsChanged.Dispose();
        _scope?.Dispose();

        try
        {
            if (_jsEventDisposable is not null)
            {
                await _jsEventDisposable.InvokeVoidAsync("stop");
                await _jsEventDisposable.DisposeAsync().ConfigureAwait(false);
            }

            if (Module is not null)
            {
                await Module.DisposeAsync().ConfigureAwait(false);
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

    private void CloseColumnResize()
    {
        _displayResizeForColumn = null;
        StateHasChanged();
    }

    private void LoadStateFromQueryString(string queryString)
    {
        if (!SaveStateInUrl)
        {
            return;
        }

        var query = System.Web.HttpUtility.ParseQueryString(queryString);
        if (query.AllKeys.Contains($"{SaveStatePrefix}orderby"))
        {
            var orderBy = query[$"{SaveStatePrefix}orderby"]!.Split(' ', 2);
            var title = orderBy[0];

            var column = _columns.FirstOrDefault(c => c.Title == title);
            if (column is not null)
            {
                _sortByColumn = column;
                _sortByAscending = orderBy.Length == 2 && orderBy[1] == "asc";
            }
        }

        if (Pagination is not null)
        {
            if (query.AllKeys.Contains($"{SaveStatePrefix}page") && int.TryParse(query[$"{SaveStatePrefix}page"]!, out var page))
            {
                Pagination.SetCurrentPageIndexAsync(page - 1);
            }

            if (query.AllKeys.Contains($"{SaveStatePrefix}top") && int.TryParse(query[$"{SaveStatePrefix}top"]!, out var itemsPerPage))
            {
                Pagination.ItemsPerPage = itemsPerPage;
            }
        }
    }

    private void SaveStateToQueryString()
    {
        if (!SaveStateInUrl)
        {
            return;
        }

        var stateParams = new Dictionary<string, object?>();
        if (_sortByColumn is not null)
        {
            var order = _sortByAscending ? "asc" : "desc";
            stateParams.Add($"{SaveStatePrefix}orderby", $"{_sortByColumn.Title} {order}");
        }
        stateParams.Add($"{SaveStatePrefix}page", Pagination?.CurrentPageIndex + 1 ?? null);
        stateParams.Add($"{SaveStatePrefix}top", Pagination?.ItemsPerPage ?? null);
        NavigationManager.NavigateTo(NavigationManager.GetUriWithQueryParameters(stateParams), replace: true);
    }

    /// <summary>
    /// Updates the <see cref="Pagination"/>s ItemPerPage parameter.
    /// Guards the CurrentPageIndex from getting greater than the LastPageIndex
    ///
    /// </summary>
    /// <param name="visibleRows">The maixmum number of rows that fits the available space</param>
    /// <returns></returns>
    [JSInvokable]
    public async Task UpdateItemsPerPageAsync(int visibleRows)
    {
        if (Pagination is null)
        {
            return;
        }

        if (visibleRows < 2)
        {
            visibleRows = 2;
        }

        await Pagination.SetItemsPerPageAsync(visibleRows - 1); // subtract 1 for the table header

        //if (Pagination.CurrentPageIndex > Pagination.LastPageIndex && Pagination.LastPageIndex.HasValue && Pagination.LastPageIndex.Value > 0)
        //{
        //    await Pagination.SetCurrentPageIndexAsync(Pagination.LastPageIndex.Value);
        //}

        //await RefreshDataAsync();
        //StateHasChanged();
    }

    //public void SetPageReference(Type page)
    //{
    //    _dotNetObjectReference = DotNetObjectReference.Create(page);
    //}

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

    /// <summary>
    /// Resizes the column width by a discrete amount.
    /// </summary>
    /// <param name="columnIndex">The column to be resized</param>
    /// <param name="widthChange">The amount of pixels to change width with</param>
    /// <returns></returns>
    public async Task SetColumnWidthDiscreteAsync(int? columnIndex, float widthChange)
    {
        if (_gridReference is not null && Module is not null)
        {
            await Module.InvokeVoidAsync("resizeColumnDiscrete", _gridReference, columnIndex, widthChange);
        }
    }

    /// <summary>
    /// Resizes the column width to the exact width specified (in pixels).
    /// </summary>
    /// <param name="columnIndex">The column to be resized</param>
    /// <param name="width">The new width in pixels</param>
    /// <returns></returns>
    public async Task SetColumnWidthExactAsync(int columnIndex, int width)
    {
        if (_gridReference is not null && Module is not null)
        {
            await Module.InvokeVoidAsync("resizeColumnExact", _gridReference, columnIndex, width);
        }
    }

    /// <summary>
    /// Resets the column widths to their initial values as specified with the <see cref="GridTemplateColumns"/> parameter.
    /// If no value is specified, the default value is "1fr" for each column.
    /// </summary>
    /// <returns></returns>
    public async Task ResetColumnWidthsAsync()
    {
        if (_gridReference is not null && Module is not null)
        {
            await Module.InvokeVoidAsync("resetColumnWidths", _gridReference);
        }
    }
}
