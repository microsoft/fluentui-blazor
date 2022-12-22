// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// An abstract base class for columns in a <see cref="FluentDataGrid{TGridItem}"/>.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
public abstract partial class ColumnBase<TGridItem>
{

    internal bool DefaultSortApplied;

    /// <summary>
    /// Get a value indicating whether this column should act as sortable if no value was set for the
    /// <see cref="ColumnBase{TGridItem}.Sortable" /> parameter. The default behavior is not to be
    /// sortable unless <see cref="ColumnBase{Titem}.Sortable" /> is true.
    ///
    /// Derived components may override this to implement alternative default sortability rules.
    /// </summary>
    /// <returns>True if the column should be sortable by default, otherwise false.</returns>
    private bool? _isSortableByDefault;

    internal SortProvider? _sortProvider;

    /// <summary>
    /// column options component rendered dynamically. this property contains an instance of dynamic component generator. 
    /// then we can access generated component with <see cref="DynamicComponent.Instance"/>
    /// </summary>
    protected DynamicComponent? headerOptionsComponentHolder { get; set; }

    [CascadingParameter] internal InternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;

    /// <summary>
    /// Title text for the column. This is rendered automatically if <see cref="HeaderCellItemTemplate" /> is not used.
    /// </summary>
    [Parameter] public string? Title { get; set; }

    /// <summary>
    /// An optional CSS class name. If specified, this is included in the class attribute of table header and body cells
    /// for this column.
    /// </summary>
    [Parameter] public string? Class { get; set; }

    /// <summary>
    /// If specified, controls the justification of table header and body cells for this column.
    /// </summary>
    [Parameter] public Align Align { get; set; }

    /// <summary>
    /// An optional template for this column's header cell. If not specified, the default header template
    /// includes the <see cref="Title" /> along with any applicable sort indicators and options buttons.
    /// </summary>
    [Parameter] public RenderFragment<ColumnBase<TGridItem>>? HeaderCellItemTemplate { get; set; }

    /// <summary>
    /// If specified, indicates that this column has this associated options UI. A button to display this
    /// UI will be included in the header cell by default.
    ///
    /// If <see cref="HeaderCellItemTemplate" /> is used, it is left up to that template to render any relevant
    /// "show options" UI and invoke the grid's <see cref="FluentDataGrid{TGridItem}.ShowColumnOptions(ColumnBase{TGridItem})" />).
    /// </summary>
    [Parameter] public RenderFragment? ColumnOptions { get; set; }

    /// <summary>
    /// Indicates whether the data should be sortable by this column.
    ///
    /// The default value may vary according to the column type (for example, a <see cref="TemplateColumn{TGridItem,TValue}" />
    /// is sortable by default if any <see cref="TemplateColumn{TGridItem,TValue}.SortProperty" /> parameter is specified).
    /// </summary>
    [Parameter] public bool? Sortable { get; set; }

    /// <summary>
    /// If specified and not null, indicates that this column represents the initial sort order
    /// for the grid. The supplied value controls the default sort direction.
    /// </summary>
    [Parameter]
    public ListSortDirection? DefaultSort { get; set; }

    /// <summary>
    /// If specified, virtualized grids will use this template to render cells whose data has not yet been loaded.
    /// </summary>
    [Parameter] public RenderFragment<PlaceholderContext>? PlaceholderTemplate { get; set; }

    /// <summary>
    /// Gets a reference to the enclosing <see cref="FluentDataGrid{TGridItem}" />.
    /// </summary>
    public FluentDataGrid<TGridItem> Grid => InternalGridContext.Grid;

    /// <summary>
    /// Overridden by derived components to provide rendering logic for the column's cells.
    /// </summary>
    /// <param name="builder">The current <see cref="RenderTreeBuilder" />.</param>
    /// <param name="item">The data for the row being rendered.</param>
    protected internal abstract void CellContent(RenderTreeBuilder builder, TGridItem item);

    /// <summary>
    /// Gets or sets a <see cref="RenderFragment" /> that will be rendered for this column's header cell.
    /// This allows derived components to change the header output. However, derived components are then
    /// responsible for using <see cref="HeaderCellItemTemplate" /> within that new output if they want to continue
    /// respecting that option.
    /// </summary>
    protected internal RenderFragment HeaderContent { get; protected set; }

    /// <summary>
    /// Get a value indicating whether this column should act as sortable if no value was set for the
    /// <see cref="ColumnBase{TGridItem}.Sortable" /> parameter. The default behavior is not to be
    /// sortable unless <see cref="ColumnBase{TGridItem}.Sortable" /> is true.
    ///
    /// Derived components may override this to implement alternative default sortability rules.
    /// </summary>
    /// <returns>True if the column should be sortable by default, otherwise false.</returns>
    internal virtual bool IsSortableByDefault() {
        if (!_isSortableByDefault.HasValue)
            _isSortableByDefault = typeof(ISortableColumn<TGridItem>).IsAssignableFrom(this.GetType());
        return _isSortableByDefault.Value;
    }

    /// <summary>
    /// Constructs an instance of <see cref="ColumnBase{TGridItem}" />.
    /// </summary>
    public ColumnBase()
    {
        HeaderContent = RenderDefaultHeaderContent;
        if (IsSortableByDefault())
            _sortProvider = new SortProvider();
    }

    /// <summary>
    /// when sort direction of any column changed,datagrid will call this function to apply new sort
    /// </summary>
    protected virtual void SortChanged()
    {
        if (typeof(ISortableColumn<TGridItem>).IsAssignableFrom(this.GetType()))
        {
            var thisSort = this as ISortableColumn<TGridItem>;
            if (thisSort is not null)
            {
                if (!thisSort.SortDirection.HasValue)
                    thisSort.SortDirection = ListSortDirection.Descending;
                thisSort.SortDirection = thisSort.SortDirection.Value switch
                {
                    ListSortDirection.Ascending => ListSortDirection.Descending,
                    ListSortDirection.Descending => ListSortDirection.Ascending,
                    _ => ListSortDirection.Ascending
                };
                Grid.ApplySort(thisSort);
            }
        }
    }

    /// <summary>
    /// Close column options popup
    /// </summary>
    public virtual void CloseFilter()
    {
        if (headerOptionsComponentHolder is not null && headerOptionsComponentHolder.Instance is not null)
            ((ColumnHeaderOptionsBase<TGridItem>)headerOptionsComponentHolder.Instance)?.CloseDropDown();
    }

}
