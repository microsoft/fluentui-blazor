// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a cell in a <see cref="FluentDataGrid{TGridItem}"/>.
/// </summary>
public partial class FluentDataGridCell<TGridItem> : FluentComponentBase
{
    /// <summary>
    /// Gets a reference to the column that this cell belongs to.
    /// </summary>
    public ColumnBase<TGridItem>? Column => Grid._columns.ElementAtOrDefault(GridColumn - 1);

    internal string CellId { get; set; } = string.Empty;

    /// <summary />
    public FluentDataGridCell(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    protected string? ClassValue => BuildClass(Grid, Column, CellType, Class, Owner.Class, Margin.ConvertSpacing().Class, Padding.ConvertSpacing().Class);

    /// <summary />
    protected string? StyleValue => BuildStyle(Grid, Column, InternalGridContext, CellType, Owner.RowType, GridColumn, Style, Owner.Style, Margin.ConvertSpacing().Style, Padding.ConvertSpacing().Style);

    /// <summary>
    /// Builds the CSS class for a grid cell. Also used by <see cref="FluentDataGrid{TGridItem}"/> when it
    /// renders cells as plain <c>&lt;td&gt;</c> elements (which pass a null margin/padding).
    /// </summary>
    internal static string? BuildClass(
        FluentDataGrid<TGridItem> grid,
        ColumnBase<TGridItem>? column,
        DataGridCellType cellType,
        string? baseClass,
        string? ownerClass,
        string? marginClass,
        string? paddingClass)
        => new CssBuilder(baseClass)
            .AddClass(marginClass)
            .AddClass(paddingClass)
            .AddClass("column-header", when: cellType == DataGridCellType.ColumnHeader)
            .AddClass("select-all", when: cellType == DataGridCellType.ColumnHeader && column is SelectColumn<TGridItem>)
            .AddClass("multiline-text", when: grid.MultiLine && (grid.Items is not null || grid.ItemsProvider is not null) && cellType != DataGridCellType.ColumnHeader)
            .AddClass("col-pinned-start", column?.Pin == DataGridColumnPin.Start)
            .AddClass("col-pinned-end", column?.Pin == DataGridColumnPin.End)
            .AddClass(ownerClass)
            .Build();

    /// <summary>
    /// Builds the inline style for a grid cell. Also used by <see cref="FluentDataGrid{TGridItem}"/> when it
    /// renders cells as plain <c>&lt;td&gt;</c> elements.
    /// </summary>
    internal static string? BuildStyle(
        FluentDataGrid<TGridItem> grid,
        ColumnBase<TGridItem>? column,
        InternalGridContext<TGridItem> gridContext,
        DataGridCellType cellType,
        DataGridRowType ownerRowType,
        int gridColumn,
        string? baseStyle,
        string? ownerStyle,
        string? marginStyle,
        string? paddingStyle)
        => new StyleBuilder(baseStyle)
            .AddStyle("margin", marginStyle)
            .AddStyle("padding", paddingStyle)
            .AddStyle("grid-column", gridColumn.ToString(CultureInfo.InvariantCulture), () => !grid.EffectiveLoadingValue && (grid.Items is not null || grid.ItemsProvider is not null) && gridContext.TotalItemCount > 0 && grid.DisplayMode == DataGridDisplayMode.Grid)
            .AddStyle("text-align", "center", column is SelectColumn<TGridItem>)
            .AddStyle("align-content", "center", column is SelectColumn<TGridItem>)
            .AddStyle("min-width", column?.MinWidth, ownerRowType is DataGridRowType.Header or DataGridRowType.StickyHeader && (grid.HeaderCellAsButtonWithMenu || column?.Pin != DataGridColumnPin.None))
            .AddStyle("padding-top", "10px", column is not HierarchicalSelectColumn<TGridItem> && column is SelectColumn<TGridItem> && (grid.RowSize == DataGridRowSize.Medium || ownerRowType == DataGridRowType.Header))
            .AddStyle("padding-top", "6px", column is SelectColumn<TGridItem> && grid.RowSize == DataGridRowSize.Small && ownerRowType == DataGridRowType.Default)
            .AddStyle("width", column?.Width, !string.IsNullOrEmpty(column?.Width) && grid.DisplayMode == DataGridDisplayMode.Table)
            .AddStyle("height", $"{grid.ItemSize.ToString(CultureInfo.InvariantCulture):0}px", () => !grid.EffectiveLoadingValue && grid.Virtualize)
            .AddStyle("height", $"{((int)grid.RowSize).ToString(CultureInfo.InvariantCulture)}px", () => !grid.EffectiveLoadingValue && !grid.Virtualize && !grid.MultiLine && (grid.Items is not null || grid.ItemsProvider is not null) && gridContext.TotalItemCount > 0)
            .AddStyle("height", "100%", grid.MultiLine)
            .AddStyle("min-height", "40px", ownerRowType != DataGridRowType.Default)
            .AddStyle("position", "sticky", column != null && column.Pin != DataGridColumnPin.None)
            .AddStyle("inset-inline-start", $"{column?.PinOffset}", column != null && column.Pin == DataGridColumnPin.Start)
            .AddStyle("inset-inline-end", $"{column?.PinOffset}", column != null && column.Pin == DataGridColumnPin.End)
            .AddStyle("z-index", "1", column != null && column.Pin != DataGridColumnPin.None && cellType == DataGridCellType.Default)
            .AddStyle(ownerStyle)
            .Build();

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
    /// Gets or sets the text used for the cell's <c>title</c> and <c>aria-label</c> attributes.
    /// </summary>
    [Parameter]
    public string? CellTitle { get; set; }

    /// <summary>
    /// Gets the tabindex for a focusable body cell, or <see langword="null"/> when the cell is not focusable.
    /// </summary>
    private string? CellTabIndex =>
        CellType == DataGridCellType.Default && Column is not null && !Column.DisableCellFocus ? "0" : null;

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
    /// Gets a reference to the enclosing <see cref="FluentDataGrid{TGridItem}" />.
    /// </summary>
    protected FluentDataGrid<TGridItem> Grid => InternalGridContext.Grid;

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        Owner.Unregister(this);
        return base.DisposeAsync();
    }

    /// <summary>
    /// Only wire the click/focus/keydown handlers when something actually handles them.
    /// </summary>
    internal bool WireCellEvents =>
        CellType != DataGridCellType.Default
        || Grid.OnCellClick.HasDelegate
        || Grid.OnCellFocus.HasDelegate
        || Column is SelectColumn<TGridItem>;

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
        if (!SelectColumn<TGridItem>.KEYBOARD_SELECT_KEYS.Contains(e.Code, StringComparer.OrdinalIgnoreCase))
        {
            return;
        }

        if (Column != null)
        {
            await Column.OnCellKeyDownAsync(this, e);
        }
    }

    /// <summary />
    protected override void OnInitialized()
    {
        Owner.Register(this);
    }
}
