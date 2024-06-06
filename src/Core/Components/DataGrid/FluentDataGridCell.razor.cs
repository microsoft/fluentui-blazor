// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
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
    public DataGridCellType? CellType { get; set; } = DataGridCellType.Default;

    /// <summary>
    /// Gets or sets the column of the cell.
    /// </summary>
    [Parameter]
    public ColumnBase<TGridItem>? Column { get; set; }

    /// <summary>
    /// Gets or sets the column index of the cell.
    /// This will be applied to the css grid-column-index value applied to the cell.
    /// </summary>
    [Parameter]
    public int ColumnIndex { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the owning <see cref="FluentDataGridRow{TItem}"/> component.
    /// </summary>
    [CascadingParameter(Name = "OwningRow")]
    public FluentDataGridRow<TGridItem> Owner { get; set; } = default!;

    /// <summary>
    /// Gets or sets the owning <see cref="FluentDataGrid{TItem}"/> component
    /// </summary>
    [CascadingParameter]
    private InternalGridContext<TGridItem> GridContext { get; set; } = default!;

    protected string? StyleValue => new StyleBuilder(Style)
       .AddStyle("height", $"{GridContext.Grid.ItemSize:0}px", () => GridContext.Grid.Virtualize && Owner.RowType == DataGridRowType.Default)
       .AddStyle("align-content", "center", () => GridContext.Grid.Virtualize && Owner.RowType == DataGridRowType.Default && string.IsNullOrEmpty(Style))
       .Build();

    protected override void OnInitialized()
    {
        Owner.Register(this);
    }

    public void Dispose() => Owner.Unregister(this);

    /// <summary />
    internal async Task HandleOnCellClickAsync(string cellId)
    {
        if (Owner.Cells.TryGetValue(cellId, out var cell))
        {
            if (GridContext.Grid.OnCellClick.HasDelegate)
            {
                await GridContext.Grid.OnCellClick.InvokeAsync(cell);
            }

            if (cell != null && cell.CellType == DataGridCellType.Default)
            {
                if (Column is SelectColumn<TGridItem> selColumn)
                {
                    await selColumn.AddOrRemoveSelectedItemAsync(Item);
                }
            }
        }
    }
}
