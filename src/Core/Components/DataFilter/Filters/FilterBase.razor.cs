using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using System.Collections;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Text.Json.Serialization;

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
    /// Gets or sets the JSON Converter.
    /// </summary>
    [Parameter]
    public JsonConverter<object>? JsonConverter { get; set; }

    /// <summary>
    /// Property info definition.
    /// </summary>
    public Type Type { get; protected set; } = default!;

    private static readonly HashSet<Type> _numericTypes =
    [
        typeof(int),
        typeof(double),
        typeof(decimal),
        typeof(long),
        typeof(short),
        typeof(sbyte),
        typeof(byte),
        typeof(ulong),
        typeof(ushort),
        typeof(uint),
        typeof(float),
        typeof(BigInteger),
        typeof(int?),
        typeof(double?),
        typeof(decimal?),
        typeof(long?),
        typeof(short?),
        typeof(sbyte?),
        typeof(byte?),
        typeof(ulong?),
        typeof(ushort?),
        typeof(uint?),
        typeof(float?),
        typeof(BigInteger?),
    ];

    private bool IsType<TType>() => Type == typeof(TType) || Nullable.GetUnderlyingType(Type) == typeof(TType);

    /// <summary>
    /// Property is enum.
    /// </summary>
    protected internal bool IsEnum => Type.IsEnum || Nullable.GetUnderlyingType(Type) is { IsEnum: true };

    /// <summary>
    /// Property is bool.
    /// </summary>
    protected internal bool IsBool => IsType<bool>();

    /// <summary>
    /// Property is date.
    /// </summary>
    protected internal bool IsDate => IsType<DateTime>() || IsType<DateOnly>() || IsType<TimeOnly>();

    /// <summary>
    /// Property is string.
    /// </summary>
    protected internal bool IsString => Type == typeof(string);

    /// <summary>
    /// Property is number.
    /// </summary>
    protected internal bool IsNumber => _numericTypes.Contains(Type);

    /// <summary>
    /// Property is nullable.
    /// </summary>
    protected internal bool IsNullable => Nullable.GetUnderlyingType(Type) != null;

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
        if (!DataFilter.Properties.Contains(this))
        {
            if (string.IsNullOrEmpty(Id))
            {
                throw new ArgumentException($"Filter '{Name}' - Id empty not allow!");
            }

            if (DataFilter.Properties.Any(a => a.Id == Id))
            {
                throw new ArgumentException($"Filter '{Name}' - Id '{Id}' it already exists!");
            }

            DataFilter.Properties.Add(this);
        }
    }

    internal object GetDefaultValue(bool isMultiValues)
    {
        object value = null!;
        if (IsEnum)
        {
            value = Enum.GetValues(Type).GetValue(0)!;
        }
        else if (IsString)
        {
            value = string.Empty;
        }
        else
        {
            value = Activator.CreateInstance(Type)!;
        }

        if (isMultiValues)
        {
            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(Type))!;
            list.Add(value);
            value = list;
        }

        return value;
    }

    protected static T ConvertTo<T>(object? value) => (T)Convert.ChangeType(value, typeof(T))!;

    /// <summary>
    /// Get filter expression
    /// </summary>
    /// <param name="value"></param>
    /// <param name="operator"></param>
    /// <param name="caseSensitivity"></param>
    /// <returns></returns>
    public abstract Expression<Func<TItem, bool>> GetFilter(object? value,
                                                            DataFilterComparisonOperator @operator,
                                                            DataFilterCaseSensitivity caseSensitivity);

    /// <summary>
    /// Get operators
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerable<DataFilterComparisonOperator> Operators
    {
        get
        {
            var operators = new List<DataFilterComparisonOperator>();
            if (IsEnum)
            {
                operators.Add(DataFilterComparisonOperator.Equal);
                operators.Add(DataFilterComparisonOperator.NotEqual);
                //operators.Add(DataFilterComparisonOperator.In);
                //operators.Add(DataFilterComparisonOperator.NotIn);
            }
            else if (IsBool)
            {
                operators.Add(DataFilterComparisonOperator.Equal);
            }
            else if (IsDate || IsNumber)
            {
                operators.AddRange([
                    DataFilterComparisonOperator.Equal,
                    DataFilterComparisonOperator.NotEqual,
                    DataFilterComparisonOperator.LessThan,
                    DataFilterComparisonOperator.LessThanOrEqual,
                    DataFilterComparisonOperator.GreaterThan,
                    DataFilterComparisonOperator.GreaterThanOrEqual,
                    //DataFilterComparisonOperator.In,
                    //DataFilterComparisonOperator.NotIn,
                ]);
            }
            else if (IsString)
            {
                operators.AddRange([
                    DataFilterComparisonOperator.Equal,
                    DataFilterComparisonOperator.NotEqual,
                    DataFilterComparisonOperator.Contains,
                    DataFilterComparisonOperator.NotContains,
                    DataFilterComparisonOperator.StartsWith,
                    DataFilterComparisonOperator.NotStartsWith,
                    DataFilterComparisonOperator.EndsWith,
                    DataFilterComparisonOperator.NotEndsWith,
                    DataFilterComparisonOperator.Empty,
                    DataFilterComparisonOperator.NotEmpty,
                    //DataFilterComparisonOperator.In,
                    //DataFilterComparisonOperator.NotIn,
                ]);
            }

            if (IsNullable)
            {
                if (!operators.Contains(DataFilterComparisonOperator.Empty))
                {
                    operators.Add(DataFilterComparisonOperator.Empty);
                }

                if (!operators.Contains(DataFilterComparisonOperator.NotEmpty))
                {
                    operators.Add(DataFilterComparisonOperator.NotEmpty);
                }
            }

            //operators.Add(DataFilterComparisonOperator.In);
            //operators.Add(DataFilterComparisonOperator.NotIn);

            return operators.Distinct();
        }
    }

    protected async Task SetValueAsync(DataFilterDescriptorCondition<TItem> condition, object value)
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
        else if (IsEnum)
        {
            return (obj as Enum)?.GetDisplayName() + "";
        }
        else
        {
            return obj + "";
        }
    }

    protected static LambdaExpression CreateExpression(Type type, string filed)
    {
        var param = Expression.Parameter(type, "a");
        Expression body = param;
        foreach (var member in filed.Split('.'))
        {
            body = Expression.PropertyOrField(body, member);
        }
        return Expression.Lambda(body, param);
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

    private Dictionary<string, object> CreateNumericFieldEditorParameter(DataFilterDescriptorCondition<TItem> condition)
    {
        var inputHelper = typeof(InputHelpers<>).MakeGenericType(Type);
        return new Dictionary<string, object>()
        {
            [nameof(FluentNumberField<int>.Value)] = Convert.ChangeType(condition.Value, Type)!,

            [nameof(FluentNumberField<int>.Immediate)] = DataFilter.Immediate,
            [nameof(FluentNumberField<int>.ImmediateDelay)] = DataFilter.ImmediateDelay,

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
