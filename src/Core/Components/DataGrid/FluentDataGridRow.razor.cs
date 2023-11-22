using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

[CascadingTypeParameter(nameof(TGridItem))]
public partial class FluentDataGridRow<TGridItem> : FluentComponentBase, IHandleEvent, IDisposable
{
    internal string RowId { get; } = Identifier.NewId();
    private readonly Dictionary<string, FluentDataGridCell<TGridItem>> cells = [];

    /// <summary>
    /// Gets or sets the reference to the item that holds this row's values.
    /// </summary>
    [Parameter]
    public TGridItem? Item { get; set; }

    /// <summary>
    /// Gets or sets the index of this row.
    /// When FluentDataGrid is virtualized, this value is not used.
    /// </summary>
    [Parameter]
    public int? RowIndex { get; set; }

    /// <summary>
    /// Gets or sets the string that gets applied to the css gridTemplateColumns attribute for the row.
    /// </summary>
    [Parameter]
    public string? GridTemplateColumns { get; set; } = null;

    /// <summary>
    /// Gets or sets the type of row. See <see cref="DataGridRowType"/>.
    /// </summary>
    [Parameter]
    public DataGridRowType? RowType { get; set; } = DataGridRowType.Default;

    [Parameter]
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback<FluentDataGridCell<TGridItem>> OnCellFocus { get; set; }

    /// <summary>
    /// Gets or sets the owning <see cref="FluentDataGrid{TItem}"/> component
    /// </summary>
    [CascadingParameter]
    private InternalGridContext<TGridItem> Owner { get; set; } = default!;

    protected string? StyleValue => new StyleBuilder(Style)
       .AddStyle("height", $"{Owner.Grid.ItemSize}px", () => Owner.Grid.Virtualize && RowType == DataGridRowType.Default)
       .AddStyle("align-items", "center", () => Owner.Grid.Virtualize && RowType == DataGridRowType.Default && string.IsNullOrEmpty(Style))
       .Build();

    protected override void OnInitialized()
    {
        Owner.Register(this);
    }

    public void Dispose() => Owner.Unregister(this);

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
            await Owner.Grid.OnCellFocus.InvokeAsync(cell);
        }
    }

    Task IHandleEvent.HandleEventAsync(
       EventCallbackWorkItem callback, object? arg) => callback.InvokeAsync(arg);
}