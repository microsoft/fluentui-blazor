using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDataGridRow<TItem> : FluentComponentBase
{
    // FAST Attributes
    [Parameter]
    public int? RowIndex { get; set; }

    [Parameter]
    public string? GridTemplateColumns { get; set; } = null;
    [Parameter]
    public DataGridRowType? RowType { get; set; } = DataGridRowType.Default;
    // FAST Properties
    [Parameter]
    public TItem? RowData { get; set; }

    [Parameter]
    public IEnumerable<ColumnDefinition<TItem>>? ColumnDefinitions { get; set; }
}