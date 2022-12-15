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
public abstract partial class ColumnBase<TGridItem> where TGridItem : class
{

    #region Fields

    /// <summary>
    /// Get a value indicating whether this column should act as sortable if no value was set for the
    /// <see cref="ColumnBase{TGridItem}.Sortable" /> parameter. The default behavior is not to be
    /// sortable unless <see cref="ColumnBase{Titem}.Sortable" /> is true.
    ///
    /// Derived components may override this to implement alternative default sortability rules.
    /// </summary>
    /// <returns>True if the column should be sortable by default, otherwise false.</returns>
    private Nullable<bool> _isSortableByDefault;

    /// <summary>
    /// Get a value indicating whether this column should act as filterable if no value was set for the
    /// <see cref="ColumnBase{TGridItem}.Filterable" /> parameter. The default behavior is not to be
    /// filterable unless <see cref="ColumnBase{TGridItem}.Filterable" /> is true.
    ///
    /// Derived components may override this to implement alternative default sortability rules.
    /// </summary>
    /// <returns>True if the column should be sortable by default, otherwise false.</returns>
    private Nullable<bool> _isFilterableByDefault;


    internal DataGridSortProvider? _sortProvider;

    internal bool InternalIsReadonly;

    /// <summary>
    /// Gets a reference to the enclosing <see cref="FluentDataGrid{TGridItem}" />.
    /// </summary>
    public FluentDataGrid<TGridItem> Grid => InternalGridContext.Grid;


    /// <summary>
    /// Gets or sets a <see cref="RenderFragment" /> that will be rendered for this column's header cell.
    /// This allows derived components to change the header output. However, derived components are then
    /// responsible for using <see cref="HeaderCellItemTemplate" /> within that new output if they want to continue
    /// respecting that option.
    /// </summary>
    protected internal RenderFragment HeaderContent { get; protected set; }


    #endregion

    #region Properties


    [CascadingParameter]
    internal InternalGridContext<TGridItem> InternalGridContext { get; set; } = default!;


    protected DynamicComponent? filterComponentHolder { get; set; }

    /// <summary>
    /// Title text for the column. This is rendered automatically if <see cref="HeaderCellItemTemplate" /> is not used.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// An optional CSS class name. If specified, this is included in the class attribute of table header and body cells
    /// for this column.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }

    /// <summary>
    /// If specified, controls the justification of table header and body cells for this column.
    /// </summary>
    [Parameter]
    public Align Align { get; set; }

    /// <summary>
    /// An optional template for this column's header cell. If not specified, the default header template
    /// includes the <see cref="Title" /> along with any applicable sort indicators and options buttons.
    /// </summary>
    [Parameter]
    public RenderFragment<ColumnBase<TGridItem>>? HeaderCellItemTemplate { get; set; }

    /// <summary>
    /// If specified, indicates that this column has this associated options UI. A button to display this
    /// UI will be included in the header cell by default.
    ///
    /// If <see cref="HeaderCellItemTemplate" /> is used, it is left up to that template to render any relevant
    /// "show options" UI and invoke the grid's <see cref="FluentDataGrid{TGridItem}.ShowColumnOptions(ColumnBase{TGridItem})" />).
    /// </summary>
    [Parameter]
    public RenderFragment? ColumnOptions { get; set; }

    /// <summary>
    /// Indicates whether the data should be sortable by this column.
    ///
    /// The default value may vary according to the column type (for example, a <see cref="ISortableColumn{TGridItem,TValue}" />
    /// is sortable by default if any <see cref="TemplateColumn{TGridItem,TValue}.SortProperty" /> parameter is specified).
    /// </summary>
    [Parameter]
    public bool? Sortable { get; set; }

    /// <summary>
    /// Indicates whether the data should be filterable by this column.
    ///
    /// The default value may vary according to the column type (implement <see cref="IFilterableColumn{TItem, TValue}" /> filterable column
    /// </summary>
    [Parameter]
    public bool? Filterable { get; set; }

