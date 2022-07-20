using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

[CascadingTypeParameter(nameof(TItem))]
public partial class FluentDataGridRow<TItem> : FluentComponentBase, IDisposable
{
    internal static int index = 0;
    internal string RowId { get; } = Identifier.NewId();
    private readonly Dictionary<string, FluentDataGridCell> cells = new();

    /// <summary>
    /// Gets or sets the index of this row
    /// </summary>
    [Parameter]
    public int? RowIndex { get; set; }

    /// <summary>
    /// Gets or sets the value that gets applied to the css gridTemplateColumns attribute of child rows
    /// </summary>
    [Parameter]
    public string? GridTemplateColumns { get; set; } = null;

    /// <summary>
    /// Gets or sets the type of row. Se <see cref="DataGridRowType"/>
    /// </summary>
    [Parameter]
    public DataGridRowType? RowType { get; set; } = DataGridRowType.Default;

    /// <summary>
    /// Gets or sets the row data
    /// </summary>
    [Parameter]
    public TItem? RowData { get; set; }

    /// <summary>
    /// Gets or sets the column definitions for the row. See <see cref="ColumnDefinition{TItem}"/>
    /// </summary>
    [Parameter]
    public IEnumerable<ColumnDefinition<TItem>>? ColumnDefinitions { get; set; }

    /// <summary>
    /// Gets or sets the owning <see cref="FluentDataGrid{TItem}"/> component
    /// </summary>
    [CascadingParameter(Name = "OwningGrid")]
    public FluentDataGrid<TItem> Owner { get; set; } = default!;

    protected override void OnInitialized()
    {
        Owner.Register(this);
        RowIndex = index++;
    }

    public void Dispose()
    {
        Owner.Unregister(this);
    }

    internal void Register(FluentDataGridCell cell)
    {
        cells.Add(cell.CellId, cell);
    }

    internal void Unregister(FluentDataGridCell cell)
    {
        cells.Remove(cell.CellId);
    }

    //private async Task HandleOnCellFocus(DataGridCellFocusEventArgs args)
    //{
    //    string? cellId = args.CellId;
    //    if (rows.TryGetValue(cellId!, out FluentDataGridCell<TItem>? cell))
    //    {
    //        await OnCellFocus.InvokeAsync(cell);
    //    }
    //}

}