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
        .AddClass("multiline-text", when: Grid.MultiLine)
        .AddClass(Owner.Class)
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("grid-column", GridColumn.ToString(), () => (!Grid.Loading && Grid.Items is not null) || Grid.Virtualize)
        .AddStyle("text-align", "center", Column is SelectColumn<TGridItem>)
        .AddStyle("align-content", "center", Column is SelectColumn<TGridItem>)
        .AddStyle("padding-inline-start", "calc(((var(--design-unit)* 3) + var(--focus-stroke-width) - var(--stroke-width))* 1px)", Column is SelectColumn<TGridItem> && Owner.RowType == DataGridRowType.Default)
        .AddStyle("padding-top", "calc(var(--design-unit) * 2.5px)", Column is SelectColumn<TGridItem> && (Grid.RowSize == DataGridRowSize.Medium || Owner.RowType == DataGridRowType.Header))
        .AddStyle("padding-top", "calc(var(--design-unit) * 1.5px)", Column is SelectColumn<TGridItem> && Grid.RowSize == DataGridRowSize.Small && Owner.RowType == DataGridRowType.Default)
        .AddStyle("height", $"{Grid.ItemSize:0}px", () => !Grid.Loading && Grid.Virtualize && Owner.RowType == DataGridRowType.Default)
        .AddStyle("height", $"{(int)Grid.RowSize}px", () => !Grid.Loading && !Grid.Virtualize && Grid.Items is not null && !Grid.MultiLine)
        .AddStyle("height", "100%", InternalGridContext.TotalItemCount == 0 || (Grid.Loading && (Grid.MultiLine || Grid.Items is null)))
        .AddStyle("min-height", "44px", Owner.RowType != DataGridRowType.Default)
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

}
