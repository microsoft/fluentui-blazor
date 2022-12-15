// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Represents a <see cref="FluentDataGrid{TGridItem}"/> column whose cells display a single value.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of the value being displayed in the column's cells.</typeparam>
public class PropertyColumn<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TGridItem, TValue> : ColumnBase<TGridItem>, IBindColumn<TGridItem, TValue>, ISortableColumn<TGridItem, TValue>, IFilterableColumn<TGridItem, TValue> where TGridItem : class
{

    public PropertyColumn()
    {
        this.Sortable = true;
        this.Filterable = true;
    }

    private Expression<Func<TGridItem, TValue>>? _lastAssignedProperty;
    protected Func<TGridItem, TValue>? _compiledProperty;
    protected Func<TGridItem, string?>? _cellTextFunc;

    private TGridItem? _currentItem;

    /// <summary>
    /// Defines the value to be displayed in this column's cells.
    /// </summary>
    [Parameter, EditorRequired] public Expression<Func<TGridItem, TValue>> Property { get; set; } = default!;

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
            if (Title is null)
            {
                PropertyInfo = typeof(TGridItem).GetProperty(memberExpression.Member.Name);
                if (PropertyInfo is null || PropertyInfo.DeclaringType is null)
                    return;
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

    protected internal override void CellEditContent(RenderTreeBuilder builder)
         => CellContent(builder, _currentItem!);

    #region Implement ISortableColumn

    private Expression<Func<TGridItem, TValue>>? _sortProperty;
    [Parameter]
    public Expression<Func<TGridItem, TValue>>? SortProperty
    {
        get
        {
            if (_sortProperty is null && Property is not null)
                return Property;
            return _sortProperty;
        }
        set => _sortProperty = value;
    }

    public Nullable<ListSortDirection> SortDirection { get; set; }

    public short SortOrder { get; set; }

    IQueryable<TGridItem> ISortableColumn<TGridItem>.ApplySort(IQueryable<TGridItem> Source, bool IsFirst)
    {
        return _sortProvider!.ApplySort(this, Source, IsFirst);
    }

    #endregion

    #region Implement IFilterableColumn

    public PropertyInfo? PropertyInfo { get; private set; }

    private Expression<Func<TGridItem, TValue>>? _filterProperty;
    [Parameter]
    public Expression<Func<TGridItem, TValue>>? FilterProperty
    {
        get
        {
            if (_filterProperty is null && Property is not null)
                return Property;
            return _filterProperty;
        }
        set { _filterProperty = value; }
    }

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

    public override void BeginEdit(TGridItem item)
    {
        _currentItem = item;
    }

}
