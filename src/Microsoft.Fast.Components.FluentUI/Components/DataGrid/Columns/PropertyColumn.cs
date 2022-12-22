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
public class PropertyColumn<TGridItem, TValue> : ColumnBase<TGridItem>, IBindableColumn<TGridItem, TValue>, ISortableColumn<TGridItem,TValue>
{
    private Expression<Func<TGridItem, TValue>>? _lastAssignedProperty;
    private Func<TGridItem, string?>? _cellTextFunc;

    public PropertyColumn()
    {
        this.Sortable = true;
    }

    /// <inheritdoc />
    public PropertyInfo? PropertyInfo { get; private set; }

    /// <inheritdoc />
    [Parameter, EditorRequired] public Expression<Func<TGridItem, TValue>> Property { get; set; } = default!;

    /// <summary>
    /// Optionally specifies a format string for the value.
    ///
    /// Using this requires the <typeparamref name="TValue"/> type to implement <see cref="IFormattable" />.
    /// </summary>
    [Parameter] public string? Format { get; set; }


    private Expression<Func<TGridItem, TValue>>? _sortProperty;
    /// <inheritdoc />
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

    /// <inheritdoc />
    public ListSortDirection? SortDirection { get; set; }

    /// <inheritdoc />
    public short SortOrder { get; set; }

    /// <inheritdoc />
    [SuppressMessage("Trimming", "IL2072:Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.", Justification = "'DynamicallyAccessedMembersAttribute' cannot be added to overriden method")]

    protected override void OnParametersSet()
    {
        // We have to do a bit of pre-processing on the lambda expression. Only do that if it's new or changed.
        if (_lastAssignedProperty != Property)
        {
            _lastAssignedProperty = Property;
            var compiledPropertyExpression = Property.Compile();

            if (!string.IsNullOrEmpty(Format))
            {
                if (typeof(IFormattable).IsAssignableFrom(typeof(TValue)))
                {
                    _cellTextFunc = item => ((IFormattable?)compiledPropertyExpression!(item))?.ToString(Format, null);

                }
                else
                {
                    throw new InvalidOperationException($"A '{nameof(Format)}' parameter was supplied, but the type '{typeof(TValue)}' does not implement '{typeof(IFormattable)}'.");
                }
            }
            else
            {
                _cellTextFunc = item => compiledPropertyExpression!(item)?.ToString();
            }
        }

        if (Property.Body is MemberExpression memberExpression)
        {
            if (Title is null)
            {
                PropertyInfo = memberExpression.Member as PropertyInfo;
                var daText = memberExpression.Member.DeclaringType?.GetDisplayAttributeString(memberExpression.Member.Name);
                if (!string.IsNullOrEmpty(daText))
                    Title = daText;
                else
                    Title = memberExpression.Member.Name;
            }
        }
    }

    IQueryable<TGridItem> ISortableColumn<TGridItem>.ApplySort(IQueryable<TGridItem> Source, bool IsFirst)
    {
        return _sortProvider!.ApplySort(this, Source, IsFirst);
    }

    /// <inheritdoc />
    protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
        => builder.AddContent(0, _cellTextFunc!(item));

}
