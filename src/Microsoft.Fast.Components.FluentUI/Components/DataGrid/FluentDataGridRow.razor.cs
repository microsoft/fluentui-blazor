using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

[CascadingTypeParameter(nameof(TGridItem))]
public partial class FluentDataGridRow<TGridItem> : FluentComponentBase, IDisposable
{
    internal static int index = 0;
    internal string RowId { get; } = Identifier.NewId();
    private readonly Dictionary<string, FluentDataGridCell<TGridItem>> cells = new();

    /// <summary>
    /// Gets or sets the index of this row
    /// </summary>
    [Parameter]
    public int? RowIndex { get; set; }

    /// <summary>
    /// String that gets applied to the the css gridTemplateColumns attribute for the row
    /// </summary>
    [Parameter]
    public string? GridTemplateColumns { get; set; } = null;

    /// <summary>
    /// Gets or sets the type of row. See <see cref="DataGridRowType"/>
    /// </summary>
    [Parameter]
    public DataGridRowType? RowType { get; set; } = DataGridRowType.Default;

    [Parameter]
    public EventCallback<FluentDataGridCell<TGridItem>> OnCellFocus { get; set; }

    /// <summary>
    /// Gets or sets the owning <see cref="FluentDataGrid{TItem}"/> component
    /// </summary>
    [CascadingParameter]
    private InternalGridContext<TGridItem> Owner { get; set; } = default!;

    private string? _rowsDataSize = null;

    protected override void OnInitialized()
    {
        Owner.Register(this);
        RowIndex = index++;
    }

    protected override void OnParametersSet()
    {
        if (Owner.Grid.Virtualize)
            _rowsDataSize = $"height: {Owner.Grid.RowsDataSize}px";
    }

    public void Dispose()
    {
        Owner.Unregister(this);
    }

    internal void Register(FluentDataGridCell<TGridItem> cell)
    {
        cells.Add(cell.CellId, cell);
    }

    internal void Unregister(FluentDataGridCell<TGridItem> cell)
    {
        cells.Remove(cell.CellId);
    }

    private async Task HandleOnCellFocus(DataGridCellFocusEventArgs args)
    {
        string? cellId = args.CellId;
        if (cells.TryGetValue(cellId!, out FluentDataGridCell<TGridItem>? cell))
        {
            await OnCellFocus.InvokeAsync(cell);
        }
    }

}