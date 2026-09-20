// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using Microsoft.FluentUI.AspNetCore.Components.Infrastructure;
using Microsoft.FluentUI.AspNetCore.Components.Localization;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A component that displays a grid.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]

[SuppressMessage("Usage", "MA0040:Forward the CancellationToken parameter to methods that take one", Justification = "The available cancellation token are not appropriate to pass along.")]
public partial class FluentDataGrid<TGridItem> : FluentComponentBase, IHandleEvent
{
    private enum ColumnHeaderUiKind
    {
        None,
        All,
        Options,
        Reorder,
        Resize,
        Sort,
    }

#if NET11_0_OR_GREATER
    // Adapts ItemComparer to the virtualizer's (row index, item) shape while comparing item identity only.
    // Reading from the owner on each call ensures parameter updates take effect.
    private sealed class VirtualizeItemComparer(FluentDataGrid<TGridItem> owner) : IEqualityComparer<(int, TGridItem)>
    {
        private readonly FluentDataGrid<TGridItem> _owner = owner;

        private IEqualityComparer<TGridItem> ItemComparer => _owner._itemComparer ?? EqualityComparer<TGridItem>.Default;

        public bool Equals((int, TGridItem) first, (int, TGridItem) second)
        {
            return ItemComparer.Equals(first.Item2, second.Item2);
        }

        public int GetHashCode((int, TGridItem) item)
        {
            return item.Item2 is null ? 0 : ItemComparer.GetHashCode(item.Item2);
        }
    }
#endif

    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "DataGrid/FluentDataGrid.razor.js";
    private static readonly TimeSpan _virtualizeRequestBurstInterval = TimeSpan.FromMilliseconds(100);

    private ElementReference? _gridReference;
    private IJSObjectReference? _gridController;
    private Virtualize<(int, TGridItem)>? _virtualizeComponent;
#if NET11_0_OR_GREATER
    private IEqualityComparer<TGridItem>? _itemComparer;
    private readonly IEqualityComparer<(int, TGridItem)> _virtualizeItemComparer;
#endif
    private IAsyncQueryExecutor? _asyncQueryExecutor;
    private AsyncServiceScope? _scope;
    private bool _asyncQueryExecuted;
    private readonly InternalGridContext<TGridItem> _internalGridContext;
    internal readonly List<ColumnBase<TGridItem>> _columns;
    private bool _collectingColumns;
    private ColumnBase<TGridItem>? _activeHeaderUiColumn;
    private ColumnHeaderUiKind _activeHeaderUiKind;
    private ColumnBase<TGridItem>? _pendingHeaderUiReopenColumn;
    private ColumnBase<TGridItem>? _restoreHeaderUiFocusColumn;
    private ColumnHeaderUiKind _pendingHeaderUiReopenKind;
    // The columns the grid is sorted by, in priority order. Holds at most one entry unless SortMode is Multiple.
    private readonly List<DataGridSortColumn<TGridItem>> _sortColumns = [];
    // The sort declared by the columns (IsDefaultSortColumn), restored when the user clears the sort.
    private readonly List<DataGridSortColumn<TGridItem>> _defaultSortColumns = [];
    private bool _defaultSortApplied;
    private List<(string Title, bool Ascending)>? _pendingSortStateFromUrl;
    private string? _sortAnnouncement;
    private bool _checkColumnHeaderUiPosition;
    private bool _checkColumnResizing;
    private bool _checkColumnReordering;
    private ColumnInteractionOptions? _columnInteractionOptions;
    private readonly List<(ColumnBase<TGridItem> Column, string Key, DataGridColumnPin Pin, string? Width, string? MinWidth)> _columnInteractionColumns = [];
    private bool _manualGrid;

    private readonly record struct ColumnInteractionOptions(
        bool Resizing,
        bool Reordering,
        bool AllRows,
        DataGridGeneratedHeaderType? Header,
        DataGridDisplayMode DisplayMode,
        DataGridRowSize RowSize,
        bool MultiLine,
        string? Template);

    // Keys (as returned by ItemKey) of the rows whose RowDetails content is currently expanded
    private readonly HashSet<object> _expandedRowDetails = [];
    private readonly RenderFragment _renderColumnHeaders;
    private readonly RenderFragment _renderNonVirtualizedRows;
    private readonly RenderFragment _renderEmptyContent;
    private readonly RenderFragment _renderLoadingContent;
    private readonly RenderFragment _renderErrorContent;
    private readonly List<string> _columnOrder;
    private string? _internalGridTemplateColumns;
    private PaginationState? _lastRefreshedPaginationState;
    private IQueryable<TGridItem>? _lastAssignedItems;
    private bool? _lastVirtualizationMode;
    private GridItemsProvider<TGridItem>? _lastAssignedItemsProvider;
    private CancellationTokenSource? _pendingDataLoadCancellationTokenSource;
    private long _lastVirtualizeProviderRequestTimestamp;
    private bool _skipNextVirtualizeProviderDelay;
    private int _lastVirtualizeProviderTotalItemCount;

    // True once ProvideVirtualizedItemsAsync has applied a provider result. Gates the virtualized
    // empty content: "zero items" only means "no data" after the provider has actually answered,
    // not during the initial load window between the Virtualize mount and its first query (#5151).
    private bool _virtualizeItemsProvided;
    private bool _retainEmptyContentOnVirtualizedRefresh;
    private Exception? _lastError;
    private GridItemsProviderRequest<TGridItem>? _lastRequest;
    private bool _forceRefreshData;
    private DotNetObjectReference<FluentDataGrid<TGridItem>>? _selfReference;
    private readonly EventCallbackSubscriber<PaginationState> _currentPageItemsChanged;

    /// <summary />
    public FluentDataGrid(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
        _columns = [];
        _columnOrder = [];
        _internalGridContext = new(this);
        _currentPageItemsChanged = new(EventCallback.Factory.Create<PaginationState>(this, RefreshDataCoreAsync));
        _renderColumnHeaders = RenderColumnHeaders;
        _renderNonVirtualizedRows = RenderNonVirtualizedRows;
        _renderEmptyContent = RenderEmptyContent;
        _renderLoadingContent = RenderLoadingContent;
        _renderErrorContent = RenderErrorContent;
#if NET11_0_OR_GREATER
        _virtualizeItemComparer = new VirtualizeItemComparer(this);
#endif

        // As a special case, we don't issue the first data load request until we've collected the initial set of columns
        // This is so we can apply default sort order (or any future per-column options) before loading data
        // We use EventCallbackSubscriber to safely hook this async operation into the synchronous rendering flow
        EventCallbackSubscriber<object?>? columnsFirstCollectedSubscriber = new(
                EventCallback.Factory.Create<object?>(this, RefreshDataCoreAsync));
        columnsFirstCollectedSubscriber.SubscribeOrMove(_internalGridContext.ColumnsFirstCollected);
    }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IServiceScopeFactory ScopeFactory { get; set; } = default!;

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
    /// Gets or sets a callback which will be called if there is a change in pagination, ordering or if a RefreshDataAsync is forced.
    ///
    /// You must supply <see cref="Items"/> if you use this callback.
    /// </summary>
    [Parameter]
    public Func<GridItemsProviderRequest<TGridItem>, Task>? RefreshItems { get; set; }

    /// <summary>
    /// Gets or sets a callback that supplies data for the grid.
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

#if NET9_0_OR_GREATER
    /// <summary>
    /// This is applicable only when using <see cref="Virtualize"/>. It defines the maximum number of items
    /// that can be rendered, even when the viewport would otherwise require more items.
    /// </summary>
    [Parameter]
    public int MaxItemCount { get; set; } = 100;
#endif

    /// <summary>
    /// This is applicable only when using <see cref="Virtualize"/>. It defines an expected height in pixels for
    /// each row, allowing the virtualization mechanism to fetch the correct number of items to match the display
    /// size and to ensure accurate scrolling.
    /// </summary>
    [Parameter]
    public float ItemSize { get; set; } = 32;

#if NET11_0_OR_GREATER
    /// <summary>
    /// This is applicable only when using <see cref="Virtualize"/>. Gets or sets the zero-based index of the
    /// row to scroll to on first interactive render. Applied once when the grid first knows its item count and
    /// ignored on subsequent re-renders; to scroll programmatically at any later point, call
    /// <see cref="ScrollToItemAsync(int, CancellationToken)"/>. Out-of-range values are clamped. The default value,
    /// <c>0</c>, means no initial scroll.
    /// </summary>
    [Parameter]
    public int InitialItemIndex { get; set; }

    /// <summary>
    /// This is applicable only when using <see cref="Virtualize"/>. Gets or sets the anchor mode that controls
    /// how the viewport behaves at the edges of the list
    /// when new items arrive during virtualization. The default is <see cref="VirtualizeAnchorMode.Start"/>.
    /// </summary>
    [Parameter]
    public VirtualizeAnchorMode AnchorMode { get; set; } = VirtualizeAnchorMode.Start;

    /// <summary>
    /// This is applicable only when using <see cref="Virtualize"/>. Gets or sets a comparer used during
    /// virtualization to detect whether items were prepended or appended between data loads.
    /// Provide a comparer that compares items by a stable unique identifier.
    /// Defaults to <see cref="EqualityComparer{T}.Default"/>.
    /// </summary>
    [Parameter]
    public IEqualityComparer<TGridItem>? ItemComparer
    {
        get => _itemComparer;
        set => _itemComparer = value;
    }
#endif

    /// <summary>
    /// If true, renders draggable handles around the column headers and adds a button to invoke a resize UI.
    /// This allows the user to resize columns manually. Size changes are not persisted.
    /// </summary>
    [Parameter]
    public bool ResizableColumns { get; set; }

