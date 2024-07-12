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
    /// <param name="allowIn"></param>
    /// <returns></returns>
    public static IEnumerable<DataFilterComparisonOperator> GetOperators(Type type, bool allowIn)
    {
        var operators = new List<DataFilterComparisonOperator>();
        if (TypeHelper.IsEnum(type))
        {
            operators.Add(DataFilterComparisonOperator.Equal);
            operators.Add(DataFilterComparisonOperator.NotEqual);
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
                ]);
        }

        if (TypeHelper.IsNullable(type) && !TypeHelper.IsString(type))
        {
            operators.Add(DataFilterComparisonOperator.Empty);
            operators.Add(DataFilterComparisonOperator.NotEmpty);
        }

        if (allowIn && !TypeHelper.IsBool(type))
        {
            operators.Add(DataFilterComparisonOperator.In);
            operators.Add(DataFilterComparisonOperator.NotIn);
        }

        return operators.Distinct();
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
    /// Set property expression.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public sealed class SetPropertyExpression<TSource>
    {
        /// <summary>
        /// Gets expression.
        /// </summary>
        public LambdaExpression Expression { get; private set; } = default!;

        /// <summary>
        /// Set property
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public SetPropertyExpression<TSource> SetProperty<TProperty>(Expression<Func<TSource, TProperty>> propertyExpression)
        {
            Expression = propertyExpression;
            return this;
        }
    }

    /// <summary>
    /// Generate expression.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="value"></param>
    /// <param name="caseSensitivity"></param>
    /// <param name="properties"></param>
    /// <returns></returns>
    public static Expression<Func<TItem, bool>> GenerateExpression<TItem>(string value,
                                                                          DataFilterCaseSensitivity caseSensitivity,
                                                                          params Expression<Func<SetPropertyExpression<TItem>, SetPropertyExpression<TItem>>>[] properties)
    {
        var ret = PredicateBuilder.False<TItem>();

        void AddCondition(SetPropertyExpression<TItem> property,
                          object? value,
                          DataFilterComparisonOperator @operator = DataFilterComparisonOperator.Equal)
        {
            ret = SetOperator(DataFilterLogicalOperator.Or,
                              ret,
                              GenerateExpression<TItem>(property.Expression, value, @operator, caseSensitivity));
        }

        foreach (var item in properties)
        {
            var property = item.Compile()(new());
            var type = property.Expression.Body.Type;

            if (TypeHelper.IsEnum(type))
            {
                foreach (var enumValue in Enum.GetValues(type))
                {
                    if (enumValue.ToString()!.Contains(value)
                        || Enum.GetName(type, enumValue)!.Contains(value))
                    {
                        AddCondition(property, enumValue);
                    }
                }
            }
            else if (TypeHelper.IsNumber(type))
            {
                if (decimal.TryParse(value, out var result))
                {
                    try
                    {
                        AddCondition(property, Convert.ChangeType(result, type));
                    }
                    catch { }
                }
            }
            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                if (DateTime.TryParse(value, out var result))
                {
                    AddCondition(property, result);
                }
            }
            else if (type == typeof(DateOnly) || type == typeof(DateOnly?))
            {
                if (DateOnly.TryParse(value, out var result))
                {
                    AddCondition(property, result);
                }
            }
            else if (type == typeof(TimeOnly) || type == typeof(TimeOnly?))
            {
                if (TimeOnly.TryParse(value, out var result))
                {
                    AddCondition(property, result);
                }
            }
            else if (TypeHelper.IsBool(type))
            {
                if (bool.TryParse(value, out var result))
                {
                    AddCondition(property, result);
                }
            }
            else if (TypeHelper.IsString(type))
            {
                AddCondition(property, value, DataFilterComparisonOperator.Contains);
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
            var collection = value as ICollection<string>;

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
                    DataFilterComparisonOperator.In => a => a != null && collection != null && collection.Contains(a),
                    DataFilterComparisonOperator.NotIn => a => a != null && collection != null && !collection.Contains(a),
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
                    DataFilterComparisonOperator.In => a => a != null && collection != null && collection.Contains(a),
                    DataFilterComparisonOperator.NotIn => a => a != null && collection != null && !collection.Contains(a),
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
                DataFilterComparisonOperator.In => expression.MakeIn<TItem>(value),
                DataFilterComparisonOperator.NotIn => PredicateBuilder.Not(expression.MakeIn<TItem>(value)),
                _ => x => true
            };
        }

        return ret;
    }
}
