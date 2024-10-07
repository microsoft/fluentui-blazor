using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// An abstract base class for columns in a <see cref="FluentDataGrid{TGridItem}"/>.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public abstract partial class ColumnBase<TGridItem>
{
    private bool _isMenuOpen;
    private static readonly string[] KEYBOARD_MENU_SELECT_KEYS = ["Enter", "NumpadEnter"];
    private readonly string _columnId = $"column-header{Identifier.NewId()}";

    [CascadingParameter]
    internal InternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;

    /// <summary>
    /// Gets or sets the title text for the column.
    /// This is rendered automatically if <see cref="HeaderCellItemTemplate" /> is not used.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the an optional CSS class name.
    /// If specified, this is included in the class attribute of header and grid cells
    /// for this column.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// Gets or sets an optional CSS style specification.
    /// If specified, this is included in the style attribute of header and grid cells
    /// for this column.
    /// </summary>
    [Parameter]
    public string? Style { get; set; }

    /// <summary>
    /// If specified, controls the justification of header and grid cells for this column.
    /// </summary>
    [Parameter]
    public Align Align { get; set; }

    /// <summary>
    /// If true, generates a title and aria-label attribute for the cell contents
    /// </summary>
    [Parameter]
    public bool Tooltip { get; set; } = false;

    /// <summary>
    /// Gets or sets the function that defines the value to be used as the tooltip and aria-label in this column's cells
    /// </summary>
    [Parameter]
    public Func<TGridItem, string?>? TooltipText { get; set; }

    /// <summary>
    /// Gets or sets the tooltip text for the column header.
    /// </summary>
    [Parameter]
    public string? HeaderTooltip { get; set; }

    /// <summary>
    /// Gets or sets an optional template for this column's header cell.
    /// If not specified, the default header template includes the <see cref="Title" /> along with any applicable sort indicators and options buttons.
    /// </summary>
    [Parameter]
    public RenderFragment<ColumnBase<TGridItem>>? HeaderCellItemTemplate { get; set; }

    /// <summary>
    /// If specified, indicates that this column has this associated options UI. A button to display this
    /// UI will be included in the header cell by default.
    ///
    /// If <see cref="HeaderCellItemTemplate" /> is used, it is left up to that template to render any relevant
    /// "show options" UI and invoke the grid's <see cref="FluentDataGrid{TGridItem}.ShowColumnOptionsAsync(ColumnBase{TGridItem})" />).
    /// </summary>
    [Parameter]
    public RenderFragment? ColumnOptions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the data should be sortable by this column.
    ///
    /// The default value may vary according to the column type (for example, a <see cref="TemplateColumn{TGridItem}" />
    /// or <see cref="PropertyColumn{TGridItem, TProp}" /> is sortable by default if any<see cref="TemplateColumn{TGridItem}.SortBy" />
    /// or <see cref="PropertyColumn{TGridItem, TProp}.SortBy" /> parameter is specified).
    /// </summary>
    [Parameter]
    public bool? Sortable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the data is currently filtered by this column.
    ///
    /// The default value is false.
    /// </summary>
    [Parameter]
    public bool? Filtered { get; set; }

    /// <summary>
    /// Gets or sets the sorting rules for a column.
    /// </summary>
    public abstract GridSort<TGridItem>? SortBy { get; set; }

    /// <summary>
    /// Gets or sets the initial sort direction.
    /// if <see cref="IsDefaultSortColumn"/> is true.
    /// </summary>
    [Parameter]
    public SortDirection InitialSortDirection { get; set; } = default;

    /// <summary>
    /// Gets or sets a value indicating whether this column should be sorted by default.
    /// </summary>
    [Parameter]
    public bool IsDefaultSortColumn { get; set; } = false;

    /// <summary>
    /// If specified, virtualized grids will use this template to render cells whose data has not yet been loaded.
    /// </summary>
    [Parameter]
    public RenderFragment<PlaceholderContext>? PlaceholderTemplate { get; set; }

    /// <summary>
    /// Gets or sets the width of the column.
    /// Use either this or the <see cref="FluentDataGrid{TGridItem}"/> GridTemplateColumns parameter but not both.
    /// Needs to be a valid CSS width value like '100px', '10%' or '0.5fr'.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets a reference to the enclosing <see cref="FluentDataGrid{TGridItem}" />.
    /// </summary>
    protected FluentDataGrid<TGridItem> Grid => InternalGridContext.Grid;

    protected bool AnyColumnActionEnabled => Sortable is true || IsDefaultSortColumn || ColumnOptions != null || Grid.ResizableColumns;

    /// <summary>
    /// Event callback for when the row is clicked.
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    protected internal virtual Task OnRowClickAsync(FluentDataGridRow<TGridItem> row)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Event callback for when the key is pressed on a row.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    protected internal virtual Task OnRowKeyDownAsync(FluentDataGridRow<TGridItem> row, KeyboardEventArgs args)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Event callback for when the cell is clicked.
    /// </summary>
    /// <param name="cell"></param>
    /// <returns></returns>
    protected internal virtual Task OnCellClickAsync(FluentDataGridCell<TGridItem> cell)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Event callback for when the key is pressed on a cell.
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    protected internal virtual Task OnCellKeyDownAsync(FluentDataGridCell<TGridItem> cell, KeyboardEventArgs args)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Overridden by derived components to provide rendering logic for the column's cells.
    /// </summary>
    /// <param name="builder">The current <see cref="RenderTreeBuilder" />.</param>
    /// <param name="item">The data for the row being rendered.</param>
    protected internal abstract void CellContent(RenderTreeBuilder builder, TGridItem item);

    /// <summary>
    /// Overridden by derived components to provide the raw content for the column's cells.
    /// </summary>
    /// <param name="item">The data for the row being rendered.</param>
    protected internal virtual string? RawCellContent(TGridItem item) => null;

    /// <summary>
    /// Gets or sets a <see cref="RenderFragment" /> that will be rendered for this column's header cell.
    /// This allows derived components to change the header output. However, derived components are then
    /// responsible for using <see cref="HeaderCellItemTemplate" /> within that new output if they want to continue
    /// respecting that option.
    /// </summary>
    protected internal RenderFragment HeaderContent { get; protected set; }

    /// <summary>
    /// Gets a value indicating whether this column should act as sortable if no value was set for the
    /// <see cref="ColumnBase{TGridItem}.Sortable" /> parameter. The default behavior is not to be
    /// sortable unless <see cref="ColumnBase{TGridItem}.Sortable" /> is true.
    ///
    /// Derived components may override this to implement alternative default sortability rules.
    /// </summary>
    /// <returns>True if the column should be sortable by default, otherwise false.</returns>
    protected virtual bool IsSortableByDefault() => false;

    protected void HandleKeyDown(FluentKeyCodeEventArgs e)
    {
        if (e.CtrlKey && e.Key == KeyCode.Enter)
        {
            Grid.RemoveSortByColumnAsync(this);
        }
    }

    public bool ShowSortIcon;

    /// <summary>
    /// Constructs an instance of <see cref="ColumnBase{TGridItem}" />.
    /// </summary>
    public ColumnBase()
    {
        HeaderContent = RenderDefaultHeaderContent;
    }

    private async Task HandleColumnHeaderClickedAsync()
    {
        if ((Sortable is true || IsDefaultSortColumn) && (Grid.ResizableColumns || ColumnOptions is not null))
        {
            _isMenuOpen = !_isMenuOpen;
        }
        else if ((Sortable is true || IsDefaultSortColumn) && !Grid.ResizableColumns && ColumnOptions is null)
        {
            await Grid.SortByColumnAsync(this);
        }
        else if (Sortable is not true && !IsDefaultSortColumn && ColumnOptions is null && Grid.ResizableColumns)
        {
            await Grid.ShowColumnResizeAsync(this);
        }
    }

    private async Task HandleSortMenuKeyDownAsync(KeyboardEventArgs args)
    {
        if (KEYBOARD_MENU_SELECT_KEYS.Contains(args.Key))
        {
            await Grid.SortByColumnAsync(this);
            StateHasChanged();                          
            _isMenuOpen = false;
        }
    }

    private async Task HandleResizeMenuKeyDownAsync(KeyboardEventArgs args)
    {
        if (KEYBOARD_MENU_SELECT_KEYS.Contains(args.Key))
        {
            await Grid.ShowColumnResizeAsync(this);
            _isMenuOpen = false;
        }
    }

    private async Task HandleOptionsMenuKeyDownAsync(KeyboardEventArgs args)
    {
        if (KEYBOARD_MENU_SELECT_KEYS.Contains(args.Key))
        {
            await Grid.ShowColumnOptionsAsync(this);
            _isMenuOpen = false;
        }
    }

    private string GetSortOptionText()
    {
        if (Grid.SortByAscending.HasValue && ShowSortIcon)
        {
            if (Grid.SortByAscending is true)
            {
                return Grid.ColumnSortLabels.SortMenuAscendingLabel;
            }
            else
            {
                return Grid.ColumnSortLabels.SortMenuDescendingLabel;
            }
        }

        return Grid.ColumnSortLabels.SortMenu;
    }
}
