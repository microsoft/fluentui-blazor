using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Components.DataFilter.Infrastructure;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using System.Collections;
using System.Linq.Expressions;

namespace Microsoft.FluentUI.AspNetCore.Components;

public abstract partial class FilterBase<TItem>
{
    [CascadingParameter]
    internal FluentDataFilter<TItem> DataFilter { get; set; } = default!;

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    [Parameter]
    public string Title { get; set; } = default!;

    /// <summary>
    /// Gets or sets the unique usage.
    /// </summary>
    [Parameter]
    public bool Unique { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine which text to display.
    /// </summary>
    [Parameter]
    public Func<object?, string>? ValueDisplayText { get; set; }

    /// <summary>
    /// Gets or sets the format string for the value.
    /// </summary>
    [Parameter]
    public string? Format { get; set; }

    /// <summary>
    /// Gets or sets type definition.
    /// </summary>
    public Type Type { get; protected set; } = default!;

    /// <summary>
    /// Get Filter name
    /// </summary>
    public string Name => GetType().Name[..GetType().Name.IndexOf('`')];

    /// <summary>
    /// Gets field,
    /// </summary>
    public string FieldPath { get; protected set; } = default!;

    protected override void OnParametersSet()
    {
        if (!DataFilter.Filters.Contains(this))
        {
            if (string.IsNullOrEmpty(Id))
            {
                throw new ArgumentException($"Filter '{Name}' - Id empty not allow!");
            }

            if (DataFilter.Filters.Any(a => a.Id == Id))
            {
                throw new ArgumentException($"Filter '{Name}' - Id '{Id}' it already exists!");
            }

            DataFilter.Filters.Add(this);
        }
    }

    /// <summary>
    /// Get enum values
    /// </summary>
    protected IEnumerable<Enum> EnumValues
        => Enum.GetValues(TypeHelper.IsNullable(Type)
                            ? Type.GetGenericArguments()[0]
                            : Type).Cast<Enum>();

    private static T ConvertTo<T>(object? value) => (T)Convert.ChangeType(value, typeof(T))!;

    /// <summary>
    /// Get filter expression.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="operator"></param>
    /// <param name="caseSensitivity"></param>
    /// <returns></returns>
    public abstract Expression<Func<TItem, bool>> ToExpression(object? value,
                                                               DataFilterComparisonOperator @operator,
                                                               DataFilterCaseSensitivity caseSensitivity);

    private IEnumerable<DataFilterComparisonOperator> Operators => DataFilterHelper.GetOperators(Type, DataFilter.Items != null);

    private async Task AfterSetOperatorAsync(DataFilterCriteriaCondition<TItem> condition)
    {
        if (condition.Operator.IsEmpty())
        {
            condition.Value = null;
        }
        else if (condition.Operator.IsIn())
        {
            var defaultValue = TypeHelper.GetDefaultValue(Type, true);
            if (condition.Value?.GetType() != defaultValue.GetType())
            {
                condition.Value = defaultValue;
            }
        }
        else if (condition.Value == null
                 //previous change was enumerable
                 || (!condition.Operator.IsIn() && condition.Value is IEnumerable))
        {
            condition.Value = TypeHelper.GetDefaultValue(Type, false);
        }

        await DataFilter.FilterChangedAsync();
    }

    /// <summary>
    /// Set value and call filter changed.
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    protected internal async Task SetValueAsync(DataFilterCriteriaCondition<TItem> condition, object? value)
    {
        condition.Value = value;
        await DataFilter.FilterChangedAsync();
    }

    protected string DisplayText(DataFilterComparisonOperator value)
        => DataFilter.ComparisonOperatorDisplayText == null
                ? value.GetDisplayName()!
                : DataFilter.ComparisonOperatorDisplayText.Invoke(value);

    protected internal abstract LambdaExpression ExpressionDef { get; }

    protected internal string ValueDisplayTextInt(object? obj)
    {
        if (ValueDisplayText != null)
        {
            return ValueDisplayText.Invoke(obj);
        }
        else if (TypeHelper.IsEnum(Type))
        {
            return (obj as Enum)?.GetDisplayName() + "";
        }
        else
        {
            return obj + "";
        }
    }

    private Type SelectorInEditorType => typeof(FilterSelectorInEditor<,>).MakeGenericType(typeof(TItem), Type);
    private Type NumericEditorType => typeof(FilterNumericEditor<,>).MakeGenericType(typeof(TItem), Type);

    private Dictionary<string, object> CreateEditorParameter(DataFilterCriteriaCondition<TItem> condition, bool readOnly)
    {
        return new Dictionary<string, object>()
        {
            [nameof(FilterSelectorInEditor<object, object>.Condition)] = condition,
            [nameof(FilterSelectorInEditor<object, object>.ReadOnly)] = readOnly,
            [nameof(FilterSelectorInEditor<object, object>.Filter)] = this,
        };
    }
}
