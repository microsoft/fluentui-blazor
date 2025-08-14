// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a <see cref="FluentDataGrid{TGridItem}"/> column whose cells display a single value.
/// </summary>
/// <typeparam name="TGridItem">The type of data represented by each row in the grid.</typeparam>
/// <typeparam name="TProp">The type of the value being displayed in the column's cells.</typeparam>
public class PropertyColumn<TGridItem, TProp> : ColumnBase<TGridItem>, IBindableColumn<TGridItem, TProp>
{

    private Expression<Func<TGridItem, TProp>>? _lastAssignedProperty;
    private Func<TGridItem, string?>? _cellTextFunc;
    private Func<TGridItem, string?>? _cellTooltipTextFunc;
    private IGridSort<TGridItem>? _sortBuilder;
    private IGridSort<TGridItem>? _customSortBy;

    public PropertyInfo? PropertyInfo { get; private set; }

    /// <summary>
    /// Defines the value to be displayed in this column's cells.
    /// </summary>
    [Parameter, EditorRequired] public Expression<Func<TGridItem, TProp>> Property { get; set; } = default!;

    /// <summary>
    /// Optionally specifies a format string for the value.
    ///
    /// Using this requires the <typeparamref name="TProp"/> type to implement <see cref="IFormattable" />.
    /// </summary>
    [Parameter] public string? Format { get; set; }

    /// <summary>
    /// Optionally specifies how to compare values in this column when sorting.
    ///
    /// Using this requires the <typeparamref name="TProp"/> type to implement <see cref="IComparable{T}"/>.
    /// </summary>
    [Parameter] public IComparer<TProp>? Comparer { get; set; } = null;

    [Parameter]
    public override IGridSort<TGridItem>? SortBy
    {
        get => _customSortBy ?? _sortBuilder;
        set => _customSortBy = value;
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        // We have to do a bit of pre-processing on the lambda expression. Only do that if it's new or changed.
        if (_lastAssignedProperty != Property)
        {
            _lastAssignedProperty = Property;
            var compiledPropertyExpression = Property.Compile();

            if (!string.IsNullOrEmpty(Format))
            {
                _cellTextFunc = CreateFormatter(compiledPropertyExpression, Format);
            }
            else
            {
                _cellTextFunc = item =>
                {
                    var value = compiledPropertyExpression!(item);

                    if (typeof(TProp).IsEnum || typeof(TProp).IsNullableEnum())
                    {
                        return (value as Enum)?.GetDisplayName();
                    }

                    return value?.ToString();
                };
            }
            if (Sortable.HasValue)
            {
                _sortBuilder = Comparer is not null ? GridSort<TGridItem>.ByAscending(Property, Comparer) : GridSort<TGridItem>.ByAscending(Property);
            }
        }

        _cellTooltipTextFunc = TooltipText ?? _cellTextFunc;
        if (Property.Body is MemberExpression memberExpression)
        {
            if (Title is null)
            {
                PropertyInfo = memberExpression.Member as PropertyInfo;
                var daText = memberExpression.Member.DeclaringType?.GetDisplayAttributeString(memberExpression.Member.Name);
                if (!string.IsNullOrEmpty(daText))
                {
                    Title = daText;
                }
                else
                {
                    Title = memberExpression.Member.Name;
                }
            }
        }
    }

    private static Func<TGridItem, string?> CreateFormatter(Func<TGridItem, TProp> getter, string format)
    {
        var closedType = typeof(PropertyColumn<,>).MakeGenericType(typeof(TGridItem), typeof(TProp));

        //Nullable struct
        if (Nullable.GetUnderlyingType(typeof(TProp)) is Type underlying &&
            typeof(IFormattable).IsAssignableFrom(underlying))
        {
            var method = closedType
                .GetMethod(nameof(CreateNullableValueTypeFormatter), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(underlying);
            return (Func<TGridItem, string?>)method.Invoke(null, [getter, format])!;
        }


        if (typeof(IFormattable).IsAssignableFrom(typeof(TProp)))
        {
            //Struct
            if (typeof(TProp).IsValueType)
            {
                var method = closedType
                    .GetMethod(nameof(CreateValueTypeFormatter), BindingFlags.NonPublic | BindingFlags.Static)!
                    .MakeGenericMethod(typeof(TProp));
                return (Func<TGridItem, string?>)method.Invoke(null, [getter, format])!;
            }

            return CreateReferenceTypeFormatter((Func<TGridItem, IFormattable?>)(object)getter, format);
        }

        throw new InvalidOperationException($"A '{nameof(Format)}' parameter was supplied, but the type '{typeof(TProp)}' does not implement '{typeof(IFormattable)}'.");
    }

    private static Func<TGridItem, string?> CreateReferenceTypeFormatter<T>(Func<TGridItem, T?> getter, string format)
        where T : class, IFormattable
    {
        return item => getter(item)?.ToString(format, null);
    }

    private static Func<TGridItem, string?> CreateValueTypeFormatter<T>(Func<TGridItem, T> getter, string format)
        where T : struct, IFormattable
    {
        return item => getter(item).ToString(format, null);
    }

    private static Func<TGridItem, string?> CreateNullableValueTypeFormatter<T>(Func<TGridItem, T?> getter, string format)
        where T : struct, IFormattable
    {
        return item => getter(item)?.ToString(format, null);
    }

    /// <inheritdoc />
    protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
        => builder.AddContent(0, _cellTextFunc?.Invoke(item));

    protected internal override string? RawCellContent(TGridItem item)
        => _cellTooltipTextFunc?.Invoke(item);

    protected override bool IsSortableByDefault()
        => _customSortBy is not null;
}
