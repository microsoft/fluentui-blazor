using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Criteria descriptor condition.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public class DataFilterCriteriaCondition<TItem>
{
    private FilterBase<TItem> _filter = default!;
    private DataFilterComparisonOperator _operator;

    [JsonIgnore]
    internal FilterBase<TItem> Filter
    {
        get => _filter;
        set
        {
            _filter = value;
            Field = _filter?.Id!;

            if (_filter != null)
            {
                Operator = _filter.Operators.FirstOrDefault();

                if (Value == null
                    || (Value != null && Value.GetType() != _filter.Type))
                {
                    Value = _filter.GetDefaultValue(false);
                }
            }
        }
    }

    /// <summary>
    /// Gets or sets Field
    /// </summary>
    public string Field { get; set; } = default!;

    /// <summary>
    /// Gets or sets Value.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Gets or sets Type.
    /// </summary>
    public string Type { get; set; } = default!;

    /// <summary>
    /// Comparison Operator.
    /// </summary>
    /// <summary>
    /// Comparison Operator.
    /// </summary>
    public DataFilterComparisonOperator Operator
    {
        get => _operator;
        set
        {
            if (value == DataFilterComparisonOperator.Empty || value == DataFilterComparisonOperator.NotEmpty)
            {
                Value = null;
            }
            else if (_operator != value
                && (value == DataFilterComparisonOperator.In || value == DataFilterComparisonOperator.In))
            {
                Value = _filter?.GetDefaultValue(true);
            }
            else if (Value == null)
            {
                Value = _filter?.GetDefaultValue(false);
            }

            _operator = value;
        }
    }

    /// <summary>
    /// To expression.
    /// </summary>
    /// <param name="caseSensitivity"></param>
    /// <returns></returns>
    public Expression<Func<TItem, bool>> ToExpression(DataFilterCaseSensitivity caseSensitivity)
    {
        Expression<Func<TItem, bool>> ret = x => true;

        if (Filter == null)
        {
            if (!string.IsNullOrEmpty(Field))
            {
                var propertyInfo = typeof(TItem).GetProperty(Field);
                if (propertyInfo != null)
                {
                    ret = DataFilterHelper.GenerateExpression<TItem>(TypeHelper.CreateExpression(typeof(TItem), Field),
                                                                     Value,
                                                                     Operator,
                                                                     caseSensitivity);
                }
            }
        }
        else
        {
            ret = Filter.ToExpression(Value, Operator, caseSensitivity);
        }

        return ret;
    }

    /// <summary>
    /// Clone object.
    /// </summary>
    /// <returns></returns>
    public DataFilterCriteriaCondition<TItem> Clone()
        => new()
        {
            Filter = Filter,
            Operator = Operator,
            Value = Value
        };
}
