using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

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
    /// Property info definition.
    /// </summary>
    public Type Type { get; protected set; } = default!;

    /// <summary>
    /// Get Filter name
    /// </summary>
    public string Name => GetType().Name[..GetType().Name.IndexOf('`')];

    private static void SelectedOptionsChangedMany<T>(IEnumerable<T> items, List<T> selected)
    {
        selected.Clear();
        selected.AddRange(items);
    }

    private static void OnSearchMany<T>(OptionsSearchEventArgs<T> e)
    {
        //e.Items = Enum.GetValues<DataTypeDemoEnum>().Where(a => a.GetDisplayName().Contains(e.Text, StringComparison.OrdinalIgnoreCase));
    }

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

    internal object GetDefaultValue(bool isMultiValues)
    {
        var type = TypeHelper.IsNullable(Type)
                    ? Type.GetGenericArguments()[0]
                    : Type;

        object value;
        if (TypeHelper.IsEnum(Type))
        {
            value = Enum.GetValues(type).GetValue(0)!;
        }
        else if (TypeHelper.IsString(Type))
        {
            value = string.Empty;
        }
        else
        {
            value = Activator.CreateInstance(type)!;
        }

        if (isMultiValues)
        {
            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type))!;
            list.Add(value);
            value = list;
        }

        return value;
    }

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

    /// <summary>
    /// Get operators
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<DataFilterComparisonOperator> Operators => DataFilterHelper.GetOperators(Type);

    protected async Task SetValueAsync(DataFilterCriteriaCondition<TItem> condition, object? value)
    {
        condition.Value = value;
        await DataFilter.FilterChangedAsync();
    }

    protected string DisplayText(DataFilterComparisonOperator value)
        => DataFilter.ComparisonOperatorDisplayText == null
                ? value.GetDisplayName()!
                : DataFilter.ComparisonOperatorDisplayText.Invoke(value);

    protected string ValueDisplayTextInt(object? obj)
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

    protected PropertyInfo GetPropertyInfo(string field)
    {
        if (string.IsNullOrEmpty(field))
        {
            throw new ArgumentException($"Field '{field}' is empty!.");
        }

        var type = typeof(TItem);
        PropertyInfo? prop = null;
        foreach (var item in field.Split('.'))
        {
            prop = type.GetProperty(item);
            if (prop == null)
            {
                throw new ArgumentException($"Field '{field}' not valid!.");
            }
            else
            {
                type = prop.PropertyType;
            }
        }

        if (prop == null || !prop.CanRead || !prop.CanWrite)
        {
            throw new ArgumentException($"Field '{field}' not valid!.");
        }

        return prop;
    }

    private Dictionary<string, object> CreateNumericFieldEditorParameter(DataFilterCriteriaCondition<TItem> condition, bool readOnly)
    {
        var inputHelper = typeof(InputHelpers<>).MakeGenericType(Type);
        return new Dictionary<string, object>()
        {
            [nameof(FluentNumberField<int>.Value)] = condition.Value!,

            [nameof(FluentNumberField<int>.Immediate)] = DataFilter.Immediate,
            [nameof(FluentNumberField<int>.ImmediateDelay)] = DataFilter.ImmediateDelay,
            [nameof(FluentNumberField<int>.Disabled)] = readOnly,

            [nameof(FluentNumberField<int>.Min)] = inputHelper.GetMethod(nameof(InputHelpers<int>.GetMinValue))!
                                                              .Invoke(inputHelper, null)!,

            [nameof(FluentNumberField<int>.Max)] = inputHelper.GetMethod(nameof(InputHelpers<int>.GetMaxValue))!
                                                              .Invoke(inputHelper, null)!,

            [nameof(FluentNumberField<int>.ValueChanged)] = EventCallbackHelper.Make(Type,
                                                                                     this,
                                                                                     async (e) => await SetValueAsync(condition, e))!
        };
    }
}
