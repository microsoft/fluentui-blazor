using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class DataFilterDescriptorCondition<TItem>
{
    private FilterBase<TItem> _filter = default!;
    private DataFilterComparisonOperator _operator;

    internal FilterBase<TItem> Filter
    {
        get => _filter;
        set
        {
            _filter = value;

            if (_filter != null)
            {
                var operators = _filter.Operators.ToList();
                if (operators.Count != 0 && !operators.Contains(Operator))
                {
                    Operator = operators[0];
                }

                if (Value == null
                    || (Value != null && Value.GetType() != _filter.Type))
                {
                    Value = _filter.GetDefaultValue(false);
                }
            }
        }
    }

    internal RenderFragment Render() => Filter.Render(this);

    /// <summary>
    /// Name of property.
    /// </summary>
    public string Field => Filter?.Id!;

    ///// <summary>
    ///// Type filter.
    ///// </summary>
    //public string Type => Filter?.GetType().Name!;

    /// <summary>
    /// Value.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Comparison Operator.
    /// </summary>
    public DataFilterComparisonOperator Operator
    {
        get => _operator;
        set
        {
            if (_operator != value
                && (_operator == DataFilterComparisonOperator.In || value == DataFilterComparisonOperator.In))
            {
                Value = _filter.GetDefaultValue(true);
            }

            _operator = value;
        }
    }

    /// <summary>
    /// Get property filter expression.
    /// </summary>
    /// <param name="caseSensitivity"></param>
    /// <returns></returns>
    public Expression<Func<TItem, bool>> GetFilter(DataFilterCaseSensitivity caseSensitivity)
        => Filter == null
            ? x => true
            : Filter.GetFilter(Value, Operator, caseSensitivity);
}
