// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a row in a <see cref="FluentDataGrid{TGridItem}"/> component, providing functionality for rendering,
/// interaction, and data binding.
/// </summary>
/// <remarks>This class is used internally by the <see cref="FluentDataGrid{TGridItem}"/> to manage rows and their
/// associated data. It provides support for row-specific behaviors such as focus, click, and keyboard interactions, as
/// well as managing the layout and content of the row.</remarks>
/// <typeparam name="TGridItem">The type of the data item associated with the row.</typeparam>
[CascadingTypeParameter(nameof(TGridItem))]
public partial class FluentDataGridRow<TGridItem> : FluentComponentBase, IHandleEvent
{
    private readonly Dictionary<string, FluentDataGridCell<TGridItem>> cells = [];
    internal string RowId { get; set; } = string.Empty;

    /// <summary />
    public FluentDataGridRow(LibraryConfiguration configuration) : base(configuration)
    {
    }

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
    public DataGridRowType RowType { get; set; } = DataGridRowType.Default;

    /// <summary>
    /// Gets or sets the vertical alignment of a row
    /// </summary>
    [Parameter]
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the owning <see cref="FluentDataGrid{TItem}"/> component
    /// </summary>
    [CascadingParameter]
    internal InternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;

    /// <summary>
    /// Gets a reference to the enclosing <see cref="FluentDataGrid{TGridItem}" />.
    /// </summary>
    protected FluentDataGrid<TGridItem> Grid => InternalGridContext.Grid;

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-data-grid-row")
        .AddClass("hover", when: Grid.ShowHover)
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
       .Build();

    /// <summary>
    /// Sets the RowIndex for this row.
    /// </summary>
    public void SetRowIndex(int rowIndex)
    {
        RowIndex = rowIndex;
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        InternalGridContext.Unregister(this);
        return base.DisposeAsync();
    }

    /// <summary />
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "Tested in aspnetcore code")]
    Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem callback, object? arg)
        => callback.InvokeAsync(arg);

    /// <summary />
    protected override void OnInitialized()
    {
        RowId = $"r{InternalGridContext.GetNextRowId().ToString(CultureInfo.InvariantCulture)}";
        InternalGridContext.Register(this);
    }

    internal void Register(FluentDataGridCell<TGridItem> cell)
    {
        cell.CellId = $"c{InternalGridContext.GetNextCellId().ToString(CultureInfo.InvariantCulture)}";
        cells.Add(cell.CellId, cell);
    }

    internal void Unregister(FluentDataGridCell<TGridItem> cell)
    {
        cells.Remove(cell.CellId!);
    }

    internal async Task HandleOnRowFocusAsync()
    {
        if (Grid.OnRowFocus.HasDelegate)
        {
            await Grid.OnRowFocus.InvokeAsync(this);
        }
    }

    /// <summary />
    internal async Task HandleOnRowClickAsync(string rowId)
    {
        var row = GetRow(rowId);

        if (row is null)
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(row.Class) &&
            (row.Class.Contains(FluentDataGrid<TGridItem>.EMPTY_CONTENT_ROW_CLASS, StringComparison.Ordinal) ||
             row.Class.Contains(FluentDataGrid<TGridItem>.LOADING_CONTENT_ROW_CLASS, StringComparison.Ordinal)))
        {
            return;
        }

        if (Grid.OnRowClick.HasDelegate)
        {
            await Grid.OnRowClick.InvokeAsync(row);
        }

        if (row.RowType == DataGridRowType.Default)
        {
            foreach (var column in Grid._columns)
            {
                await column.OnRowClickAsync(row);
            }
        }
    }

    /// <summary />
    internal async Task HandleOnRowDoubleClickAsync(string rowId)
    {
        var row = GetRow(rowId);
        if (row is null)
        {
            return;
        }

        if (Grid.OnRowDoubleClick.HasDelegate)
        {
            await Grid.OnRowDoubleClick.InvokeAsync(row);
        }
    }

    /// <summary />
    internal async Task HandleOnRowKeyDownAsync(string rowId, KeyboardEventArgs e)
    {
        var row = GetRow(rowId);

        if (row is null)
        {
            return;
        }

        if (!SelectColumn<TGridItem>.KEYBOARD_SELECT_KEYS.Contains(e.Code, StringComparer.Ordinal))
        {
            return;
        }

        if (Grid.OnRowClick.HasDelegate)
        {
            await Grid.OnRowClick.InvokeAsync(row);
        }

        if (row.RowType == DataGridRowType.Default)
        {
            foreach (var column in Grid._columns)
            {
                await column.OnRowKeyDownAsync(row, e);
            }
        }
    }

    private FluentDataGridRow<TGridItem>? GetRow(string rowId)
    {
        if (!string.IsNullOrEmpty(rowId) && InternalGridContext.Rows.TryGetValue(rowId, out var row))
        {
            return row;
        }

        return null;
    }
}
