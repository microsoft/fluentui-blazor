using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class DataFilterHelper
{
    public static string Serialize<TItem>(DataFilterDescriptor<TItem> group)
    {
        return JsonSerializer.Serialize(group, new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter(),
            }
        });
    }

    public static DataFilterDescriptor<TItem> Deserialize<TItem>(string data)
    {
        var aa = JsonSerializer.Deserialize<DataFilterDescriptor<TItem>>(data, new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            }
        });

        return new();
    }

    public static Expression<Func<TItem, bool>> GenerateExpression<TItem>(LambdaExpression expression,
                                                                          object? value,
                                                                          DataFilterComparisonOperator @operator,
                                                                          DataFilterCaseSensitivity caseSensitivity)
    {
        Expression<Func<TItem, bool>> ret = x => true;

        if (expression.Body.Type == typeof(string))
        {
            var valueStr = value?.ToString();

            Expression<Func<string?, bool>> func;
            if (caseSensitivity == DataFilterCaseSensitivity.Ignore)
            {
                func = @operator switch
                {
                    DataFilterComparisonOperator.Contains => a => a != null && valueStr != null && a.Contains((string)valueStr),
                    DataFilterComparisonOperator.NotContains => a => a != null && valueStr != null && !a.Contains((string)valueStr),
                    DataFilterComparisonOperator.Equal => a => a != null && a.Equals((string?)valueStr),
                    DataFilterComparisonOperator.NotEqual => a => a != null && !a.Equals((string?)valueStr),
                    DataFilterComparisonOperator.StartsWith => a => a != null && valueStr != null && a.StartsWith((string)valueStr),
                    DataFilterComparisonOperator.EndsWith => a => a != null && valueStr != null && a.EndsWith((string)valueStr),
                    DataFilterComparisonOperator.Empty => a => string.IsNullOrWhiteSpace(a),
                    DataFilterComparisonOperator.NotEmpty => a => !string.IsNullOrWhiteSpace(a),
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
                    DataFilterComparisonOperator.Contains => a => a != null && valueStr != null && a.Contains((string)valueStr, comparer),
                    DataFilterComparisonOperator.NotContains => a => a != null && valueStr != null && !a.Contains((string)valueStr, comparer),
                    DataFilterComparisonOperator.Equal => a => a != null && a.Equals((string?)valueStr, comparer),
                    DataFilterComparisonOperator.NotEqual => a => a != null && !a.Equals((string?)valueStr, comparer),
                    DataFilterComparisonOperator.StartsWith => a => a != null && valueStr != null && a.StartsWith((string)valueStr, comparer),
                    DataFilterComparisonOperator.EndsWith => a => a != null && valueStr != null && a.EndsWith((string)valueStr, comparer),
                    DataFilterComparisonOperator.Empty => x => string.IsNullOrWhiteSpace(x),
                    DataFilterComparisonOperator.NotEmpty => x => !string.IsNullOrWhiteSpace(x),
                    _ => x => true
                };
            }

            ret = expression.Make<TItem>(func);
        }
        else
        {
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
                _ => x => true
            };
        }

        return ret;
    }
}
