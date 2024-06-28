using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;
public partial class ColumnResizeOptions<TGridItem>
{
    private string? _width;

    [CascadingParameter]
    internal InternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;

    /// <summary>
    /// Gets a reference to the enclosing <see cref="FluentDataGrid{TGridItem}" />.
    /// </summary>
    private FluentDataGrid<TGridItem> Grid => InternalGridContext.Grid;

    /// <summary>
    /// Gets or sets the index of the Column to act upon
    /// </summary>
    [Parameter]
    public int Column { get; set; }

    /// <summary>
    /// Gets or sets the label to display above the resize options
    /// </summary>
    [Parameter]
    public string? Label{ get; set; }

    /// <summary>
    /// Gets or sets the type of resize to perform
    /// Discrete: resize by a 10 pixels at a time
    /// Exact: resize to the exact width specified (in pixels)
    /// The display of this component is dependant on a ResizeType being set
    /// </summary>
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