    /// <summary>
    /// Indicates whether the data should be readonly by this column.
    ///
    /// The default value may vary according to the column type (Editable columns must implement <see cref="IBindColumn{Titem,TValue}" />
    /// </summary>
    [Parameter]
    public bool IsReadonly { get; set; }

    internal bool IsEditable { get; set; }

    /// <summary>
    /// If specified and not null, indicates that this column represents the initial sort order
    /// for the grid. The supplied value controls the default sort direction.
    /// </summary>
    [Parameter]
    public ListSortDirection? DefaultSort { get; set; }


    /// <summary>
    /// If specified, virtualized grids will use this template to render cells whose data has not yet been loaded.
    /// </summary>
    [Parameter]
    public RenderFragment<PlaceholderContext>? PlaceholderTemplate { get; set; }

    #endregion

    #region Initialization

    /// <summary>
    /// Constructs an instance of <see cref="ColumnBase{TGridItem}" />.
    /// </summary>
    public ColumnBase()
    {
        HeaderContent = RenderDefaultHeaderContent;
        if (IsSortableByDefault())
            _sortProvider = new DataGridSortProvider();
    }

    #endregion

    #region Functions

    /// <summary>
    /// Overridden by derived components to provide rendering logic for the column's cells.
    /// </summary>
    /// <param name="builder">The current <see cref="RenderTreeBuilder" />.</param>
    /// <param name="item">The data for the row being rendered.</param>
    protected internal abstract void CellContent(RenderTreeBuilder builder, TGridItem item);

    /// <summary>
    /// Overridden by derived components to provide rendering logic for the editing column's cells, if column is readonly
    /// </summary>
    /// <param name="builder">The current <see cref="RenderTreeBuilder" />.</param>
    protected internal abstract void CellEditContent(RenderTreeBuilder builder);

    /// <summary>
    /// Get a value indicating whether this column should act as sortable if no value was set for the
    /// <see cref="ColumnBase{TGridItem}.Sortable" /> parameter. The default behavior is not to be
    /// sortable unless <see cref="ColumnBase{TGridItem}.Sortable" /> is true.
    ///
    /// </summary>
    /// <returns>True if the column should be sortable by default, otherwise false.</returns>
    internal virtual bool IsSortableByDefault()
    {
        if (!_isSortableByDefault.HasValue)
            _isSortableByDefault = typeof(ISortableColumn<TGridItem>).IsAssignableFrom(this.GetType());
        return _isSortableByDefault.Value;
    }

    /// <summary>
    /// Get a value indicating whether this column is filterable if no value was set for
    /// <see cref="ColumnBase{TGridItem}.Filterable" /> parameter. The default behavior is not to be
    /// sortable unless <see cref="ColumnBase{TGridItem}.Filterable" /> is true.
    ///
    /// </summary>
    /// <returns>True if the column should be sortable by default, otherwise false.</returns>
    internal virtual bool IsFilterableByDefault()
    {
        if (!_isFilterableByDefault.HasValue)
        {
            _isFilterableByDefault = typeof(IFilterableColumn<TGridItem>).IsAssignableFrom(this.GetType());
        }

        return _isFilterableByDefault.Value;
    }

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
        if (filterComponentHolder is not null && filterComponentHolder.Instance is not null)
            ((DataGridFilterColumnHeaderBase<TGridItem>)filterComponentHolder.Instance)?.CloseDropDown();
    }

    /// <summary>
    /// set focuse to input element, on edit mode
    /// </summary>
    public abstract void SetFocuse();

    /// <summary>
    /// get user inputed value of column, before commit edit and update source
    /// </summary>
    public abstract object? GetCurrentValue();

    /// <summary>
    /// prepare column for edit. for example get initiale value of column or ...
    /// this function called on begin edit
    /// </summary>
    /// <param name="Item"></param>
    public abstract void BeginEdit(TGridItem Item);

    /// <summary>
    /// update source value from input value. this funcation called on commit edit
    /// </summary>
    public abstract void UpdateSource();

    #endregion
}
