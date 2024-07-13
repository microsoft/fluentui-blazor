using System.Linq.Expressions;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Criteria descriptor condition.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public class DataFilterCriteriaCondition<TItem>
{
    /// <summary>
    /// Gets or sets field
    /// </summary>
    public string Field { get; set; } = default!;

    /// <summary>
    /// Gets or sets value.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Gets or sets filter id.
    /// </summary>
    public string FilterId { get; set; } = default!;

    /// <summary>
    /// Comparison Operator.
    /// </summary>
    public DataFilterComparisonOperator Operator { get; set; }

    /// <summary>
    /// To expression.
    /// </summary>
    /// <param name="caseSensitivity"></param>
    /// <returns></returns>
    public Expression<Func<TItem, bool>> ToExpression(DataFilterCaseSensitivity caseSensitivity)
        => !string.IsNullOrEmpty(Field)
                ? DataFilterHelper.GenerateExpression<TItem>(TypeHelper.CreateExpression(typeof(TItem), Field),
                                                                Value,
                                                                Operator,
                                                                caseSensitivity)
                : x => false;

    /// <summary>
    /// Clone object.
    /// </summary>
    /// <returns></returns>
    public DataFilterCriteriaCondition<TItem> Clone()
        => new()
        {
            Field = Field,
            FilterId=FilterId,
            Operator = Operator,
            Value = Value
        };
}
