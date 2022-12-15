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
/// Represents a <see cref="FluentDataGrid{TGridItem}"/> column whose cells render a supplied template.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// /// <typeparam name="TValue">The type of data represented by this column.</typeparam>
public class TemplateColumn<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]  TGridItem, TValue> : ColumnBase<TGridItem>, ISortableColumn<TGridItem, TValue>, IFilterableColumn<TGridItem, TValue> where TGridItem : class
{

    private TGridItem? _currentItem;

    private readonly static RenderFragment<TGridItem> EmptyChildContent = _ => builder => { };

    protected override void OnParametersSet()
    {
        if (FilterProperty is not null && FilterProperty.Body is MemberExpression memberExpression)
        {
            PropertyInfo = typeof(TGridItem).GetProperty(memberExpression.Member.Name)!;
        }
    }

    /// <summary>
    /// Specifies the content to be rendered for each row in the table.
    /// </summary>
    [Parameter] public RenderFragment<TGridItem> ChildContent { get; set; } = EmptyChildContent;

    /// <inheritdoc />
    protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
        => builder.AddContent(0, ChildContent(item));

    /// <inheritdoc />
    protected internal override void CellEditContent(RenderTreeBuilder builder)
      => CellContent(builder, _currentItem!);

    /// <inheritdoc />
    internal override bool IsSortableByDefault()
        => SortProperty is not null;

    /// <inheritdoc />
    internal override bool IsFilterableByDefault()
        => FilterProperty is not null;


    #region Implement ISortableColumn

    [Parameter]
    public Expression<Func<TGridItem, TValue>>? SortProperty { get; set; }

    public Nullable<ListSortDirection> SortDirection { get; set; }

    public short SortOrder { get; set; }

    IQueryable<TGridItem> ISortableColumn<TGridItem>.ApplySort(IQueryable<TGridItem> Source, bool IsFirst)
    {
        return _sortProvider!.ApplySort(this, Source, IsFirst);
    }

    #endregion

    #region Implement IFilterableColumn

    public PropertyInfo? PropertyInfo { get; private set; }

    [Parameter]
    public Expression<Func<TGridItem, TValue>>? FilterProperty { get; set; }

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
