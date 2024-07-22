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
    public DataGridCellType? CellType { get; set; } = DataGridCellType.Default;

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
    private InternalGridContext<TGridItem> GridContext { get; set; } = default!;

    /// <summary>
    /// Gets a reference to the column that this cell belongs to.
    /// </summary>
    private ColumnBase<TGridItem>? Column => Owner.Owner.Grid._columns.ElementAtOrDefault(GridColumn - 1);

    protected string? StyleValue => new StyleBuilder(Style)
       .AddStyle("height", $"{GridContext.Grid.ItemSize:0}px", () => !GridContext.Grid.Loading && GridContext.Grid.Virtualize && Owner.RowType == DataGridRowType.Default)
       .AddStyle("align-content", "center", () => !GridContext.Grid.Loading && GridContext.Grid.Virtualize && Owner.RowType == DataGridRowType.Default && string.IsNullOrEmpty(Style))
       .Build();

    protected override void OnInitialized()
    {
        Owner.Register(this);
    }

    /// <summary />
    internal async Task HandleOnCellClickAsync()
    {
        if (GridContext.Grid.OnCellClick.HasDelegate)
        {
            await GridContext.Grid.OnCellClick.InvokeAsync(this);
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
