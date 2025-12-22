// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides options for resizing a column in a <see cref="FluentDataGrid{TGridItem}" />.
/// </summary>
/// <remarks>This class allows configuring the resizing behavior for a specific column in the grid.  The resizing
/// can be performed either in discrete steps or to an exact width, depending on the <see cref="ResizeType"/>.</remarks>
/// <typeparam name="TGridItem">The type of the data items displayed in the grid.</typeparam>
public partial class ColumnResizeOptions<TGridItem>
{
    private string? _width;

    /// <summary />
    [Inject]
    public IFluentLocalizer Localizer { get; set; } = default!;

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
    /// Gets or sets the type of resize to perform
    /// Discrete: resize by a 10 pixels at a time
    /// Exact: resize to the exact width specified (in pixels)
    /// The display of this component is dependent on a ResizeType being set
    /// </summary>
    [Parameter]
    public DataGridResizeType? ResizeType { get; set; } = DataGridResizeType.Discrete;

    /// <summary />
    protected override void OnParametersSet()
    {
        if (Column == 0)
        {
            throw new ArgumentException("Column must have a value greater than zero");
        }
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
        var valid = int.TryParse(_width, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result);
        if (valid)
        {
            await Grid.SetColumnWidthExactAsync(Column, result);
        }
    }

    private async Task HandleColumnWidthKeyDownAsync(KeyboardEventArgs args)
    {
        if (string.Equals(args.Key, "Enter", StringComparison.OrdinalIgnoreCase))
        {
            await HandleColumnWidthAsync();
        }
    }
}
