using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentDataGridCell : FluentComponentBase
{
    // FAST Attributes
    [Parameter]
    public DataGridCellType? CellType { get; set; } = DataGridCellType.Default;
    [Parameter]
    public int GridColumn { get; set; }
}
