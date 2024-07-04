using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class DataFilterDescriptorProperty<TItem>
{
    private PropertyFilterBase<TItem> _property = default!;

    internal PropertyFilterBase<TItem> Property
    {
        get => _property;
        set
        {
            _property = value;

            if (_property != null)
            {
                var operators = _property.Operators.ToList();
                if (operators.Any() && !operators.Contains(Operator))
                {
                    Operator = operators[0];
                }

                if (Value == null
                    || (Value != null && Value.GetType() != _property.Type))
                {
                    _property.SetDefaultValue(this);
                }
            }
        }
    }

    /// <summary>
    /// Name of property.
    /// </summary>
    public string Field => Property?.Id!;

    /// <summary>
    /// Value serialized
    /// </summary>
    [JsonPropertyName("Value")]
    public string ValueSerialized => Property?.SerializeValue(Value)!;

    /// <summary>
    /// Type filter.
    /// </summary>
    public string Type => Property?.GetType().Name!;

    /// <summary>
    /// Value.
    /// </summary>
    [JsonIgnore]
    public object? Value { get; set; }

    /// <summary>
    /// Comparison Operator.
    /// </summary>
    public DataFilterComparisonOperator Operator { get; set; }

    /// <summary>
    /// Get property filter expression.
    /// </summary>
    /// <param name="caseSensitivity"></param>
    /// <returns></returns>
    public Expression<Func<TItem, bool>> GetFilter(DataFilterCaseSensitivity caseSensitivity)
        => Property == null
            ? x => true
            : Property.GetFilter(Value, Operator, caseSensitivity);
}