    /// <summary>
    /// If true, allows unpinned columns to be reordered by drag-and-drop and through the header popup UI.
    /// </summary>
    [Parameter]
    public bool ReorderableColumns { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether column resize handles should extend the full height of the grid.
    /// When true, columns can be resized by dragging from any row. When false, columns can only be resized
    /// by dragging from the column header. Default is true.
    /// </summary>
    [Parameter]
    public bool ResizeColumnOnAllRows { get; set; } = true;

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
    /// Settings (icon, icon position) used in the column resize UI.
    /// (aria) labels are controlled through <see cref="IFluentLocalizer"/>
    /// </summary>
    [Parameter]
    public ColumnMenuSettings ColumnResizeMenuSettings { get; set; } = new(new CoreIcons.Regular.Size20.TableResizeColumn(), LanguageResource.DataGrid_ResizeMenu);

    /// <summary>
    /// Settings (icon, icon position) used in the column reorder UI.
    /// (aria) labels are controlled through <see cref="IFluentLocalizer"/>
    /// </summary>
    [Parameter]
    public ColumnMenuSettings ColumnReorderMenuSettings { get; set; } = new(new CoreIcons.Regular.Size20.ColumnArrowRight(), LanguageResource.DataGrid_ReorderMenu);

    /// <summary>
    /// Settings (icon, icon position) used in the column sort UI.
    /// (aria) labels are controlled through <see cref="IFluentLocalizer"/>
    /// </summary>
    [Parameter]
    public ColumnMenuSettings ColumnSortMenuSettings { get; set; } = new(new CoreIcons.Regular.Size20.ArrowSort(), LanguageResource.DataGrid_SortMenu);

    /// <summary>
    /// Settings (icon, icon position) used in the column options menu. (aria) labels are controlled through
    /// <see cref="IFluentLocalizer"/>
    /// </summary>
    [Parameter]
    public ColumnMenuSettings ColumnOptionsMenuSettings { get; set; } = new(new CoreIcons.Regular.Size20.Filter(), LanguageResource.DataGrid_OptionsMenu);

    /// <summary>
    /// If true, enables the new style of header cell that includes a button to display all column options through a
    /// menu.
    /// </summary>
    [Parameter]
    public bool HeaderCellAsButtonWithMenu { get; set; }

    /// <summary>
    /// Use IMenuService to create the menu, if this service was injected.
    /// This value must be defined before the component is rendered (you can't change it during the component lifecycle).
    /// Default, true.
    /// </summary>
    [Parameter]
    public bool UseMenuService { get; set; } = true;

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
    /// Gets or sets a value indicating whether the grid should automatically generate a header row and its type.
    /// See <see cref="DataGridGeneratedHeaderType"/>
    /// </summary>
    [Parameter]
    public DataGridGeneratedHeaderType? GenerateHeader { get; set; } = DataGridGeneratedHeaderType.Default;

    /// <summary>
    /// Gets or sets the value that gets applied to the css gridTemplateColumns attribute of child rows.
    /// Can be specified here or on the column level with the Width parameter but not both.
    /// Needs to be a valid CSS string of space-separated values, such as "auto 1fr 2fr 100px".
    /// </summary>
    [Parameter]
    public string? GridTemplateColumns { get; set; } = null;

    /// <summary>
    /// Gets or sets the persisted display order for the grid columns.
    /// Pinned columns are ignored when applying this order and remain fixed at their respective edges.
    /// </summary>
    [Parameter]
    public IReadOnlyList<string>? ColumnOrder { get; set; }

    /// <summary>
    /// Gets or sets a callback that is invoked when the column order changes.
    /// </summary>
    [Parameter]
    public EventCallback<IReadOnlyList<string>> ColumnOrderChanged { get; set; }

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
    /// Event callback for when a hierarchical row is expanded or collapsed.
    /// </summary>
    [Parameter]
    public EventCallback<TGridItem> OnToggle { get; set; }

    /// <summary>
    /// Event callback for when all hierarchical rows are expanded.
    /// </summary>
    [Parameter]
    public EventCallback OnExpandAll { get; set; }

    /// <summary>
    /// Event callback for when all hierarchical rows are collapsed.
    /// </summary>
    [Parameter]
    public EventCallback OnCollapseAll { get; set; }

    /// <summary>
    /// Gets or sets the template that defines the detail content shown when a row is expanded (master/detail view).
    ///
    /// When set, each row displays an expand/collapse button in its first column. The expanded content is rendered
    /// in an extra row spanning all columns, directly below the master row. The template's context is the row's
    /// <typeparamref name="TGridItem"/>, so it can, for example, contain a child <see cref="FluentDataGrid{TGridItem}"/>
    /// whose items are filtered by the master row.
    ///
    /// Cannot be used together with <see cref="Virtualize"/>.
    /// </summary>
    [Parameter]
    public RenderFragment<TGridItem>? RowDetails { get; set; }

    /// <summary>
    /// Optionally determines, per row, whether that row has <see cref="RowDetails"/> content to show.
    /// When set, the expand/collapse toggle button is only rendered for rows where this returns
    /// <see langword="true"/> — other rows keep the same indentation but show no button, so their
    /// content can't be expanded via the UI. If not set, every row gets the toggle button.
    ///
    /// This only controls the toggle button; it doesn't affect <see cref="ToggleRowDetailsAsync"/> and the
    /// other programmatic expand/collapse methods, which work regardless.
    /// </summary>
    [Parameter]
    public Func<TGridItem, bool>? HasRowDetails { get; set; }

    /// <summary>
    /// Event callback for when a row's <see cref="RowDetails"/> content is expanded or collapsed.
    /// </summary>
    [Parameter]
    public EventCallback<TGridItem> OnRowDetailsToggle { get; set; }

    /// <summary>
    /// Event callback for when the grid's sort order changes.
    /// </summary>
    [Parameter]
    public EventCallback<DataGridSortEventArgs<TGridItem>> OnSortChanged { get; set; }

    /// <summary>
    /// Gets or sets whether the grid can be sorted by more than one column at a time.
    /// The default is <see cref="DataGridSortMode.Single"/>.
    ///
    /// With <see cref="DataGridSortMode.Multiple"/>, Shift+click (Shift+Enter from the keyboard) on a column header
    /// adds that column to the sort, and the column header menu gains items to add a column to the sort, remove it
    /// again and clear the sort. Use <see cref="HeaderCellAsButtonWithMenu"/> to put those items in the header menu
    /// rather than in the column header options popup.
    /// </summary>
    [Parameter]
    public DataGridSortMode SortMode { get; set; } = DataGridSortMode.Single;

    /// <summary>
    /// Gets or sets whether the column headers offer the multi-column sort actions when <see cref="SortMode"/> is
    /// <see cref="DataGridSortMode.Multiple"/>. The default is <see langword="true"/>.
    ///
    /// Set this to <see langword="false"/> to keep the headers as they are in single sort mode; sorting by several
    /// columns is then only possible through Shift+click, Shift+Enter and the grid's sort methods. Note that this
    /// leaves no way to build a multi-column sort by touch, or with a screen reader that does not pass Shift+Enter
    /// through, so only turn it off when your application offers its own UI for it.
    /// </summary>
    [Parameter]
    public bool ShowMultiSortActions { get; set; } = true;

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
    /// Gets or sets the callback that is invoked when the asynchronous loading state of items changes and <see cref="IAsyncQueryExecutor"/> is used.
    /// </summary>
    /// <remarks>The callback receives a <see langword="true"/> value when items start loading
    /// and a <see langword="false"/> value when the loading process completes.</remarks>
    [Parameter]
    public EventCallback<bool> OnItemsLoading { get; set; }

    /// <summary>
    /// Gets or sets a delegate that determines whether a given exception should be handled.
    /// </summary>
    [Parameter]
    public Func<Exception, bool>? HandleLoadingError { get; set; }

    /// <summary>
    /// Gets or sets the content to render when an error occurs.
    /// </summary>
    [Parameter]
    public RenderFragment<Exception?>? ErrorContent { get; set; }

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

    /// <summary>
    /// Gets or sets a value that indicates whether rows should be rendered with
    /// alternating "striped" backgrounds. When <see langword="true"/>, odd and
    /// even rows will use different background styling to improve readability.
    /// </summary>
    [Parameter]
    public bool StripedRows { get; set; }

    /// <summary>
    /// Gets or set the <see cref="DataGridDisplayMode"/> of the grid.
    /// Default is 'Grid'.
    /// When set to Grid, <see cref="GridTemplateColumns" /> can be used to specify column widths.
    /// When set to Table, widths need to be specified at the column level.
    /// When using <see cref="Virtualize"/>, it is recommended to use Table.
    /// </summary>
    [Parameter]
    public DataGridDisplayMode DisplayMode { get; set; } = DataGridDisplayMode.Grid;

    /// <summary>
    /// Gets or sets the size of each row in the grid based on the <see cref="DataGridRowSize"/> enum.
    /// </summary>
    [Parameter]
    public DataGridRowSize RowSize { get; set; } = DataGridRowSize.Small;

    /// <summary>
    /// Gets or sets a value indicating whether the grid should allow multiple lines of text in cells.
    /// Cannot be used together with Virtualize.
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

    /// <summary>
    /// Gets or sets a value indicating whether the grid's dataset is not expected to change during its lifetime.
    /// When set to true, reduces automatic refresh checks for better performance with static datasets.
    /// Default is false to maintain backward compatibility.
    /// </summary>
    [Parameter]
    public bool IsFixed { get; set; }

    // Returns Loading if set (controlled). If not controlled,
    // we assume the grid is loading until the next data load completes
    internal bool EffectiveLoadingValue => Loading ?? (ItemsProvider is not null);

    internal long LastVirtualizeProviderRequestTimestamp => _lastVirtualizeProviderRequestTimestamp;

    internal bool SkipNextVirtualizeProviderDelay => _skipNextVirtualizeProviderDelay;

    internal int LastVirtualizeProviderTotalItemCount => _lastVirtualizeProviderTotalItemCount;

    /// <summary>
    /// Gets the columns the grid is sorted by, in priority order: the first entry is the primary sort.
    /// Holds at most one entry unless <see cref="SortMode"/> is <see cref="DataGridSortMode.Multiple"/>,
    /// and is empty when the grid is not sorted.
    /// </summary>
    public IReadOnlyList<DataGridSortColumn<TGridItem>> SortColumns => _sortColumns;

    /// <summary>
    /// Gets whether any column declares a sort through <see cref="ColumnBase{TGridItem}.IsDefaultSortColumn"/>, which
    /// is what <see cref="ResetSortAsync"/> goes back to.
    /// </summary>
    internal bool HasDeclaredSort => _defaultSortColumns.Count > 0;

    /// <summary>
    /// Gets the sort level for the specified column, or <see langword="null"/> when the grid is not sorted by it.
    /// <c>Level</c> is 1-based, so it can be shown as the sort priority in the column header.
    /// </summary>
    internal (int Level, bool Ascending)? GetSortLevel(ColumnBase<TGridItem> column)
    {
        for (var i = 0; i < _sortColumns.Count; i++)
        {
            if (_sortColumns[i].Column == column)
            {
                return (i + 1, _sortColumns[i].Ascending);
            }
        }

        return null;
    }

    /// <summary>
    /// Gets whether the specified column can be added to the current sort as an extra level, which needs
    /// <see cref="SortMode"/> to be <see cref="DataGridSortMode.Multiple"/> and its
    /// <see cref="ColumnBase{TGridItem}.SortBy"/> to support being applied on top of another ordering.
    /// </summary>
    internal bool CanAddToSort(ColumnBase<TGridItem> column)
        => SortMode == DataGridSortMode.Multiple
            && column.CanSortFromHeader()
            && (_sortColumns.Count == 0 || column.SortBy?.CanApplyThen != false);

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
    protected override async Task OnParametersSetAsync()
    {
        // The associated pagination state may have been added/removed/replaced
        _currentPageItemsChanged.SubscribeOrMove(Pagination?.CurrentPageItemsChanged);

        if (Items is not null && ItemsProvider is not null)
        {
            throw new InvalidOperationException($"FluentDataGrid requires one of {nameof(Items)} or {nameof(ItemsProvider)}, but both were specified.");
        }

        if (Virtualize && MultiLine)
        {
            throw new InvalidOperationException($"FluentDataGrid cannot use both {nameof(Virtualize)} and {nameof(MultiLine)} at the same time.");
        }

        if (Virtualize && RowDetails is not null)
        {
            throw new InvalidOperationException($"FluentDataGrid cannot use both {nameof(Virtualize)} and {nameof(RowDetails)} at the same time.");
        }

        // Perform a re-query only if the data source or something else has changed
        var dataSourceHasChanged = !Equals(ItemsProvider, _lastAssignedItemsProvider) || !ReferenceEquals(Items, _lastAssignedItems);
        if (dataSourceHasChanged)
        {
            await (_scope?.DisposeAsync() ?? default);
            _scope = ScopeFactory.CreateAsyncScope();
            _lastAssignedItemsProvider = ItemsProvider;
            _lastAssignedItems = Items;
            _asyncQueryExecutor = AsyncQueryExecutorSupplier.GetAsyncQueryExecutor(_scope.Value.ServiceProvider, Items);
            _asyncQueryExecuted = false;
        }

        if (_lastVirtualizationMode != Virtualize)
        {
            _lastVirtualizationMode = Virtualize;
            _asyncQueryExecuted = false;
            _retainEmptyContentOnVirtualizedRefresh = false;
        }

        if (Loading == true && _asyncQueryExecutor is not null && _asyncQueryExecuted)
        {
            Loading = false; // switch to uncontrolled loading state after first IAsyncQueryExecutor completes
        }

        var paginationStateHasChanged =
            Pagination?.ItemsPerPage != _lastRefreshedPaginationState?.ItemsPerPage
            || Pagination?.CurrentPageIndex != _lastRefreshedPaginationState?.CurrentPageIndex;

        var mustRefreshData = dataSourceHasChanged || paginationStateHasChanged || EffectiveLoadingValue;

        // We don't want to trigger the first data load until we've collected the initial set of columns,
        // because they might perform some action like setting the default sort order, so it would be wasteful
        // to have to re-query immediately
        if (_columns.Count > 0 && mustRefreshData)
        {
            await RefreshDataCoreAsync();
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && _gridReference is not null)
        {
            // Import the JavaScript module
            if (!await JSModule.TryImportJavaScriptModuleAsync(JAVASCRIPT_FILE))
            {
                return;
            }

            _selfReference = DotNetObjectReference.Create(this);

            _gridController = await JSModule.ObjectReference.InvokeAsync<IJSObjectReference>("Microsoft.FluentUI.Blazor.DataGrid.Initialize", _gridReference, AutoFocus);
            if (AutoItemsPerPage)
            {

                await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.DataGrid.DynamicItemsPerPage", _gridReference, DotNetObjectReference.Create(this), (int)RowSize);
            }

            if (AutoFit)
            {
                await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.DataGrid.AutoFitGridColumns", _gridReference, _columns.Count);
            }
        }

        SaveStateToQueryString();

        if (_checkColumnHeaderUiPosition && _activeHeaderUiColumn is not null)
        {
            _checkColumnHeaderUiPosition = false;
            await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.DataGrid.CheckColumnPopupPosition", _gridReference);
        }

        await UpdateColumnInteractionsAsync();

        if (_restoreHeaderUiFocusColumn is not null)
        {
            var focusColumn = _restoreHeaderUiFocusColumn;
            _restoreHeaderUiFocusColumn = null;

            await focusColumn.FocusOptionsButtonAsync();
        }

        if (_pendingHeaderUiReopenColumn is not null && _pendingHeaderUiReopenKind != ColumnHeaderUiKind.None)
        {
            var column = _pendingHeaderUiReopenColumn;
            var kind = _pendingHeaderUiReopenKind;

            _pendingHeaderUiReopenColumn = null;
            _pendingHeaderUiReopenKind = ColumnHeaderUiKind.None;

            await ShowColumnHeaderUiAsync(column, kind);
        }
    }

    /// <inheritdoc />
    protected override async ValueTask DisposeAsync(IJSObjectReference jsModule)
    {
        if (_gridController is not null)
        {
            var controller = _gridController;
            _gridController = null;
            try
            {
                // Initialize returns a controller whose stop method aborts its registered event listeners and observers.
                await controller.InvokeVoidAsync("stop");
            }
            finally
            {
                await controller.DisposeAsync();
            }
        }
    }

    private async Task UpdateColumnInteractionsAsync()
    {
        if (_checkColumnResizing && _gridReference is not null && JSModule.Imported)
        {
            _checkColumnResizing = false;
            if (ResizableColumns)
            {
                await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.DataGrid.EnableColumnResizing", _gridReference, ResizeColumnOnAllRows);
            }
            else
            {
                await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.DataGrid.DisableColumnResizing", _gridReference);
            }
        }

        if (_checkColumnReordering && _gridReference is not null && JSModule.Imported && _selfReference is not null)
        {
            _checkColumnReordering = false;
            if (ReorderableColumns)
            {
                await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.DataGrid.EnableColumnReordering", _gridReference, _selfReference);
            }
            else
            {
                await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.DataGrid.DisableColumnReordering", _gridReference);
            }
        }
    }

    // Invoked by descendant columns at a special time during rendering
    internal void AddColumn(ColumnBase<TGridItem> column, DataGridSortDirection? initialSortDirection, bool isDefaultSortColumn)
    {
        if (!_collectingColumns)
        {
            return;
        }

        column.SetColumnIndex(_columns.Count + 1);
        _columns.Add(column);

        if (!isDefaultSortColumn || !initialSortDirection.HasValue || _defaultSortColumns.Exists(x => x.Column == column))
        {
            return;
        }

        // Only the first default sort column is used in Single mode; in Multiple mode the columns are sorted on in
        // declaration order.
        if (SortMode != DataGridSortMode.Multiple && _defaultSortColumns.Count > 0)
        {
            return;
        }

        var ascending = initialSortDirection.Value != DataGridSortDirection.Descending;
        _defaultSortColumns.Add(new DataGridSortColumn<TGridItem>(column, ascending));

        if (_internalGridContext.DefaultSortColumn.Column is null)
        {
            _internalGridContext.DefaultSortColumn = (column, initialSortDirection.Value);
        }

        // The declared sort is applied while the columns are collected for the first time, unless something else
        // (restored state, or a programmatic call) already sorted the grid.
        if (!_defaultSortApplied && _sortColumns.Count == _defaultSortColumns.Count - 1)
        {
            _sortColumns.Add(new DataGridSortColumn<TGridItem>(column, ascending));
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

        if (_columns.Count > 0)
        {
            // Sort state that was restored before the columns existed can only be resolved to columns now.
            ApplyPendingSortStateFromUrl();
            _defaultSortApplied = true;
        }

        if (!string.IsNullOrWhiteSpace(GridTemplateColumns) && _columns.Exists(x => x is not SelectColumn<TGridItem> && !string.IsNullOrWhiteSpace(x.Width)))
        {
            throw new ArgumentException("You can use either the 'GridTemplateColumns' parameter on the grid or the 'Width' property at the column level, not both.");
        }

        if (_columns.Where(x => x.HierarchicalToggle).Skip(1).Any())
        {
            throw new ArgumentException("Only one column can have 'HierarchicalToggle' set to true.");
        }

        if (_columns.Exists(x => x.HierarchicalToggle) && !_columns[0].HierarchicalToggle)
        {
            throw new ArgumentException("The 'HierarchicalToggle' parameter can only be set on the first column of the grid.");
        }

        AssignColumnKeys();
        ValidatePinnedColumnConstraints();
        ApplyStoredColumnOrder();
        ValidateAndComputePinnedColumns();
        UpdateGridTemplateColumns();

        UpdateColumnInteractionState();
    }

    private void UpdateColumnInteractionState()
    {
        var options = new ColumnInteractionOptions(
            ResizableColumns,
            ReorderableColumns,
            ResizeColumnOnAllRows,
            GenerateHeader,
            DisplayMode,
            RowSize,
            MultiLine,
            _internalGridTemplateColumns);
        var changed = _columnInteractionOptions != options || _columnInteractionColumns.Count != _columns.Count;

        for (var columnIndex = 0; columnIndex < _columns.Count; columnIndex++)
        {
            var column = _columns[columnIndex];
            var state = (column, column.ColumnKey, column.Pin, column.Width, column.MinWidth);
            if (columnIndex >= _columnInteractionColumns.Count)
            {
                _columnInteractionColumns.Add(state);
            }
            else if (_columnInteractionColumns[columnIndex] != state)
            {
                _columnInteractionColumns[columnIndex] = state;
                changed = true;
            }
        }

        if (_columnInteractionColumns.Count > _columns.Count)
        {
            _columnInteractionColumns.RemoveRange(_columns.Count, _columnInteractionColumns.Count - _columns.Count);
        }

        if (changed)
        {
            _checkColumnResizing |= ResizableColumns || _columnInteractionOptions?.Resizing == true;
            _checkColumnReordering |= ReorderableColumns || _columnInteractionOptions?.Reordering == true;
        }

        _columnInteractionOptions = options;
    }

    private void AssignColumnKeys()
    {
        var keyCounts = new Dictionary<string, int>(StringComparer.Ordinal);

        foreach (var column in _columns)
        {
            var baseKey = GetColumnKeyBase(column);
            keyCounts.TryGetValue(baseKey, out var existingCount);
            keyCounts[baseKey] = existingCount + 1;
            column.SetColumnKey(existingCount == 0 ? baseKey : $"{baseKey}_{existingCount.ToString(CultureInfo.InvariantCulture)}");
        }
    }

    private static string GetColumnKeyBase(ColumnBase<TGridItem> column)
    {
        if (!string.IsNullOrWhiteSpace(column.ColumnId))
        {
            return column.ColumnId;
        }

        if (column is IBindableColumn bindable && bindable.PropertyInfo is not null)
        {
            return $"{bindable.PropertyInfo.DeclaringType?.FullName ?? typeof(TGridItem).FullName}.{bindable.PropertyInfo.Name}";
        }

        if (!string.IsNullOrWhiteSpace(column.Title))
        {
            return column.Title;
        }

        return $"{column.GetType().Name}_{column.Index.ToString(CultureInfo.InvariantCulture)}";
    }

    private void ApplyStoredColumnOrder()
    {
        if (_columns.Count == 0)
        {
            _columnOrder.Clear();
            return;
        }

        if (ColumnOrder is not null)
        {
            _columnOrder.Clear();
            _columnOrder.AddRange(ColumnOrder.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        var startPinnedColumns = _columns.Where(c => c.Pin == DataGridColumnPin.Start).ToList();
        var reorderableColumns = _columns.Where(c => c.Pin == DataGridColumnPin.None).ToList();
        var endPinnedColumns = _columns.Where(c => c.Pin == DataGridColumnPin.End).ToList();

        if (_columnOrder.Count > 0 && reorderableColumns.Count > 0)
        {
            var orderLookup = _columnOrder
                .Distinct(StringComparer.Ordinal)
                .Select((columnKey, orderIndex) => (columnKey, orderIndex))
                .ToDictionary(x => x.columnKey, x => x.orderIndex, StringComparer.Ordinal);

            reorderableColumns = [.. reorderableColumns
                .OrderBy(column => orderLookup.TryGetValue(column.ColumnKey, out var orderIndex) ? orderIndex : int.MaxValue)
                .ThenBy(column => column.Index)];
        }

        _columns.Clear();
        _columns.AddRange(startPinnedColumns);
        _columns.AddRange(reorderableColumns);
        _columns.AddRange(endPinnedColumns);

        ResetColumnIndices();
        UpdateTrackedColumnOrder();
    }

    private void ResetColumnIndices()
    {
        for (var index = 0; index < _columns.Count; index++)
        {
            _columns[index].SetColumnIndex(index + 1);
        }
    }

    private void UpdateGridTemplateColumns()
    {
        // Always re-evaluate after collecting columns when using displaymode grid. A column might be added or hidden and the _internalGridTemplateColumns needs to reflect that.
        if (DisplayMode != DataGridDisplayMode.Grid)
        {
            return;
        }

        if (!AutoFit)
        {
            _internalGridTemplateColumns = GridTemplateColumns ?? string.Join(' ', Enumerable.Repeat("1fr", _columns.Count));
        }

        if (_columns.Exists(x => !string.IsNullOrWhiteSpace(x.Width)))
        {
            _internalGridTemplateColumns = GridTemplateColumns ?? string.Join(' ', _columns.Select(x => x.Width ?? "auto"));
        }
    }

    private void UpdateTrackedColumnOrder()
    {
        _columnOrder.Clear();
        _columnOrder.AddRange(_columns.Select(column => column.ColumnKey));
    }

    /// <summary>
    /// Validates the pinned-column configuration and seeds initial sticky offsets for each
    /// pinned column before JavaScript recomputes them from rendered widths after first render.
    /// Rules enforced:
    /// <list type="bullet">
    ///   <item>Pinned columns must specify an explicit <c>Width</c>.</item>
    ///   <item>Start-pinned columns must be contiguous at the beginning of the column list.</item>
    ///   <item>End-pinned columns must be contiguous at the end of the column list.</item>
    /// </list>
    /// </summary>
    private void ValidateAndComputePinnedColumns()
    {
        var hasPinned = _columns.Exists(c => c.Pin != DataGridColumnPin.None);
        if (!hasPinned)
        {
            return;
        }

        ValidatePinnedColumnConstraints();

        // Compute start-pin sticky offsets in display order.
        var startOffset = 0.0;
        foreach (var col in _columns.Where(c => c.Pin == DataGridColumnPin.Start))
        {
            col.PinOffset = $"{startOffset.ToString(CultureInfo.InvariantCulture)}px";
            startOffset += ParsePixelWidth(col.Width);
        }

        // Compute end-pin sticky offsets in reverse display order.
        var endOffset = 0.0;
        foreach (var col in _columns.Where(c => c.Pin == DataGridColumnPin.End).Reverse())
        {
            col.PinOffset = $"{endOffset.ToString(CultureInfo.InvariantCulture)}px";
            endOffset += ParsePixelWidth(col.Width);
        }
    }

    /// <summary>
    /// Enforces width and ordering constraints for pinned columns. Called only when at least one
    /// pinned column exists.
    /// </summary>
    private void ValidatePinnedColumnConstraints()
    {
        // Width must be explicitly provided for pinned columns.
        foreach (var col in _columns.Where(c => c.Pin != DataGridColumnPin.None))
        {
            if (string.IsNullOrWhiteSpace(col.Width))
            {
                throw new ArgumentException(
                    $"Column '{col.Title ?? col.Index.ToString(CultureInfo.InvariantCulture)}' has Pin set but no Width. " +
                    "Pinned columns require an explicit Width.");
            }
        }

        // Start-pinned columns must be contiguous at the start: each one must be preceded by
        // another start-pinned column (or be the very first column).
        for (var i = 0; i < _columns.Count; i++)
        {
            if (_columns[i].Pin == DataGridColumnPin.Start && i > 0 && _columns[i - 1].Pin != DataGridColumnPin.Start)
            {
                throw new ArgumentException(
                    $"Column '{_columns[i].Title ?? _columns[i].Index.ToString(CultureInfo.InvariantCulture)}' is start-pinned but the preceding column is not. " +
                    "Start-pinned columns must be contiguous at the start of the column list.");
            }
        }

        // End-pinned columns must be contiguous at the end: each one must be followed by
        // another end-pinned column (or be the very last column).
        for (var i = 0; i < _columns.Count; i++)
        {
            if (_columns[i].Pin == DataGridColumnPin.End && i < _columns.Count - 1 && _columns[i + 1].Pin != DataGridColumnPin.End)
            {
                throw new ArgumentException(
                    $"Column '{_columns[i].Title ?? _columns[i].Index.ToString(CultureInfo.InvariantCulture)}' is end-pinned but the following column is not. " +
                    "End-pinned columns must be contiguous at the end of the column list.");
            }
        }
    }

    /// <summary>
    /// Parses a CSS pixel value string such as <c>"150px"</c> and returns the numeric value.
    /// Returns <c>0</c> if the string is null, empty, or not a valid pixel value so JavaScript
    /// can recompute the final sticky offsets from rendered widths after first render.
    /// </summary>
    private static double ParsePixelWidth(string? width)
    {
        if (string.IsNullOrWhiteSpace(width))
        {
            return 0;
        }

        var trimmed = width.Trim();
        if (trimmed.EndsWith("px", StringComparison.OrdinalIgnoreCase) &&
            double.TryParse(trimmed[..^2], NumberStyles.Number, CultureInfo.InvariantCulture, out var px))
        {
            return px;
        }

        return 0;
    }

    internal ColumnHeaderCapabilities GetHeaderCapabilities(ColumnBase<TGridItem> column)
    {
        var canSort = column.CanSortFromHeader();
        var canResize = ResizeType is not null && ResizableColumns;
        var canReorder = ReorderableColumns && IsColumnEligibleForReordering(column);
        var hasOptions = column.ColumnOptions is not null;

        // Without the header menu, the multi-column sort actions live in the column header popup, which is the only
        // place they can be reached without the Shift+click shortcut.
        var hasSortOptions = canSort && SortMode == DataGridSortMode.Multiple && ShowMultiSortActions && !HeaderCellAsButtonWithMenu;
        var hasHeaderPopupContent = canReorder || hasOptions || hasSortOptions || (!HeaderCellAsButtonWithMenu && canResize);

        return new(canSort, hasSortOptions, canResize, canReorder, hasOptions, hasHeaderPopupContent);
    }

    internal bool CanMoveColumnToStart(ColumnBase<TGridItem> column)
        => TryGetReorderableColumnIndex(column, out var index) && index > 0;

    internal bool CanMoveColumnLeft(ColumnBase<TGridItem> column)
        => CanMoveColumnToStart(column);

    internal bool CanMoveColumnRight(ColumnBase<TGridItem> column)
        => TryGetReorderableColumnIndex(column, out var index) && index >= 0 && index < GetReorderableColumns().Count - 1;

    internal bool CanMoveColumnToEnd(ColumnBase<TGridItem> column)
        => CanMoveColumnRight(column);

    /// <summary>
    /// Gets the grid's current column order.
    /// </summary>
    public IReadOnlyList<string> GetColumnOrder() => [.. _columns.Select(column => column.ColumnKey)];

    /// <summary>
    /// Applies a persisted column order to the current grid.
    /// </summary>
    /// <param name="columnOrder">The persisted column order to apply.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task SetColumnOrderAsync(IEnumerable<string>? columnOrder)
    {
        _columnOrder.Clear();

        if (columnOrder is not null)
        {
            _columnOrder.AddRange(columnOrder.Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        if (_columns.Count == 0)
        {
            return Task.CompletedTask;
        }

        ApplyStoredColumnOrder();
        ValidateAndComputePinnedColumns();
        UpdateGridTemplateColumns();
        _ = InvokeAsync(StateHasChanged);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Moves the specified column one position to the left within the reorderable column range.
    /// </summary>
    public Task MoveColumnLeftAsync(ColumnBase<TGridItem> column) => MoveColumnToReorderableIndexAsync(column, GetCurrentReorderableIndex(column) - 1);

    /// <summary>
    /// Moves the specified column one position to the right within the reorderable column range.
    /// </summary>
    public Task MoveColumnRightAsync(ColumnBase<TGridItem> column) => MoveColumnToReorderableIndexAsync(column, GetCurrentReorderableIndex(column) + 1);

    /// <summary>
    /// Moves the specified column to the start of the reorderable column range.
    /// </summary>
    public Task MoveColumnToStartAsync(ColumnBase<TGridItem> column) => MoveColumnToReorderableIndexAsync(column, 0);

    /// <summary>
    /// Moves the specified column to the end of the reorderable column range.
    /// </summary>
    public Task MoveColumnToEndAsync(ColumnBase<TGridItem> column) => MoveColumnToReorderableIndexAsync(column, GetReorderableColumns().Count - 1);

    /// <summary>
    /// Reorders a column from a drag-and-drop operation reported by JavaScript.
    /// </summary>
    /// <param name="sourceColumnKey">The key of the dragged column.</param>
    /// <param name="targetColumnKey">The key of the drop target column.</param>
    /// <param name="insertAfter">If true, inserts the dragged column after the target column; otherwise inserts it before.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    [JSInvokable]
    public Task ReorderColumnFromDragAsync(string sourceColumnKey, string targetColumnKey, bool insertAfter)
    {
        var reorderableColumns = GetReorderableColumns();
        var sourceIndex = reorderableColumns.FindIndex(column => string.Equals(column.ColumnKey, sourceColumnKey, StringComparison.Ordinal));
        var targetIndex = reorderableColumns.FindIndex(column => string.Equals(column.ColumnKey, targetColumnKey, StringComparison.Ordinal));

        if (sourceIndex < 0 || targetIndex < 0)
        {
            return Task.CompletedTask;
        }

        if (insertAfter && sourceIndex > targetIndex)
        {
            targetIndex++;
        }
        else if (!insertAfter && sourceIndex < targetIndex)
        {
            targetIndex--;
        }

        return MoveColumnToReorderableIndexAsync(reorderableColumns[sourceIndex], targetIndex);
    }

    private async Task MoveColumnToReorderableIndexAsync(ColumnBase<TGridItem> column, int targetIndex)
    {
        var reorderableColumns = GetReorderableColumns();
        var currentIndex = reorderableColumns.IndexOf(column);
        var reopenReorderPopupAfterMove = DisplayMode == DataGridDisplayMode.Table
            && _activeHeaderUiKind == ColumnHeaderUiKind.Reorder
            && _activeHeaderUiColumn == column;

        if (currentIndex < 0 || reorderableColumns.Count <= 1)
        {
            return;
        }

        targetIndex = Math.Clamp(targetIndex, 0, reorderableColumns.Count - 1);
        if (targetIndex == currentIndex)
        {
            return;
        }

        if (reopenReorderPopupAfterMove)
        {
            _activeHeaderUiColumn = null;
            _activeHeaderUiKind = ColumnHeaderUiKind.None;
            _checkColumnHeaderUiPosition = false;
            _pendingHeaderUiReopenColumn = column;
            _pendingHeaderUiReopenKind = ColumnHeaderUiKind.Reorder;
        }

        reorderableColumns.RemoveAt(currentIndex);
        reorderableColumns.Insert(targetIndex, column);

        var startPinnedColumns = _columns.Where(c => c.Pin == DataGridColumnPin.Start).ToList();
        var endPinnedColumns = _columns.Where(c => c.Pin == DataGridColumnPin.End).ToList();

        _columns.Clear();
        _columns.AddRange(startPinnedColumns);
        _columns.AddRange(reorderableColumns);
        _columns.AddRange(endPinnedColumns);

        ResetColumnIndices();
        ValidateAndComputePinnedColumns();
        UpdateGridTemplateColumns();
        await PersistColumnOrderAsync();

        _checkColumnHeaderUiPosition = _activeHeaderUiColumn is not null;
        await InvokeAsync(StateHasChanged);
    }

    private async Task PersistColumnOrderAsync()
    {
        UpdateTrackedColumnOrder();

        if (ColumnOrderChanged.HasDelegate)
        {
            await ColumnOrderChanged.InvokeAsync(GetColumnOrder());
        }
    }

    private bool IsColumnEligibleForReordering(ColumnBase<TGridItem> column)
        => column.Pin == DataGridColumnPin.None && GetReorderableColumns().Count > 1;

    private bool TryGetReorderableColumnIndex(ColumnBase<TGridItem> column, out int index)
    {
        index = GetCurrentReorderableIndex(column);
        return index >= 0;
    }

    private int GetCurrentReorderableIndex(ColumnBase<TGridItem> column)
        => GetReorderableColumns().IndexOf(column);

    private List<ColumnBase<TGridItem>> GetReorderableColumns()
        => [.. _columns.Where(c => c.Pin == DataGridColumnPin.None)];

    private Task ShowColumnHeaderUiAsync(ColumnBase<TGridItem> column, ColumnHeaderUiKind kind)
    {
        _activeHeaderUiColumn = column;
        _activeHeaderUiKind = kind;
        _checkColumnHeaderUiPosition = true;
        _ = InvokeAsync(StateHasChanged);
        return Task.CompletedTask;
    }

    private bool IsColumnHeaderUiActive(ColumnBase<TGridItem> column, ColumnHeaderUiKind kind)
        => _activeHeaderUiColumn == column && _activeHeaderUiKind == kind;

    private bool ShouldRenderColumnHeaderUi(ColumnBase<TGridItem> column, ColumnHeaderCapabilities headerCapabilities)
        => IsColumnHeaderUiActive(column, ColumnHeaderUiKind.All)
            ? headerCapabilities.HasAnyAction
            : ShouldRenderColumnSort(column, headerCapabilities)
                || ShouldRenderColumnOptions(column, headerCapabilities)
                || ShouldRenderColumnResize(column, headerCapabilities)
                || ShouldRenderColumnReorder(column, headerCapabilities);

    private bool ShouldRenderColumnSort(ColumnBase<TGridItem> column, ColumnHeaderCapabilities headerCapabilities)
        => headerCapabilities.HasSortOptions
            && (IsColumnHeaderUiActive(column, ColumnHeaderUiKind.Sort)
                || IsColumnHeaderUiActive(column, ColumnHeaderUiKind.All));

    /// <summary>
    /// Gets whether the column header popup holds nothing but the sort menu, which then sizes the popup instead of the
    /// fixed width the options, resize and reorder UI need.
    /// </summary>
    private bool IsSortOnlyHeaderUi(ColumnBase<TGridItem> column, ColumnHeaderCapabilities headerCapabilities)
        => ShouldRenderColumnSort(column, headerCapabilities)
            && !ShouldRenderColumnOptions(column, headerCapabilities)
            && !ShouldRenderColumnResize(column, headerCapabilities)
            && !ShouldRenderColumnReorder(column, headerCapabilities);

    private bool ShouldRenderColumnOptions(ColumnBase<TGridItem> column, ColumnHeaderCapabilities headerCapabilities)
        => headerCapabilities.HasOptions
            && (IsColumnHeaderUiActive(column, ColumnHeaderUiKind.Options)
                || IsColumnHeaderUiActive(column, ColumnHeaderUiKind.All));

    private bool ShouldRenderColumnResize(ColumnBase<TGridItem> column, ColumnHeaderCapabilities headerCapabilities)
        => headerCapabilities.CanResize
            && (IsColumnHeaderUiActive(column, ColumnHeaderUiKind.Resize)
                || IsColumnHeaderUiActive(column, ColumnHeaderUiKind.All));

    private bool ShouldRenderColumnReorder(ColumnBase<TGridItem> column, ColumnHeaderCapabilities headerCapabilities)
        => headerCapabilities.CanReorder
            && (IsColumnHeaderUiActive(column, ColumnHeaderUiKind.Reorder)
                || IsColumnHeaderUiActive(column, ColumnHeaderUiKind.All));

    internal bool IsColumnResizeUiOpen => _activeHeaderUiKind == ColumnHeaderUiKind.Resize && _activeHeaderUiColumn is not null;

    /// <summary>
    /// Sets the grid's current sort column to the specified <paramref name="column"/>, replacing any columns the grid
    /// was sorted by. Use <see cref="AddSortByColumnAsync(ColumnBase{TGridItem}, DataGridSortDirection)"/> to add a
    /// sort level instead.
    /// </summary>
    /// <param name="column">The column that defines the new sort order.</param>
    /// <param name="direction">The direction of sorting. If the value is <see cref="DataGridSortDirection.Auto"/>, then it will toggle the direction on each call.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public async Task SortByColumnAsync(ColumnBase<TGridItem> column, DataGridSortDirection direction = DataGridSortDirection.Auto)
    {
        var primary = _sortColumns.Count > 0 ? _sortColumns[0] : default(DataGridSortColumn<TGridItem>?);
        var ascending = direction switch
        {
            DataGridSortDirection.Ascending => true,
            DataGridSortDirection.Descending => false,
            DataGridSortDirection.Auto => primary?.Column != column || !primary.Value.Ascending,
            _ => true,
        };

        _sortColumns.Clear();
        _sortColumns.Add(new DataGridSortColumn<TGridItem>(column, ascending));

        await NotifySortChangedAsync();
    }

    /// <summary>
    /// Adds the specified <paramref name="column"/> to the grid's sort as an extra level, keeping the columns the grid
    /// is already sorted by. If the grid is already sorted by the column, only its direction changes and it keeps its
    /// level.
    ///
    /// Requires <see cref="SortMode"/> to be <see cref="DataGridSortMode.Multiple"/>. Otherwise, and when the grid is
    /// not sorted yet or the column's sort cannot be applied on top of another ordering, this sorts by the column
    /// alone, like <see cref="SortByColumnAsync(ColumnBase{TGridItem}, DataGridSortDirection)"/> does.
    /// </summary>
    /// <param name="column">The column to add to the sort.</param>
    /// <param name="direction">The direction of sorting. If the value is <see cref="DataGridSortDirection.Auto"/>, a
    /// column that is already sorted on has its direction toggled and a new column is sorted ascending.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public async Task AddSortByColumnAsync(ColumnBase<TGridItem> column, DataGridSortDirection direction = DataGridSortDirection.Auto)
    {
        var index = _sortColumns.FindIndex(x => x.Column == column);

        if (SortMode != DataGridSortMode.Multiple || _sortColumns.Count == 0 || (index < 0 && !CanAddToSort(column)))
        {
            await SortByColumnAsync(column, direction);
            return;
        }

        var ascending = direction switch
        {
            DataGridSortDirection.Ascending => true,
            DataGridSortDirection.Descending => false,
            DataGridSortDirection.Auto => index < 0 || !_sortColumns[index].Ascending,
            _ => true,
        };

        if (index < 0)
        {
            _sortColumns.Add(new DataGridSortColumn<TGridItem>(column, ascending));
        }
        else
        {
            _sortColumns[index] = new DataGridSortColumn<TGridItem>(column, ascending);
        }

        await NotifySortChangedAsync();
    }

    /// <summary>
    /// Adds the column with the specified <paramref name="title"/> to the grid's sort as an extra level. If the title
    /// is not found, nothing happens.
    /// </summary>
    /// <param name="title">The title of the column to add to the sort.</param>
    /// <param name="direction">The direction of sorting, as in <see cref="AddSortByColumnAsync(ColumnBase{TGridItem}, DataGridSortDirection)"/>.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task AddSortByColumnAsync(string title, DataGridSortDirection direction = DataGridSortDirection.Auto)
    {
        var column = _columns.Find(c => c.Title?.Equals(title, StringComparison.InvariantCultureIgnoreCase) ?? false);

        return column is not null ? AddSortByColumnAsync(column, direction) : Task.CompletedTask;
    }

    /// <summary>
    /// Adds the column with the specified <paramref name="index"/> to the grid's sort as an extra level. If the index
    /// is out of range, nothing happens.
    /// </summary>
    /// <param name="index">The index of the column to add to the sort.</param>
    /// <param name="direction">The direction of sorting, as in <see cref="AddSortByColumnAsync(ColumnBase{TGridItem}, DataGridSortDirection)"/>.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task AddSortByColumnAsync(int index, DataGridSortDirection direction = DataGridSortDirection.Auto)
    {
        return index >= 0 && index < _columns.Count ? AddSortByColumnAsync(_columns[index], direction) : Task.CompletedTask;
    }

    /// <summary>
    /// Replaces the grid's sort with the specified columns, in priority order. An empty collection leaves the grid
    /// unsorted, as <see cref="ClearSortAsync"/> does.
    /// </summary>
    /// <param name="sortColumns">The columns to sort by, where the first entry becomes the primary sort.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    /// <exception cref="ArgumentException">A column appears more than once, or a column's sort cannot be used at the
    /// level it is given.</exception>
    /// <exception cref="InvalidOperationException">More than one column is given while <see cref="SortMode"/> is
    /// <see cref="DataGridSortMode.Single"/>.</exception>
    public async Task SetSortAsync(IEnumerable<DataGridSortColumn<TGridItem>> sortColumns)
    {
        ArgumentNullException.ThrowIfNull(sortColumns);

        var levels = new List<DataGridSortColumn<TGridItem>>();
        foreach (var level in sortColumns)
        {
            if (levels.Exists(x => x.Column == level.Column))
            {
                throw new ArgumentException($"The column '{level.Column.Title}' can only be sorted on once.", nameof(sortColumns));
            }

            if (levels.Count > 0)
            {
                if (SortMode != DataGridSortMode.Multiple)
                {
                    throw new InvalidOperationException($"The grid can only be sorted by one column because {nameof(SortMode)} is {nameof(DataGridSortMode.Single)}.");
                }

                if (level.Column.SortBy?.CanApplyThen == false)
                {
                    throw new ArgumentException($"The sort of column '{level.Column.Title}' cannot be used as a secondary sort level, so it can only be the first column sorted on.", nameof(sortColumns));
                }
            }

            levels.Add(level);
        }

        _sortColumns.Clear();
        _sortColumns.AddRange(levels);

        await NotifySortChangedAsync();
    }

    /// <summary>
    /// Removes every sort level, leaving the grid unsorted. The sort declared by the columns through
    /// <see cref="ColumnBase{TGridItem}.IsDefaultSortColumn"/> is not restored; use <see cref="ResetSortAsync"/>
    /// for that.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public async Task ClearSortAsync()
    {
        if (_sortColumns.Count == 0)
        {
            return;
        }

        _sortColumns.Clear();

        await NotifySortChangedAsync(useCoreRefresh: true);
    }

    /// <summary>
    /// Restores the sort declared by the columns through <see cref="ColumnBase{TGridItem}.IsDefaultSortColumn"/>,
    /// leaving the grid unsorted when no column declares one. This is what removing the last sort level and the
    /// Shift+S shortcut do.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public async Task ResetSortAsync()
    {
        RestoreDefaultSort();

        await NotifySortChangedAsync(useCoreRefresh: true);
    }

    /// <summary>
    /// Sorts the grid by the specified column <paramref name="title"/> found first. If the title is not found, nothing happens.
    /// </summary>
    /// <param name="title">The title of the column to sort by.</param>
    /// <param name="direction">The direction of sorting. The default is <see cref="DataGridSortDirection.Auto"/>. If the value is <see cref="DataGridSortDirection.Auto"/>, then it will toggle the direction on each call.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task SortByColumnAsync(string title, DataGridSortDirection direction = DataGridSortDirection.Auto)
    {
        var column = _columns.Find(c => c.Title?.Equals(title, StringComparison.InvariantCultureIgnoreCase) ?? false);

        return column is not null ? SortByColumnAsync(column, direction) : Task.CompletedTask;
    }

    /// <summary>
    /// Sorts the grid by the specified column <paramref name="index"/>. If the index is out of range, nothing happens.
    /// </summary>
    /// <param name="index">The index of the column to sort by.</param>
    /// <param name="direction">The direction of sorting. The default is <see cref="DataGridSortDirection.Auto"/>. If the value is <see cref="DataGridSortDirection.Auto"/>, then it will toggle the direction on each call.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task SortByColumnAsync(int index, DataGridSortDirection direction = DataGridSortDirection.Auto)
    {
        return index >= 0 && index < _columns.Count ? SortByColumnAsync(_columns[index], direction) : Task.CompletedTask;
    }

    /// <summary>
    /// Removes the specified <paramref name="column"/> from the grid's sort.
    ///
    /// With <see cref="SortMode"/> set to <see cref="DataGridSortMode.Multiple"/>, the other sort levels are kept and
    /// the sort declared by the columns is restored once the last level is removed. Otherwise the sort declared by the
    /// columns is restored right away, and a default sort column is not removed.
    /// </summary>
    /// <param name="column">The column to stop sorting by.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public async Task RemoveSortByColumnAsync(ColumnBase<TGridItem> column)
    {
        if (SortMode == DataGridSortMode.Multiple)
        {
            var index = _sortColumns.FindIndex(x => x.Column == column);
            if (index < 0)
            {
                return;
            }

            _sortColumns.RemoveAt(index);

            if (_sortColumns.Count == 0)
            {
                RestoreDefaultSort();
            }

            await NotifySortChangedAsync(useCoreRefresh: true);
            return;
        }

        if (_sortColumns.Count > 0 && _sortColumns[0].Column == column && !column.IsDefaultSortColumn)
        {
            RestoreDefaultSort();

            await NotifySortChangedAsync(useCoreRefresh: true);
        }
    }

    /// <summary>
    /// Removes the grid's sort on double click for the currently sorted column if it's not a default sort column.
    /// With <see cref="SortMode"/> set to <see cref="DataGridSortMode.Multiple"/>, every sort level is removed and the
    /// sort declared by the columns is restored.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task RemoveSortByColumnAsync()
    {
        if (SortMode == DataGridSortMode.Multiple)
        {
            return _sortColumns.Count > 0 ? ResetSortAsync() : Task.CompletedTask;
        }

        return _sortColumns.Count > 0 ? RemoveSortByColumnAsync(_sortColumns[0].Column) : Task.CompletedTask;
    }

    private void RestoreDefaultSort()
    {
        _sortColumns.Clear();
        _sortColumns.AddRange(_defaultSortColumns);
    }

    private async Task NotifySortChangedAsync(bool useCoreRefresh = false)
    {
        UpdateSortAnnouncement();

        if (OnSortChanged.HasDelegate)
        {
            await OnSortChanged.InvokeAsync(new()
            {
                SortColumns = [.. _sortColumns],
            });
        }

        _ = InvokeAsync(StateHasChanged); // We want to see the updated sort order in the header, even before the data query is completed

        if (useCoreRefresh)
        {
            await RefreshDataCoreAsync();
        }
        else
        {
            await RefreshDataAsync();
        }
    }

    /// <summary>
    /// Builds the text for the grid's sort status message, which is announced by screen readers because changing the
    /// sort updates neither the focused element nor its accessible name.
    /// </summary>
    private void UpdateSortAnnouncement()
    {
        if (SortMode != DataGridSortMode.Multiple)
        {
            return;
        }

        if (_sortColumns.Count == 0)
        {
            _sortAnnouncement = Localizer[LanguageResource.DataGrid_SortAnnouncementCleared];
            return;
        }

        var levels = _sortColumns.Select(level => Localizer[
            level.Ascending ? LanguageResource.DataGrid_SortColumnAscending : LanguageResource.DataGrid_SortColumnDescending,
            level.Column.Title ?? level.Column.ColumnKey]);

        _sortAnnouncement = Localizer[
            LanguageResource.DataGrid_SortAnnouncement,
            string.Join(Localizer[LanguageResource.DataGrid_SortAnnouncementSeparator], levels)];
    }

    /// <summary>
    /// Displays the <see cref="ColumnBase{TGridItem}.ColumnOptions"/> UI for the specified column, closing any other column
    /// options UI that was previously displayed.
    /// </summary>
    /// <param name="column">The column whose options are to be displayed, if any are available.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task ShowColumnOptionsAsync(ColumnBase<TGridItem> column)
    {
        return ShowColumnHeaderUiAsync(column, HeaderCellAsButtonWithMenu ? ColumnHeaderUiKind.Options : ColumnHeaderUiKind.All);
    }

    /// <summary>
    /// Displays all available header UI elements for the specified column asynchronously.
    /// </summary>
    /// <param name="column">The column for which to display all header UI elements. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task ShowAllHeaderUIAsync(ColumnBase<TGridItem> column)
    {
        return ShowColumnHeaderUiAsync(column, ColumnHeaderUiKind.All);
    }

    /// <summary>
    /// Displays the <see cref="ColumnBase{TGridItem}.ColumnOptions"/> UI for the specified column,
    /// closing any other column options UI that was previously displayed. If the title is not found, nothing happens.
    /// </summary>
    internal Task ShowColumnReorderAsync(ColumnBase<TGridItem> column)
    {
        return ShowColumnHeaderUiAsync(column, ColumnHeaderUiKind.Reorder);
    }

    /// <summary>
    /// Displays the sort UI for the specified column, closing any other column header UI that was previously
    /// displayed.
    /// </summary>
    internal Task ShowColumnSortAsync(ColumnBase<TGridItem> column)
    {
        return ShowColumnHeaderUiAsync(column, ColumnHeaderUiKind.Sort);
    }

    /// <summary>
    /// Displays the <see cref="ColumnBase{TGridItem}.ColumnOptions"/> UI for the specified column
    /// <paramref name="title"/> found first, closing any other column options UI that was previously displayed. If the
    /// title is not found, nothing happens.
    /// </summary>
    /// <param name="title">The column title whose options UI is to be displayed.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task ShowColumnOptionsAsync(string title)
    {
        var column = _columns.Find(c => c.Title?.Equals(title, StringComparison.InvariantCultureIgnoreCase) ?? false);
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
    /// Closes the column resize UI that was previously displayed.
    /// </summary>
    public Task CloseColumnHeaderUIAsync() => CloseColumnHeaderUIAsync(restoreFocus: false);

    /// <summary>
    /// Closes the column header UI, optionally putting focus back on the button that opened it. Focus is restored when
    /// the grid itself closes the popup (after a sort action, say), not when the user clicks elsewhere, where moving
    /// focus would take it away from whatever they clicked.
    /// </summary>
    internal Task CloseColumnHeaderUIAsync(bool restoreFocus)
    {
        if (restoreFocus)
        {
            _restoreHeaderUiFocusColumn = _activeHeaderUiColumn;
        }

        _activeHeaderUiColumn = null;
        _activeHeaderUiKind = ColumnHeaderUiKind.None;
        _checkColumnHeaderUiPosition = false;
        _ = InvokeAsync(StateHasChanged);
        return Task.CompletedTask;

    }

    /// <summary>
    /// Displays the column resize UI for the specified column, closing any other column
    /// resize UI that was previously displayed.
    /// </summary>
    /// <param name="column">The column whose resize UI is to be displayed.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task ShowColumnResizeAsync(ColumnBase<TGridItem> column)
    {
        return ShowColumnHeaderUiAsync(column, ColumnHeaderUiKind.Resize);
    }

    /// <summary>
    /// Displays the column resize UI for the specified column, closing any other column
    /// resize UI that was previously displayed.
    /// </summary>
    /// <param name="title">The column title whose resize UI is to be displayed.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task ShowColumnResizeAsync(string title)
    {
        var column = _columns.Find(c => c.Title?.Equals(title, StringComparison.InvariantCultureIgnoreCase) ?? false);
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
    /// <summary>
    /// Sets the grid's loading state to the specified value.
    /// </summary>
    /// <param name="loading"></param>
    public void SetLoadingState(bool? loading)
    {
        Loading = loading;
    }

    /// <summary>
    /// Instructs the grid to re-fetch and render the current data from the supplied data source
    /// (either <see cref="Items"/> or <see cref="ItemsProvider"/>).
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the completion of the operation.</returns>
    public async Task RefreshDataAsync(bool force = false)
    {
        _forceRefreshData = force;
        await RefreshDataCoreAsync();
    }

#if NET11_0_OR_GREATER
    /// <summary>
    /// Scrolls the virtualized grid so the row at <paramref name="itemIndex"/> is aligned to the top of the scrollable area.
    /// </summary>
    /// <remarks>
    /// Each call cancels any previously-running <see cref="ScrollToItemAsync(int, CancellationToken)"/> operation.
    /// Must be called on the renderer's synchronization context; background-thread callers should wrap with
    /// <see cref="ComponentBase.InvokeAsync(Func{Task})"/>.
    /// </remarks>
    /// <param name="itemIndex">The zero-based index of the row to scroll to.</param>
    /// <param name="cancellationToken">A token that lets the caller request cancellation.</param>
    /// <returns>A <see cref="Task"/> that completes when the target is aligned or superseded by another call.</returns>
    public Task ScrollToItemAsync(int itemIndex, CancellationToken cancellationToken = default)
    {
        if (!Virtualize || _virtualizeComponent is null)
        {
            throw new InvalidOperationException(
                $"{nameof(ScrollToItemAsync)} can only be used when {nameof(Virtualize)} is enabled and the grid has been rendered.");
        }

        return _virtualizeComponent.ScrollToItemAsync(itemIndex, cancellationToken);
    }
#endif

    // Same as RefreshDataAsync, except without forcing a re-render. We use this from OnParametersSetAsync
    // because in that case there's going to be a re-render anyway.
    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "Not going to do artificial optimization because of some random arbitrary determined line count number")]
    private async Task RefreshDataCoreAsync()
    {
        // Move into a "loading" state, cancelling any earlier-but-still-pending load
        _pendingDataLoadCancellationTokenSource?.CancelAsync();
        var thisLoadCts = _pendingDataLoadCancellationTokenSource = new CancellationTokenSource();

        if (Virtualize)
        {
            _skipNextVirtualizeProviderDelay = true;
            if (_virtualizeComponent is not null)
            {
                // If we're using Virtualize, we have to go through its RefreshDataAsync API otherwise:
                // (1) It won't know to update its own internal state if the provider output has changed
                // (2) We won't know what slice of data to query for
                // ProvideVirtualizedItemsAsync updates _internalGridContext.Items and fires ItemsChanged,
                // so no second query is needed here.
                await _virtualizeComponent.RefreshDataAsync();
                _pendingDataLoadCancellationTokenSource = null;
                return;
            }
            // If Virtualize is true but we don't have a reference to the component, either we're still in
            // the first render, or the empty content replaced (and disposed) the component after the
            // provider reported zero items. Clear the flag so the re-render below re-mounts Virtualize;
            // it will call us when it's ready, so we can just wait for that instead of trying to load
            // data now (#5151).
            _retainEmptyContentOnVirtualizedRefresh |= _virtualizeItemsProvided
                && _internalGridContext.TotalItemCount == 0;
            _virtualizeItemsProvided = false;
            _pendingDataLoadCancellationTokenSource = null;
            thisLoadCts.Dispose();
            Loading = false;
            StateHasChanged();
            return;
        }

        // If we're not using Virtualize, we build and execute a request against the items provider directly
        var startIndex = Pagination is null ? 0 : (Pagination.CurrentPageIndex * Pagination.ItemsPerPage);
        GridItemsProviderRequest<TGridItem> request = new(
            startIndex, Pagination?.ItemsPerPage, [.. _sortColumns], thisLoadCts.Token);
        _lastRefreshedPaginationState = Pagination;

        if (RefreshItems is not null)
        {
            if (IsFixed)
            {
                if (_forceRefreshData || _lastRequest == null)
                {
                    _lastRequest = request;
                    await RefreshItems.Invoke(request);
                }
            }
            else
            {
                if (_forceRefreshData || _lastRequest == null || !_lastRequest.Value.IsSameRequest(request))
                {
                    _lastRequest = request;
                    await RefreshItems.Invoke(request);
                }
            }

            _forceRefreshData = false;
        }

        var result = await ResolveItemsRequestAsync(request);
        if (!thisLoadCts.IsCancellationRequested)
        {
            _internalGridContext.Items = result.Items;
            _internalGridContext.TotalItemCount = result.TotalItemCount;
            if (RefreshItems is null)
            {
                Pagination?.SetTotalItemCountAsync(_internalGridContext.TotalItemCount);
            }

            _pendingDataLoadCancellationTokenSource = null;
            _ = InvokeAsync(() => _internalGridContext.ItemsChanged.InvokeCallbacksAsync(eventArg: null));
        }

        _internalGridContext.ResetRowIndexes(startIndex);
        _ = InvokeAsync(StateHasChanged);
    }

    internal bool ShouldDebounceVirtualizeProviderRequest(long requestTimestamp, bool isRequestCanceled)
    {
        var isRequestBurst = _lastVirtualizeProviderRequestTimestamp != 0
            && Stopwatch.GetElapsedTime(_lastVirtualizeProviderRequestTimestamp, requestTimestamp) < _virtualizeRequestBurstInterval;

        // All request arrivals extend the burst window, including requests that are already canceled.
        _lastVirtualizeProviderRequestTimestamp = requestTimestamp;

        if (isRequestCanceled)
        {
            // Preserve the one-shot bypass for the next request that can invoke the provider.
            return false;
        }

        var skipDelay = _skipNextVirtualizeProviderDelay;
        _skipNextVirtualizeProviderDelay = false;

        return isRequestBurst && !skipDelay;
    }

    // Gets called both by RefreshDataCoreAsync and directly by the Virtualize child component during scrolling
    [ExcludeFromCodeCoverage(Justification = "This method requires Virtualiztion which cannot be tested with bunit.")]
    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "Not going to do artificial optimization because of some random arbitrary determined line count number")]
    internal async ValueTask<ItemsProviderResult<(int, TGridItem)>> ProvideVirtualizedItemsAsync(ItemsProviderRequest request)
    {
        _lastRefreshedPaginationState = Pagination;
        // Debounce rapid requests from scrolling, but do not delay the first request after an idle period,
        // an explicit refresh, or a provider result that changed the total item count.
        if (ShouldDebounceVirtualizeProviderRequest(Stopwatch.GetTimestamp(), request.CancellationToken.IsCancellationRequested))
        {
            await Task.Delay(20, request.CancellationToken)
                .ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing | ConfigureAwaitOptions.ContinueOnCapturedContext);
        }

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
            startIndex, count, [.. _sortColumns], request.CancellationToken);
        var providerResult = await ResolveItemsRequestAsync(providerRequest);

        if (!request.CancellationToken.IsCancellationRequested)
        {
            // ARIA's rowcount is part of the UI, so it should reflect what the human user regards as the number of rows in the table,
            // not the number of physical <tr> elements. For virtualization this means what's in the entire scrollable range, not just
            // the current viewport. In the case where you're also paginating then it means what's conceptually on the current page.
            // TODO: This currently assumes we always want to expand the last page to have ItemsPerPage rows, but the experience might
            //       be better if we let the last page only be as big as its number of actual rows.
            var isFirstProviderResult = !_virtualizeItemsProvided;
            var totalItemCountChanged = _lastVirtualizeProviderTotalItemCount != providerResult.TotalItemCount;

            _lastVirtualizeProviderTotalItemCount = providerResult.TotalItemCount;
            _internalGridContext.TotalItemCount = providerResult.TotalItemCount;
            _internalGridContext.TotalViewItemCount = Pagination?.ItemsPerPage ?? providerResult.TotalItemCount;
            _skipNextVirtualizeProviderDelay |= totalItemCountChanged;
            _virtualizeItemsProvided = true;
            _retainEmptyContentOnVirtualizedRefresh = false;

            if (RefreshItems is null)
            {
                Pagination?.SetTotalItemCountAsync(_internalGridContext.TotalItemCount);
            }

            // The grid (not Virtualize) renders the empty content and the table's aria-rowcount, so it
            // must re-render whenever the provider's answer changes what the grid shows: the first result
            // (which may be "no data") and any change in the total count. Loads that keep the same total
            // (scrolling) are rendered by Virtualize itself and skip this, as before (#5151).
            if (isFirstProviderResult || totalItemCountChanged || Loading != false || _lastError != null)
            {
                Loading = false;
                _ = InvokeAsync(StateHasChanged);
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
    [SuppressMessage("Design", "MA0051:Method is too long", Justification = "Not going to do artificial optimization because of some random arbitrary determined line count number")]
    private async ValueTask<GridItemsProviderResult<TGridItem>> ResolveItemsRequestAsync(GridItemsProviderRequest<TGridItem> request)
    {
        if (_lastError != null)
        {
            _lastError = null;
            StateHasChanged();
        }

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

            if (Items is not null)
            {
                var result = Items;
                if (RefreshItems is null)
                {
                    result = request.ApplySorting(Items).Skip(request.StartIndex);
                    if (request.Count.HasValue)
                    {
                        result = result.Take(request.Count.Value);
                    }
                }

                if (_asyncQueryExecutor is not null)
                {
                    await OnItemsLoading.InvokeAsync(true);

                    var resultArray = Array.Empty<TGridItem>();
                    var totalItemCount = await _asyncQueryExecutor.CountAsync(Items, request.CancellationToken);
                    request.CancellationToken.ThrowIfCancellationRequested();

                    if (!request.Count.HasValue || request.Count.Value > 0)
                    {
                        resultArray = await _asyncQueryExecutor.ToArrayAsync(result, request.CancellationToken);
                        request.CancellationToken.ThrowIfCancellationRequested();
                    }

                    Loading = false;
                    _asyncQueryExecuted = true;
                    _internalGridContext.TotalItemCount = totalItemCount;

                    return GridItemsProviderResult.From(resultArray, totalItemCount);
                }
                else
                {
                    var totalItemCount = Items.Count();
                    _internalGridContext.TotalItemCount = totalItemCount;
                    return GridItemsProviderResult.From([.. result], totalItemCount);
                }
            }
        }
        catch (OperationCanceledException oce) when (oce.CancellationToken == request.CancellationToken) // No-op; we canceled the operation, so it's fine to suppress this exception.
        {
        }
        catch (Exception ex) when (HandleLoadingError?.Invoke(ex) == true)
        {
            _lastError = ex.GetBaseException();
        }
        finally
        {
            if (Items is not null && _asyncQueryExecutor is not null)
            {
                await OnItemsLoading.InvokeAsync(false);
            }
        }

        return GridItemsProviderResult.From(Array.Empty<TGridItem>(), 0);
    }

    /// <summary>
    /// Gets the header's <c>aria-sort</c> value. Only the primary sort column gets a direction: WAI-ARIA states that
    /// authors should apply aria-sort to only one header at a time, so the other sort levels are conveyed through the
    /// header's accessible description instead (see <see cref="SortLevelDescription"/>).
    /// </summary>
    private string AriaSortValue(ColumnBase<TGridItem> column)
         => _sortColumns.Count > 0 && _sortColumns[0].Column == column
             ? (_sortColumns[0].Ascending ? "ascending" : "descending")
             : "none";

    /// <summary>
    /// Gets the text describing which level of a multi-column sort a column is sorted at, or <see langword="null"/>
    /// when there is nothing to add to <c>aria-sort</c>: a single sorted column already announces its direction.
    /// </summary>
    internal string? SortLevelDescription(ColumnBase<TGridItem> column)
    {
        if (SortMode != DataGridSortMode.Multiple || _sortColumns.Count < 2)
        {
            return null;
        }

        var level = GetSortLevel(column);

        return level is null
            ? null
            : Localizer[
                LanguageResource.DataGrid_SortLevel,
                Localizer[level.Value.Ascending ? LanguageResource.DataGrid_SortDirectionAscending : LanguageResource.DataGrid_SortDirectionDescending],
                level.Value.Level,
                _sortColumns.Count];
    }

    private string? StyleValue => DefaultStyleBuilder
        .AddStyle("grid-template-columns", _internalGridTemplateColumns, !string.IsNullOrWhiteSpace(_internalGridTemplateColumns) && DisplayMode == DataGridDisplayMode.Grid)
        .AddStyle("grid-template-rows", "auto 1fr", (_internalGridContext.Items.Count == 0 || Items is null || EffectiveLoadingValue) && DisplayMode == DataGridDisplayMode.Grid)
        // The 100% stretch vertically centers the empty/loading content rows. It must NOT apply
        // while a virtualized grid is still waiting for its first provider result: the only rows
        // are the two zero-height Virtualize spacers, and stretching the table inflates them,
        // defeating Virtualize's startup guard for the after spacer (spacerAfter.offsetHeight > 0).
        // The stale "after spacer visible" observation is then applied once the real count arrives,
        // so the grid jumped to the tail of the data set and back — a visible double flash and two
        // wasted provider round-trips on every first load.
        .AddStyle("height", "100%", (_internalGridContext.TotalItemCount == 0 && (!Virtualize || _virtualizeItemsProvided)) || EffectiveLoadingValue)
        .AddStyle("border-collapse", "separate", GenerateHeader == DataGridGeneratedHeaderType.Sticky)
        .AddStyle("border-spacing", "0", GenerateHeader == DataGridGeneratedHeaderType.Sticky)
        .AddStyle("width", "100%", DisplayMode == DataGridDisplayMode.Table)
        .AddStyle("table-layout", "fixed", DisplayMode == DataGridDisplayMode.Table)
        .Build();

    private string? GridClass => DefaultClassBuilder
            .AddClass("fluent-data-grid")
            .Build();

    private string? ColumnHeaderClass(ColumnBase<TGridItem> column)
    {
        return new CssBuilder(Class)
            .AddClass(ColumnClass(column))
            .Build();
    }

    private Dictionary<string, object?> GetColumnHeaderAttributes(ColumnBase<TGridItem> column, ColumnHeaderCapabilities headerCapabilities)
    {
        var attributes = FluentDataGridCell<TGridItem>.BuildAttributes(this, column, DataGridCellType.ColumnHeader);

        if (GetSortLevel(column) is { } sortLevel)
        {
            attributes["col-sort"] = sortLevel.Ascending ? "asc" : "desc";
        }

        if (ResizableColumns)
        {
            attributes["resizable"] = bool.TrueString.ToLowerInvariant();
        }

        if (!headerCapabilities.CanReorder)
        {
            return attributes;
        }

        attributes["data-column-key"] = column.ColumnKey;
        attributes["reorderable"] = bool.TrueString.ToLowerInvariant();
        attributes["draggable"] = bool.TrueString.ToLowerInvariant();

        return attributes;
    }

    /// <summary>
    /// Returns whether the given column's cells render as the <see cref="FluentDataGridCell{TGridItem}"/>
    /// component. Columns that don't need it render as plain <c>&lt;td&gt;</c> elements, which avoids the
    /// per-cell component overhead. Grid-level cell handlers make every column need the component; the
    /// hierarchical toggle does not, since its content renders the same inside either variant.
    /// </summary>
    private bool CellNeedsComponent(ColumnBase<TGridItem> column)
        => column.RequiresCellComponent || OnCellClick.HasDelegate || OnCellFocus.HasDelegate;

    /// <summary>
    /// Builds the class for a plain <c>&lt;td&gt;</c> cell, using the same builder as <see cref="FluentDataGridCell{TGridItem}"/>.
    /// </summary>
    private static string? PlainCellClass(ColumnBase<TGridItem> column, string? rowClass)
        => FluentDataGridCell<TGridItem>.BuildClass(ColumnClass(column), rowClass, marginClass: null, paddingClass: null);

    /// <summary>
    /// Builds the attributes for a plain <c>&lt;td&gt;</c> cell, using the same builder as <see cref="FluentDataGridCell{TGridItem}"/>.
    /// </summary>
    private Dictionary<string, object?> PlainCellAttributes(ColumnBase<TGridItem> column)
        => FluentDataGridCell<TGridItem>.BuildAttributes(this, column, DataGridCellType.Default);

    /// <summary>
    /// Builds the inline style for a plain <c>&lt;td&gt;</c> cell, using the same builder as <see cref="FluentDataGridCell{TGridItem}"/>.
    /// </summary>
    private string? PlainCellStyle(ColumnBase<TGridItem> column, int gridColumn, string? rowStyle)
        => FluentDataGridCell<TGridItem>.BuildStyle(this, column, _internalGridContext, DataGridCellType.Default, DataGridRowType.Default, gridColumn, column.Style, rowStyle, marginStyle: null, paddingStyle: null);

    private Dictionary<string, object?> PlaceholderCellAttributes(ColumnBase<TGridItem> column)
    {
        var attributes = new Dictionary<string, object?>(PlainCellAttributes(column), StringComparer.OrdinalIgnoreCase)
        {
            ["grid-cell-placeholder"] = bool.TrueString.ToLowerInvariant(),
        };

        return attributes;
    }

    private static string? ColumnClass(ColumnBase<TGridItem> column)
        => column.Class;

    /// <inheritdoc/>
    [ExcludeFromCodeCoverage(Justification = "Tested via integration tests.")]
    public override ValueTask DisposeAsync()
    {
        _currentPageItemsChanged.Dispose();
        _selfReference?.Dispose();
#pragma warning disable MA0042 // Do not use blocking calls in an async method
        _scope?.Dispose();
#pragma warning restore MA0042 // Do not use blocking calls in an async method

        return base.DisposeAsync();
    }

    internal void LoadStateFromQueryString(string queryString)
    {
        if (!SaveStateInUrl)
        {
            return;
        }

        var query = System.Web.HttpUtility.ParseQueryString(queryString);
        if (query.AllKeys.Contains($"{SaveStatePrefix}orderby", StringComparer.Ordinal))
        {
            var sortState = new List<(string Title, bool Ascending)>();

            // One "<title> <asc|desc>" entry per sort level, in priority order.
            foreach (var entry in query[$"{SaveStatePrefix}orderby"]!.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var orderBy = entry.Split(' ', 2);
                sortState.Add((orderBy[0], orderBy.Length == 2 && string.Equals(orderBy[1], "asc", StringComparison.Ordinal)));

                if (SortMode != DataGridSortMode.Multiple)
                {
                    break;
                }
            }

            // The columns are collected during the first render, which happens after OnInitialized calls this.
            // Whichever of the two runs last resolves the titles to columns.
            _pendingSortStateFromUrl = sortState;
            ApplyPendingSortStateFromUrl();
        }

        if (Pagination is not null)
        {
            if (query.AllKeys.Contains($"{SaveStatePrefix}page", StringComparer.Ordinal) && int.TryParse(query[$"{SaveStatePrefix}page"]!, NumberStyles.Integer, CultureInfo.InvariantCulture, out var page))
            {
                _ = Pagination.SetCurrentPageIndexAsync(page - 1);
            }

            if (query.AllKeys.Contains($"{SaveStatePrefix}top", StringComparer.Ordinal) && int.TryParse(query[$"{SaveStatePrefix}top"]!, NumberStyles.Integer, CultureInfo.InvariantCulture, out var itemsPerPage))
            {
                Pagination.ItemsPerPage = itemsPerPage;
            }
        }
    }

    /// <summary>
    /// Resolves sort state that was restored from the query string to the grid's columns, and applies it. Called both
    /// from <see cref="LoadStateFromQueryString"/> and once the columns have been collected, because either of the two
    /// can be the last to run.
    /// </summary>
    private void ApplyPendingSortStateFromUrl()
    {
        if (_pendingSortStateFromUrl is null || _columns.Count == 0)
        {
            return;
        }

        var levels = new List<DataGridSortColumn<TGridItem>>();
        foreach (var (title, ascending) in _pendingSortStateFromUrl)
        {
            var column = _columns.Find(c => string.Equals(c.Title, title, StringComparison.Ordinal));

            // Sort state that no longer matches a sortable column, or that cannot be applied at this level, is dropped
            // rather than failing the render: the query string is user input.
            if (column is null
                || levels.Exists(x => x.Column == column)
                || (levels.Count > 0 && column.SortBy?.CanApplyThen == false))
            {
                continue;
            }

            levels.Add(new DataGridSortColumn<TGridItem>(column, ascending));
        }

        _pendingSortStateFromUrl = null;

        if (levels.Count == 0)
        {
            return;
        }

        _sortColumns.Clear();
        _sortColumns.AddRange(levels);
        UpdateSortAnnouncement();
    }

    private void SaveStateToQueryString()
    {
        if (!SaveStateInUrl)
        {
            return;
        }

        var stateParams = new Dictionary<string, object?>(StringComparer.Ordinal);
        if (_sortColumns.Count > 0)
        {
            var orderBy = string.Join(',', _sortColumns.Select(level => $"{level.Column.Title} {(level.Ascending ? "asc" : "desc")}"));
            stateParams.Add($"{SaveStatePrefix}orderby", orderBy);
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
    /// <param name="visibleRows">The maximum number of rows that fits the available space</param>
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
    }

    /// <summary>
    /// Checks if key pressed should be handled
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public async Task OnKeyDownAsync(FluentKeyCodeEventArgs args)
    {
        if (args.ShiftKey && args.Key == KeyCode.KeyR)
        {
            await ResetColumnWidthsAsync();
        }

        if (args.ShiftKey && args.Key == KeyCode.KeyS)
        {
            await RemoveSortByColumnAsync();
        }

        if (string.Equals(args.Value, "-", StringComparison.Ordinal))
        {
            await SetColumnWidthDiscreteAsync(columnIndex: null, -10);
        }

        if (string.Equals(args.Value, "+", StringComparison.Ordinal))
        {
            await SetColumnWidthDiscreteAsync(columnIndex: null, 10);
        }

        var activeColumn =
            _activeHeaderUiKind == ColumnHeaderUiKind.Reorder
                ? _activeHeaderUiColumn
                : null;

        if (activeColumn is null)
        {
            return;
        }

        var reorderableIndex = GetCurrentReorderableIndex(activeColumn);

        if (activeColumn is not null && args.AltKey && args.Key == KeyCode.KeyF)
        {
            await MoveColumnToStartAsync(activeColumn);
        }

        if (activeColumn is not null && args.AltKey && args.Key == KeyCode.KeyP)
        {
            await MoveColumnToReorderableIndexAsync(activeColumn, --reorderableIndex);
        }

        if (activeColumn is not null && args.AltKey && args.Key == KeyCode.KeyN)
        {
            await MoveColumnToReorderableIndexAsync(activeColumn, ++reorderableIndex);
        }

        if (activeColumn is not null && args.AltKey && args.Key == KeyCode.KeyL)
        {
            await MoveColumnToEndAsync(activeColumn);
        }
    }

    /// <summary>
    /// Resizes the column width by a discrete amount.
    /// </summary>
    /// <param name="columnIndex">The column to be resized</param>
    /// <param name="widthChange">The amount of pixels to change width with</param>
    /// <returns></returns>
    public async Task SetColumnWidthDiscreteAsync(int? columnIndex, float widthChange)
    {
        if (_gridReference is not null && JSModule is not null)
        {
            await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.DataGrid.ResizeColumnDiscrete", _gridReference, columnIndex, widthChange);
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
        if (_gridReference is not null && JSModule is not null)
        {
            await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.DataGrid.ResizeColumnExact", _gridReference, columnIndex, width);
        }
    }

    /// <summary>
    /// Resets the column widths to their initial values as specified with the <see cref="GridTemplateColumns"/> parameter.
    /// If no value is specified, the default value is "1fr" for each column.
    /// </summary>
    /// <returns></returns>
    public async Task ResetColumnWidthsAsync()
    {
        if (_gridReference is not null && JSModule is not null)
        {
            await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.DataGrid.ResetColumnWidths", _gridReference);
        }
    }

    /// <summary>
    /// Expands all rows in a hierarchical data grid.
    /// Items must implement the <see cref="IHierarchicalGridItem"/> interface.
    /// </summary>
    public async Task ExpandAllHierarchicalRowsAsync(int startDepth = 0)
    {
        var hasChildren = false;
        await RefreshDataAsync();

        foreach (var item in _internalGridContext.Items)
        {
            if (item is IHierarchicalGridItem hierarchicalItem && hierarchicalItem.Depth == startDepth)
            {
                hierarchicalItem.IsCollapsed = false;
                hierarchicalItem.IsHidden = false;
                if (hierarchicalItem.HasChildren)
                {
                    hasChildren = true;
                }
            }
        }

        if (hasChildren)
        {
            await ExpandAllHierarchicalRowsAsync(startDepth + 1);
        }
        else
        {
            if (OnExpandAll.HasDelegate)
            {
                await OnExpandAll.InvokeAsync();
            }
        }
    }

    /// <summary>
    /// Collapses all rows in a hierarchical data grid with a depth greater than 0.
    /// Items must implement the <see cref="IHierarchicalGridItem"/> interface.
    /// </summary>
    public async Task CollapseAllHierarchicalRowsAsync()
    {
        foreach (var item in _internalGridContext.Items)
        {
            if (item is IHierarchicalGridItem hierarchicalItem && hierarchicalItem.Depth > 0)
            {
                hierarchicalItem.IsCollapsed = true;
                hierarchicalItem.IsHidden = true;
            }
        }

        if (OnCollapseAll.HasDelegate)
        {
            await OnCollapseAll.InvokeAsync();
        }
    }

    /// <summary>
    /// Selects all rows in a hierarchical data grid.
    /// Items must implement the <see cref="IHierarchicalGridItem"/> interface.
    /// </summary>
    public async Task SelectAllHierarchicalRowsAsync()
    {
        foreach (var item in _internalGridContext.Items)
        {
            if (item is IHierarchicalGridItem hierarchicalItem && hierarchicalItem.Depth == 0)
            {
                hierarchicalItem.IsSelected = true;
            }
        }

        await RefreshDataAsync();
    }

    /// <summary>
    /// Deselects all rows in a hierarchical data grid.
    /// Items must implement the <see cref="IHierarchicalGridItem"/> interface.
    /// </summary>
    public async Task DeselectAllHierarchicalRowsAsync()
    {
        foreach (var item in _internalGridContext.Items)
        {
            if (item is IHierarchicalGridItem hierarchicalItem && hierarchicalItem.Depth == 0)
            {
                hierarchicalItem.IsSelected = false;
            }
        }

        await RefreshDataAsync();
    }

    private void RenderActualError(RenderTreeBuilder builder)
    {
        if (ErrorContent is null)
        {
            builder.AddContent(0, Localizer[Localization.LanguageResource.DataGrid_ErrorContent]);

        }
        else
        {
            builder.AddContent(1, ErrorContent(_lastError));

        }
    }

    private async Task ToggleExpandedAsync(TGridItem item)
    {
        if (item is IHierarchicalGridItem hierarchicalItem)
        {
            hierarchicalItem.IsCollapsed = !hierarchicalItem.IsCollapsed;
            if (OnToggle.HasDelegate)
            {
                await OnToggle.InvokeAsync(item);
            }

            await RefreshDataAsync();
        }
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="RowDetails"/> content of the specified <paramref name="item"/> is currently expanded.
    /// </summary>
    /// <param name="item">The item that holds the row's values.</param>
    public bool IsRowDetailsExpanded(TGridItem item) => _expandedRowDetails.Contains(ItemKey(item));

    /// <summary>
    /// Expands or collapses the <see cref="RowDetails"/> content of the specified <paramref name="item"/>.
    /// </summary>
    /// <param name="item">The item that holds the row's values.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public async Task ToggleRowDetailsAsync(TGridItem item)
    {
        var key = ItemKey(item);
        if (!_expandedRowDetails.Remove(key))
        {
            _expandedRowDetails.Add(key);
        }

        if (OnRowDetailsToggle.HasDelegate)
        {
            await OnRowDetailsToggle.InvokeAsync(item);
        }

        StateHasChanged();
    }

    /// <summary>
    /// Expands the <see cref="RowDetails"/> content of the specified <paramref name="item"/>.
    /// </summary>
    /// <param name="item">The item that holds the row's values.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task ExpandRowDetailsAsync(TGridItem item)
        => IsRowDetailsExpanded(item) ? Task.CompletedTask : ToggleRowDetailsAsync(item);

    /// <summary>
    /// Collapses the <see cref="RowDetails"/> content of the specified <paramref name="item"/>.
    /// </summary>
    /// <param name="item">The item that holds the row's values.</param>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public Task CollapseRowDetailsAsync(TGridItem item)
        => IsRowDetailsExpanded(item) ? ToggleRowDetailsAsync(item) : Task.CompletedTask;

    /// <summary>
    /// Expands the <see cref="RowDetails"/> content of all currently loaded rows.
    /// Raises <see cref="OnRowDetailsToggle"/> for each row that was not already expanded.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public async Task ExpandAllRowDetailsAsync()
    {
        foreach (var item in _internalGridContext.Items)
        {
            if (_expandedRowDetails.Add(ItemKey(item)) && OnRowDetailsToggle.HasDelegate)
            {
                await OnRowDetailsToggle.InvokeAsync(item);
            }
        }

        StateHasChanged();
    }

    /// <summary>
    /// Collapses the <see cref="RowDetails"/> content of all rows.
    /// Raises <see cref="OnRowDetailsToggle"/> for each currently loaded row that was expanded.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the completion of the operation.</returns>
    public async Task CollapseAllRowDetailsAsync()
    {
        if (OnRowDetailsToggle.HasDelegate)
        {
            foreach (var item in _internalGridContext.Items)
            {
                if (_expandedRowDetails.Contains(ItemKey(item)))
                {
                    await OnRowDetailsToggle.InvokeAsync(item);
                }
            }
        }

        _expandedRowDetails.Clear();
        StateHasChanged();
    }

    // Distinct @key for the extra details row rendered below the master row
    private object RowDetailsKey(TGridItem item) => (ItemKey(item), nameof(RowDetails));
}
