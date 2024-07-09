using System.Linq.Expressions;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class DataFilterHelper
{
    public static Expression<Func<TItem, bool>> GenerateExpression<TItem>(LambdaExpression expression,
                                                                          object? value,
                                                                          DataFilterComparisonOperator @operator,
                                                                          DataFilterCaseSensitivity caseSensitivity)
    {
        Expression<Func<TItem, bool>> ret = x => true;
        if (expression.Body.Type == typeof(string))
        {
            var valueStr = value?.ToString();
            var multiValues = (value as IEnumerable<string>)!;

            Expression<Func<string?, bool>> func;
            if (caseSensitivity == DataFilterCaseSensitivity.Ignore)
            {
                func = @operator switch
                {
                    DataFilterComparisonOperator.Contains => a => a != null && valueStr != null && a.Contains(valueStr),
                    DataFilterComparisonOperator.NotContains => a => a != null && valueStr != null && !a.Contains(valueStr),
                    DataFilterComparisonOperator.Equal => a => a != null && a.Equals(valueStr),
                    DataFilterComparisonOperator.NotEqual => a => a != null && !a.Equals(valueStr),
                    DataFilterComparisonOperator.StartsWith => a => a != null && valueStr != null && a.StartsWith(valueStr),
                    DataFilterComparisonOperator.NotStartsWith => a => a != null && valueStr != null && !a.StartsWith(valueStr),
                    DataFilterComparisonOperator.EndsWith => a => a != null && valueStr != null && a.EndsWith(valueStr),
                    DataFilterComparisonOperator.NotEndsWith => a => a != null && valueStr != null && !a.EndsWith(valueStr),
                    DataFilterComparisonOperator.Empty => a => string.IsNullOrWhiteSpace(a),
                    DataFilterComparisonOperator.NotEmpty => a => !string.IsNullOrWhiteSpace(a),
                    DataFilterComparisonOperator.In => a => multiValues.Contains(a),
                    DataFilterComparisonOperator.NotIn => a => !multiValues.Contains(a),
                    _ => a => true
                };
            }
            else
            {
                var comparer = caseSensitivity == DataFilterCaseSensitivity.Default
                                ? StringComparison.Ordinal
                                : StringComparison.OrdinalIgnoreCase;

                func = @operator switch
                {
                    DataFilterComparisonOperator.Contains => a => a != null && valueStr != null && a.Contains(valueStr, comparer),
                    DataFilterComparisonOperator.NotContains => a => a != null && valueStr != null && !a.Contains(valueStr, comparer),
                    DataFilterComparisonOperator.Equal => a => a != null && a.Equals(valueStr, comparer),
                    DataFilterComparisonOperator.NotEqual => a => a != null && !a.Equals(valueStr, comparer),
                    DataFilterComparisonOperator.StartsWith => a => a != null && valueStr != null && a.StartsWith(valueStr, comparer),
                    DataFilterComparisonOperator.NotStartsWith => a => a != null && valueStr != null && !a.StartsWith(valueStr, comparer),
                    DataFilterComparisonOperator.EndsWith => a => a != null && valueStr != null && a.EndsWith(valueStr, comparer),
                    DataFilterComparisonOperator.NotEndsWith => a => a != null && valueStr != null && !a.EndsWith(valueStr, comparer),
                    DataFilterComparisonOperator.Empty => x => string.IsNullOrWhiteSpace(x),
                    DataFilterComparisonOperator.NotEmpty => x => !string.IsNullOrWhiteSpace(x),
                    DataFilterComparisonOperator.In => a => multiValues.Contains(a),
                    DataFilterComparisonOperator.NotIn => a => !multiValues.Contains(a),
                    _ => x => true
                };
            }

            ret = expression.Make<TItem>(func);
        }
        else
        {
            var multiValues = (value as IEnumerable<object>)!;

            ret = @operator switch
            {
                DataFilterComparisonOperator.Equal => expression.Make<TItem>(ExpressionType.Equal, value),
                DataFilterComparisonOperator.NotEqual => expression.Make<TItem>(ExpressionType.NotEqual, value),
                DataFilterComparisonOperator.GreaterThan => expression.Make<TItem>(ExpressionType.GreaterThan, value),
                DataFilterComparisonOperator.GreaterThanOrEqual => expression.Make<TItem>(ExpressionType.GreaterThanOrEqual, value),
                DataFilterComparisonOperator.LessThan => expression.Make<TItem>(ExpressionType.LessThan, value),
                DataFilterComparisonOperator.LessThanOrEqual => expression.Make<TItem>(ExpressionType.LessThanOrEqual, value),
                DataFilterComparisonOperator.Empty => expression.Make<TItem>(ExpressionType.Equal, null),
                DataFilterComparisonOperator.NotEmpty => expression.Make<TItem>(ExpressionType.NotEqual, null),
                DataFilterComparisonOperator.In => expression.MakeContains<TItem>(false, multiValues),
                DataFilterComparisonOperator.NotIn => expression.MakeContains<TItem>(true, multiValues),
                _ => x => true
            };
        }

        return ret;
    }
}
