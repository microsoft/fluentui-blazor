using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentDataGridCell
{
    // FAST Attributes
    [Parameter]
    public DataGridCellType? CellType { get; set; } = DataGridCellType.Default;
    [Parameter]
    public int GridColumn { get; set; }

    // General Blazor parameters
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? AdditionalAttributes { get; set; }
}
