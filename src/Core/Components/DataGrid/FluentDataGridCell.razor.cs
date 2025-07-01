// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a cell in a <see cref="FluentDataGrid{TGridItem}"/>.
/// </summary>
public partial class FluentDataGridCell<TGridItem> : FluentComponentBase
{
    /// <summary>
    /// Gets a reference to the column that this cell belongs to.
    /// </summary>
    private ColumnBase<TGridItem>? Column => Grid._columns.ElementAtOrDefault(GridColumn - 1);

    internal string CellId { get; set; } = string.Empty;

    /// <summary />
    public FluentDataGridCell(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary>
    /// Gets or sets the reference to the item that holds this cell's values.
    /// </summary>
    [Parameter]
    public TGridItem? Item { get; set; }

    /// <summary>
    /// Gets or sets the cell type. See <see cref="DataGridCellType"/>.
    /// </summary>
    [Parameter]
    public DataGridCellType CellType { get; set; }

    /// <summary>
    /// Gets or sets the column index of the cell.
    /// This will be applied to the css grid-column-index value applied to the cell.
    /// </summary>
    [Parameter]
    public int GridColumn { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the owning <see cref="FluentDataGridRow{TItem}"/> component.
    /// </summary>
    [CascadingParameter(Name = "OwningRow")]
    internal FluentDataGridRow<TGridItem> Owner { get; set; } = default!;

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
        .AddClass("column-header", when: CellType == DataGridCellType.ColumnHeader)
        .AddClass("select-all", when: CellType == DataGridCellType.ColumnHeader && Column is SelectColumn<TGridItem>)
        .AddClass("multiline-text", when: Grid.MultiLine && (Grid.Items is not null || Grid.ItemsProvider is not null) && CellType != DataGridCellType.ColumnHeader)
        .AddClass(Owner.Class)
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("grid-column", GridColumn.ToString(CultureInfo.InvariantCulture), () => !Grid.EffectiveLoadingValue && (Grid.Items is not null || Grid.ItemsProvider is not null) && Grid.DisplayMode == DataGridDisplayMode.Grid)
        .AddStyle("text-align", "center", Column is SelectColumn<TGridItem>)
        .AddStyle("align-content", "center", Column is SelectColumn<TGridItem>)
        //.AddStyle("padding-inline-start", "calc(((var(--design-unit)* 3) + var(--focus-stroke-width) - var(--stroke-width))* 1px)", Column is SelectColumn<TGridItem> && Owner.RowType == DataGridRowType.Default)
        .AddStyle("padding-top", "10px", Column is SelectColumn<TGridItem> && (Grid.RowSize == DataGridRowSize.Medium || Owner.RowType == DataGridRowType.Header))
        .AddStyle("padding-top", "6px", Column is SelectColumn<TGridItem> && Grid.RowSize == DataGridRowSize.Small && Owner.RowType == DataGridRowType.Default)
        .AddStyle("width", Column?.Width, !string.IsNullOrEmpty(Column?.Width) && Grid.DisplayMode == DataGridDisplayMode.Table)
        .AddStyle("height", $"{Grid.ItemSize.ToString(CultureInfo.InvariantCulture):0}px", () => !Grid.EffectiveLoadingValue && Grid.Virtualize)
        .AddStyle("height", $"{((int)Grid.RowSize).ToString(CultureInfo.InvariantCulture)}px", () => !Grid.EffectiveLoadingValue && !Grid.Virtualize && !Grid.MultiLine && (Grid.Items is not null || Grid.ItemsProvider is not null))
        .AddStyle("height", "100%", Grid.MultiLine)
        .AddStyle("min-height", "44px", Owner.RowType != DataGridRowType.Default)
        .AddStyle("z-index", ZIndex.DataGridHeaderPopup.ToString(CultureInfo.InvariantCulture), CellType == DataGridCellType.ColumnHeader && Grid._columns.Count > 0 && Grid.UseMenuService)
        .AddStyle(Owner.Style)
        .Build();

    /// <inheritdoc />
    public void Dispose() => Owner.Unregister(this);

    /// <summary />
    internal async Task HandleOnCellClickAsync()
    {
        if (Grid.OnCellClick.HasDelegate)
        {
            await Grid.OnCellClick.InvokeAsync(this);
        }

        if (Column != null)
        {
            await Column.OnCellClickAsync(this);
        }
    }

    internal async Task HandleOnCellFocusAsync()
    {
        if (CellType == DataGridCellType.Default)
        {
            await Grid.OnCellFocus.InvokeAsync(this);
        }
    }

    internal async Task HandleOnCellKeyDownAsync(KeyboardEventArgs e)
    {
        if (!SelectColumn<TGridItem>.KEYBOARD_SELECT_KEYS.Contains(e.Code, StringComparer.OrdinalIgnoreCase))
        {
            return;
        }

        if (Column != null)
        {
            await Column.OnCellKeyDownAsync(this, e);
        }
    }

    /// <summary />
    protected override void OnInitialized()
    {
        Owner.Register(this);
    }
}
