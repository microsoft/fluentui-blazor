// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentDataGridCell<TGridItem> : FluentComponentBase
{
    internal string CellId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the reference to the item that holds this cell's values.
    /// </summary>
    [Parameter]
    public TGridItem? Item { get; set; }

    /// <summary>
    /// Gets or sets the cell type. See <see cref="DataGridCellType"/>.
    /// </summary>
    [Parameter]
    public DataGridCellType CellType { get; set; }

    /// <summary>
    /// Gets or sets the column index of the cell.
    /// This will be applied to the css grid-column-index value applied to the cell.
    /// </summary>
    [Parameter]
    public int GridColumn { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the owning <see cref="FluentDataGridRow{TItem}"/> component.
    /// </summary>
    [CascadingParameter(Name = "OwningRow")]
    internal FluentDataGridRow<TGridItem> Owner { get; set; } = default!;

    /// <summary>
    /// Gets or sets the owning <see cref="FluentDataGrid{TItem}"/> component
    /// </summary>
    [CascadingParameter]
    internal InternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;

    /// <summary>
    /// Gets a reference to the column that this cell belongs to.
    /// </summary>
    private ColumnBase<TGridItem>? Column => Grid._columns.ElementAtOrDefault(GridColumn - 1);

    /// <summary>
    /// Gets a reference to the enclosing <see cref="FluentDataGrid{TGridItem}" />.
    /// </summary>
    protected FluentDataGrid<TGridItem> Grid => InternalGridContext.Grid;

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("column-header", when: CellType == DataGridCellType.ColumnHeader)
        .AddClass("select-all", when: CellType == DataGridCellType.ColumnHeader && Column is SelectColumn<TGridItem>)
        .AddClass("multiline-text", when: Grid.MultiLine && (Grid.Items is not null || Grid.ItemsProvider is not null) && CellType != DataGridCellType.ColumnHeader)
        .AddClass(Owner.Class)
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("grid-column", GridColumn.ToString(), () => (!Grid.EffectiveLoadingValue && (Grid.Items is not null || Grid.ItemsProvider is not null)) || Grid.Virtualize)
        .AddStyle("text-align", "center", Column is SelectColumn<TGridItem>)
        .AddStyle("align-content", "center", Column is SelectColumn<TGridItem>)
        .AddStyle("padding-inline-start", "calc(((var(--design-unit)* 3) + var(--focus-stroke-width) - var(--stroke-width))* 1px)", Column is SelectColumn<TGridItem> && Owner.RowType == DataGridRowType.Default)
        .AddStyle("padding-top", "calc(var(--design-unit) * 2.5px)", Column is SelectColumn<TGridItem> && (Grid.RowSize == DataGridRowSize.Medium || Owner.RowType == DataGridRowType.Header))
        .AddStyle("padding-top", "calc(var(--design-unit) * 1.5px)", Column is SelectColumn<TGridItem> && Grid.RowSize == DataGridRowSize.Small && Owner.RowType == DataGridRowType.Default)
        .AddStyle("width", Column?.Width, !string.IsNullOrEmpty(Column?.Width) && Grid.DisplayMode == DataGridDisplayMode.Table)
        .AddStyle("height", $"{Grid.ItemSize:0}px", () => !Grid.EffectiveLoadingValue && Grid.Virtualize && Owner.RowType == DataGridRowType.Default)
        .AddStyle("height", $"{(int)Grid.RowSize}px", () => !Grid.EffectiveLoadingValue && !Grid.Virtualize && !Grid.MultiLine && (Grid.Items is not null || Grid.ItemsProvider is not null))
        .AddStyle("height", "100%", Grid.MultiLine)
        .AddStyle("min-height", "44px", Owner.RowType != DataGridRowType.Default)
        .AddStyle("display", "flex", ShouldHaveDisplayFlex())
        .AddStyle("z-index", ZIndex.DataGridHeaderPopup.ToString(), CellType == DataGridCellType.ColumnHeader && Grid._columns.Count > 0)
        .AddStyle(Owner.Style)
        .Build();

    protected override void OnInitialized()
    {
        Owner.Register(this);
    }

    /// <summary />
    internal async Task HandleOnCellClickAsync()
    {
        if (Grid.OnCellClick.HasDelegate)
        {
            await Grid.OnCellClick.InvokeAsync(this);
        }

        if (Column != null)
        {
            await Column.OnCellClickAsync(this);
        }
    }

    internal async Task HandleOnCellFocusAsync()
    {
        if (CellType == DataGridCellType.Default)
        {
            await Grid.OnCellFocus.InvokeAsync(this);
        }
    }

    internal async Task HandleOnCellKeyDownAsync(KeyboardEventArgs e)
    {
        if (!SelectColumn<TGridItem>.KEYBOARD_SELECT_KEYS.Contains(e.Code))
        {
            return;
        }

        if (Column != null)
        {
            await Column.OnCellKeyDownAsync(this, e);
        }
    }

    public void Dispose() => Owner.Unregister(this);

    private bool ShouldHaveDisplayFlex()
    {

        var isHeaderCell = CellType == DataGridCellType.ColumnHeader;

        if (!isHeaderCell)
        {
            return false;
        }

        var isButtonWithMenu = Grid.HeaderCellAsButtonWithMenu;
        var isResizable = Grid.ResizableColumns;
        var isNotResizableWithOptions = !Grid.ResizableColumns && Column?.ColumnOptions is not null;
        var isSelectColumn = Column?.GetType() == typeof(SelectColumn<TGridItem>);
        //var isColumnNull = Column is null;
        var isSortable = (Column?.Sortable.HasValue ?? false) && Column?.Sortable.Value == true;
        var isSortByNull = Column?.SortBy is null;
        var isColumnsCountGreaterThanZero = Grid._columns.Count > 0;

        return isHeaderCell && !isButtonWithMenu && (isResizable || isNotResizableWithOptions) && !isSelectColumn && isColumnsCountGreaterThanZero && (!isSortable || isSortByNull);
    }
}
