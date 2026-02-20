// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

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

    /// <summary>
    /// Gets the property info for the property this column binds to.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the sorting behavior for this column.
    ///</summary>
    [Parameter]
    public override IGridSort<TGridItem>? SortBy { get; set; }

    /// <inheritdoc />
#pragma warning disable IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
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
                        return (value as Enum)?.GetDisplay();
                    }

                    return value?.ToString();
                };
            }

            if (Sortable.HasValue)
            {
                SortBy = Comparer is not null ? GridSort<TGridItem>.ByAscending(Property, Comparer) : GridSort<TGridItem>.ByAscending(Property);
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

#pragma warning restore IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
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

            //Double cast required because CreateReferenceTypeFormatter required the TProp to be a reference type which implements IFormattable.
            return CreateReferenceTypeFormatter((Func<TGridItem, IFormattable?>)(object)getter, format);
        }

        throw new InvalidOperationException($"A '{nameof(Format)}' parameter was supplied, but the type '{typeof(TProp)}' does not implement '{typeof(IFormattable)}'.");
    }

    private static Func<TGridItem, string?> CreateReferenceTypeFormatter<T>(Func<TGridItem, T?> getter, string format)
        where T : class, IFormattable
    {
        return item => getter(item)?.ToString(format, formatProvider: null);
    }

    private static Func<TGridItem, string?> CreateValueTypeFormatter<T>(Func<TGridItem, T> getter, string format)
        where T : struct, IFormattable
    {
        return item => getter(item).ToString(format, formatProvider: null);
    }

    private static Func<TGridItem, string?> CreateNullableValueTypeFormatter<T>(Func<TGridItem, T?> getter, string format)
        where T : struct, IFormattable
    {
        return item => getter(item)?.ToString(format, formatProvider: null);
    }

    /// <inheritdoc />
    protected internal override void CellContent(RenderTreeBuilder builder, TGridItem item)
        => builder.AddContent(0, _cellTextFunc?.Invoke(item));

    /// <inheritdoc />
    protected internal override string? RawCellContent(TGridItem item)
        => _cellTooltipTextFunc?.Invoke(item);

    /// <inheritdoc />
    protected override bool IsSortableByDefault()
        => SortBy is not null;
}
