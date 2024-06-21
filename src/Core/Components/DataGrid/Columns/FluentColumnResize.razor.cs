using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;
public partial class FluentColumnResize<TGridItem>
{
    private string? _width;
    [CascadingParameter] internal InternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;

    /// <summary>
    /// Gets a reference to the enclosing <see cref="FluentDataGrid{TGridItem}" />.
    /// </summary>
    public FluentDataGrid<TGridItem> Grid => InternalGridContext.Grid;

    [Parameter]
    public string? Label{ get; set; } = "Resize";

    [Parameter]
    public DataGridResizeType? ResizeType { get; set; } = DataGridResizeType.Discreet;

    private async Task HandleShrinkAsync()
    {
        await Grid.SetColumnWidthAsync(-10);
    }

    private async Task HandleGrowAsync()
    {
        await Grid.SetColumnWidthAsync(10);
    }

    private async Task HandleResetAsync()
    {
        await Grid.ResetColumnWidthsAsync();
    }
}
