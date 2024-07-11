using System.Linq.Expressions;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Data filter helper.
/// </summary>
public class DataFilterHelper
{
    /// <summary>
    /// Get operators from type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<DataFilterComparisonOperator> GetOperators(Type type)
    {
        var operators = new List<DataFilterComparisonOperator>();
        if (TypeHelper.IsEnum(type))
        {
            operators.Add(DataFilterComparisonOperator.Equal);
            operators.Add(DataFilterComparisonOperator.NotEqual);
            //operators.Add(DataFilterComparisonOperator.In);
            //operators.Add(DataFilterComparisonOperator.NotIn);
        }
        else if (TypeHelper.IsBool(type))
        {
            operators.Add(DataFilterComparisonOperator.Equal);
        }
        else if (TypeHelper.IsDate(type) || TypeHelper.IsNumber(type))
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
        else if (TypeHelper.IsString(type))
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

        if (TypeHelper.IsNullable(type))
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

    /// <summary>
    /// Get default comparison operator.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static DataFilterComparisonOperator GetDefaultComparisonOperator(Type type)
    {
        if (TypeHelper.IsEnum(type))
        {
            return DataFilterComparisonOperator.Equal;
        }
        else if (TypeHelper.IsNumber(type))
        {
            return DataFilterComparisonOperator.Equal;
        }
        else if (TypeHelper.IsDate(type))
        {
            return DataFilterComparisonOperator.Equal;
        }
        else if (TypeHelper.IsBool(type))
        {
            return DataFilterComparisonOperator.Equal;
        }
        else if (TypeHelper.IsString(type))
        {
            return DataFilterComparisonOperator.Contains;
        }

        return DataFilterComparisonOperator.Equal;
    }

    /// <summary>
    /// Set logical operator.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="operator"></param>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Expression<Func<TItem, bool>> SetOperator<TItem>(DataFilterLogicalOperator @operator,
                                                                   Expression<Func<TItem, bool>> first,
                                                                   Expression<Func<TItem, bool>> second)
        => @operator switch
        {
            DataFilterLogicalOperator.And or DataFilterLogicalOperator.NotAnd => PredicateBuilder.And(first, second),
            DataFilterLogicalOperator.Or or DataFilterLogicalOperator.NotOr => PredicateBuilder.Or(first, second),
            _ => first,
        };

    /// <summary>
    /// Get expression.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="value"></param>
    /// <param name="caseSensitivity"></param>
    /// <param name="properties"></param>
    /// <returns></returns>
    public static Expression<Func<TItem, bool>> GenerateExpression<TItem>(object? value,
                                                                          DataFilterCaseSensitivity caseSensitivity,
                                                                          params Expression<Func<TItem, object>>[] properties)
    {
        var ret = PredicateBuilder.True<TItem>();
        if (value != null)
        {
            var typeValue = value.GetType();

            foreach (var item in properties)
            {
                var type = item.Body.Type;

                var valid = false;

                if (TypeHelper.IsEnum(type))
                {
                    valid = Enum.TryParse(type, value.ToString(), true, out var result);
                }
                else if (TypeHelper.IsNumber(type))
                {
                    valid = decimal.TryParse(value.ToString(), out var result);
                }
                if (type == typeof(DateTime) || type == typeof(DateTime?))
                {
                    valid = DateTime.TryParse(value.ToString(), out var result);
                }
                else if (type == typeof(DateOnly) || type == typeof(DateOnly?))
                {
                    valid = DateOnly.TryParse(value.ToString(), out var result);
                }
                else if (type == typeof(TimeOnly) || type == typeof(TimeOnly?))
                {
                    valid = TimeOnly.TryParse(value.ToString(), out var result);
                }
                else if (TypeHelper.IsBool(type))
                {
                    valid = bool.TryParse(value.ToString(), out var result);
                }
                else if (TypeHelper.IsString(type))
                {
                    valid = true;
                }

                if (valid)
                {
                    ret = SetOperator(DataFilterLogicalOperator.Or,
                                      ret,
                                      GenerateExpression<TItem>(item, value, GetDefaultComparisonOperator(type), caseSensitivity));
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// Generate expression.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="expression"></param>
    /// <param name="value"></param>
    /// <param name="operator"></param>
    /// <param name="caseSensitivity"></param>
    /// <returns></returns>
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
