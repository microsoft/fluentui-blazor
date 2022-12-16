using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.Fast.Components.FluentUI;

public class ExtendPropertyColumn<TGridItem, TValue, TSort, TFilter>
    : ColumnBase<TGridItem>, IBindColumn<TGridItem, TValue>, ISortableColumn<TGridItem, TSort>, IFilterableColumn<TGridItem, TFilter>
    where TGridItem : class
{

    public ExtendPropertyColumn()
    {
        this.Sortable = true;
        this.Filterable = true;
    }

    private Expression<Func<TGridItem, TValue>>? _lastAssignedProperty;
    private Func<TGridItem, TValue>? _compiledProperty;
    private Func<TGridItem, string?>? _cellTextFunc;

    private TGridItem? _currenTGridItem;

    /// <summary>
    /// Defines the value to be displayed in this column's cells.
    /// </summary>
    [Parameter, EditorRequired]
    public Expression<Func<TGridItem, TValue>> Property { get; set; } = default!;

    /// <summary>
    /// Optionally specifies a format string for the value.
    ///
    /// Using this requires the <typeparamref name="TValue"/> type to implement <see cref="IFormattable" />.
    /// </summary>
    [Parameter] public string? Format { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        // We have to do a bit of pre-processing on the lambda expression. Only do that if it's new or changed.
        if (_lastAssignedProperty != Property)
        {
            _lastAssignedProperty = Property;
            _compiledProperty = Property.Compile();

            if (!string.IsNullOrEmpty(Format))
            {
                if (typeof(IFormattable).IsAssignableFrom(typeof(TValue)))
                {
                    _cellTextFunc = item => ((IFormattable?)_compiledProperty!(item))?.ToString(Format, null);

                }
                else
                {
                    throw new InvalidOperationException($"A '{nameof(Format)}' parameter was supplied, but the type '{typeof(TValue)}' does not implement '{typeof(IFormattable)}'.");
                }
            }
            else
            {
                _cellTextFunc = item => _compiledProperty!(item)?.ToString();
            }
        }

        if (Property.Body is MemberExpression memberExpression)
        {
            PropertyInfo = typeof(TGridItem).GetProperty(memberExpression.Member.Name);
            if (PropertyInfo is null || PropertyInfo.DeclaringType is null)
                return;
            if (Title is null)
            {
                var daText = PropertyInfo.DeclaringType.GetDisplayAttributeString(memberExpression.Member.Name);
                if (!string.IsNullOrEmpty(daText))
                    Title = daText;
                else
                    Title = memberExpression.Member.Name;
            }
        }
    }

    /// <inheritdoc />
    protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
        => builder.AddContent(10, _cellTextFunc!(item));

    /// <inheritdoc />
    protected internal override void CellEditContent(RenderTreeBuilder builder)
        => CellContent(builder, _currenTGridItem!);


    #region Implement ISortableColumn

    [Parameter]
    public Expression<Func<TGridItem, TSort>>? SortProperty { get; set; }

    public Nullable<ListSortDirection> SortDirection { get; set; }

    public PropertyInfo? PropertyInfo { get; private set; }

    public short SortOrder { get; set; }

    IQueryable<TGridItem> ISortableColumn<TGridItem>.ApplySort(IQueryable<TGridItem> Source, bool IsFirst)
    {
        return _sortProvider!.ApplySort(this, Source,IsFirst);
    }


    #endregion

    #region Implement IFilterableColumn

    [Parameter]
    public Expression<Func<TGridItem, TFilter>>? FilterProperty { get; set; }

    IQueryable<TGridItem> IFilterableColumn<TGridItem>.ApplyFilter(IQueryable<TGridItem> Source)
    {
        if (FilterProperty is not null)
        {
            if (filterComponentHolder is not null && filterComponentHolder.Instance is not null)
            {
                Source = ((DataGridFilterColumnHeaderBase<TGridItem>)filterComponentHolder.Instance).ApplyFilter(Source);
            }
            return Source;
        }
        return Source;
    }


    #endregion

    public override void SetFocuse()
    {

    }

    public override object? GetCurrentValue()
    {
        return null;
    }

    public override void UpdateSource()
    {

    }

    public override void BeginEdit(TGridItem Item)
    {
        _currenTGridItem = Item;
    }
}
