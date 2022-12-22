// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Fast.Components.FluentUI.DataGrid.Infrastructure;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Represents a <see cref="FluentDataGrid{TGridItem}"/> column whose cells render a supplied template.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TValue">The type of property for sorting.</typeparam>
public class TemplateColumn<TGridItem, TValue> : ColumnBase<TGridItem>, ISortableColumn<TGridItem,TValue>
{
    private readonly static RenderFragment<TGridItem> EmptyChildContent = _ => builder => { };

    /// <inheritdoc />
    [Parameter]
    public Expression<Func<TGridItem, TValue>>? SortProperty { get; set; }

    /// <inheritdoc />
    public Nullable<ListSortDirection> SortDirection { get; set; }

    /// <inheritdoc />
    public short SortOrder { get; set; }

    protected override void OnParametersSet()
    {
        if (_sortProvider is null && IsSortableByDefault())
            _sortProvider = new SortProvider();
        base.OnParametersSet();
    }

    /// <summary>
    /// Specifies the content to be rendered for each row in the table.
    /// </summary>
    [Parameter] public RenderFragment<TGridItem> ChildContent { get; set; } = EmptyChildContent;

       /// <inheritdoc />
    protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
        => builder.AddContent(0, ChildContent(item));

    /// <inheritdoc />
    internal override bool IsSortableByDefault()
        => SortProperty is not null && base.IsSortableByDefault();

    IQueryable<TGridItem> ISortableColumn<TGridItem>.ApplySort(IQueryable<TGridItem> Source, bool IsFirst)
    {
        return _sortProvider!.ApplySort(this, Source, IsFirst);
    }
}
