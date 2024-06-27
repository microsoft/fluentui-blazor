using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;
public partial class ColumnResizeOptions<TGridItem>
{
    private string? _width;
    [CascadingParameter] internal InternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;

    /// <summary>
    /// Gets a reference to the enclosing <see cref="FluentDataGrid{TGridItem}" />.
    /// </summary>
    public FluentDataGrid<TGridItem> Grid => InternalGridContext.Grid;

    [Parameter]
    public int Column { get; set; }

    [Parameter]
    public string? Label{ get; set; }

    [Parameter]
    public DataGridResizeType? ResizeType { get; set; } = DataGridResizeType.Discrete;

    protected override void OnParametersSet()
    {
        if (Column == 0)
        {
            throw new ArgumentException("Column must have a value greater than zero");
        }

        Label = Grid.ResizeLabel;
    }

    private async Task HandleShrinkAsync()
    {
        await Grid.SetColumnWidthDiscreteAsync(Column, -10);
    }

    private async Task HandleGrowAsync()
    {
        await Grid.SetColumnWidthDiscreteAsync(Column, 10);
    }

    private async Task HandleResetAsync()
    {
        await Grid.ResetColumnWidthsAsync();
    }

    private async Task HandleColumnWidthAsync()
    {

        var valid = int.TryParse(_width, out var result);
        if (valid)
        {
            await Grid.SetColumnWidthExactAsync(Column, result);
        }
    }

    private async Task HandleColumnWidthKeyDownAsync(KeyboardEventArgs args)
    {
        if (args.Key == "Enter")
        {
            await HandleColumnWidthAsync();
        }
    }
}
