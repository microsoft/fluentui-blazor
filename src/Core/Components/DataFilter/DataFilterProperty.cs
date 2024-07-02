using System.Linq.Expressions;
using System.Numerics;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class DataFilterProperty<TItem>
{
    /// <summary>
    /// Property filter reference.
    /// </summary>
    public IPropertyFilter<TItem> Property { get; set; } = default!;

    /// <summary>
    /// Value.
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// Comparison Operator.
    /// </summary>
    public DataFilterComparisonOperator Operator { get; set; }

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
    /// Type of property.
    /// </summary>
    public Type Type => Property.PropertyInfo.PropertyType;

    /// <summary>
    /// Property is enum.
    /// </summary>
    public bool IsEnum => Type.IsEnum || Nullable.GetUnderlyingType(Type) is { IsEnum: true };

    /// <summary>
    /// Property is bool.
    /// </summary>
    public bool IsBool => IsType<bool>();

    /// <summary>
    /// Property is date.
    /// </summary>
    public bool IsDate => IsType<DateTime>() || IsType<DateOnly>() || IsType<TimeOnly>();

    /// <summary>
    /// Property is string.
    /// </summary>
    public bool IsString => Type == typeof(string);

    /// <summary>
    /// Property is number.
    /// </summary>
    public bool IsNumber => _numericTypes.Contains(Type);

    /// <summary>
    /// Property is nullable.
    /// </summary>
    public bool IsNullable => Nullable.GetUnderlyingType(Type) != null;

    /// <summary>
    /// Get expression for filter property.
    /// </summary>
    /// <param name="caseSensitivity"></param>
    /// <returns></returns>
    public Expression<Func<TItem, bool>> GenerateExpression(DataFilterCaseSensitivity caseSensitivity)
    {
        Expression<Func<TItem, bool>> ret = x => true;

        if (Property != null)
        {
            var le = Property.LambdaExpression;
            if (IsString)
            {
                var value = Value?.ToString();

                Expression<Func<string?, bool>> func;
                if (caseSensitivity == DataFilterCaseSensitivity.Ignore)
                {
                    func = Operator switch
                    {
                        DataFilterComparisonOperator.Contains => a => a != null && value != null && a.Contains(value),
                        DataFilterComparisonOperator.NotContains => a => a != null && value != null && !a.Contains(value),
                        DataFilterComparisonOperator.Equal => a => a != null && a.Equals(value),
                        DataFilterComparisonOperator.NotEqual => a => a != null && !a.Equals(value),
                        DataFilterComparisonOperator.StartsWith => a => a != null && value != null && a.StartsWith(value),
                        DataFilterComparisonOperator.EndsWith => a => a != null && value != null && a.EndsWith(value),
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

                    func = Operator switch
                    {
                        DataFilterComparisonOperator.Contains => a => a != null && value != null && a.Contains(value, comparer),
                        DataFilterComparisonOperator.NotContains => a => a != null && value != null && !a.Contains(value, comparer),
                        DataFilterComparisonOperator.Equal => a => a != null && a.Equals(value, comparer),
                        DataFilterComparisonOperator.NotEqual => a => a != null && !a.Equals(value, comparer),
                        DataFilterComparisonOperator.StartsWith => a => a != null && value != null && a.StartsWith(value, comparer),
                        DataFilterComparisonOperator.EndsWith => a => a != null && value != null && a.EndsWith(value, comparer),
                        DataFilterComparisonOperator.Empty => x => string.IsNullOrWhiteSpace(x),
                        DataFilterComparisonOperator.NotEmpty => x => !string.IsNullOrWhiteSpace(x),
                        _ => x => true
                    };
                }

                ret = le.Make<TItem>(func);
            }
            else
            {
                ret = Operator switch
                {
                    DataFilterComparisonOperator.Equal => le.Make<TItem>(ExpressionType.Equal, Value),
                    DataFilterComparisonOperator.NotEqual => le.Make<TItem>(ExpressionType.NotEqual, Value),
                    DataFilterComparisonOperator.GreaterThan => le.Make<TItem>(ExpressionType.GreaterThan, Value),
                    DataFilterComparisonOperator.GreaterThanOrEqual => le.Make<TItem>(ExpressionType.GreaterThanOrEqual, Value),
                    DataFilterComparisonOperator.LessThan => le.Make<TItem>(ExpressionType.LessThan, Value),
                    DataFilterComparisonOperator.LessThanOrEqual => le.Make<TItem>(ExpressionType.LessThanOrEqual, Value),
                    DataFilterComparisonOperator.Empty => le.Make<TItem>(ExpressionType.Equal, null),
                    DataFilterComparisonOperator.NotEmpty => le.Make<TItem>(ExpressionType.NotEqual, null),
                    _ => x => true
                };
            }
        }

        return ret;
    }

    public virtual IEnumerable<DataFilterComparisonOperator> GetAvailableComparisonOperator()
    {
        var operators = new List<DataFilterComparisonOperator>();

        if (IsEnum)
        {
            operators.Add(DataFilterComparisonOperator.Equal);
            operators.Add(DataFilterComparisonOperator.NotEqual);
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
                DataFilterComparisonOperator.EndsWith,
                DataFilterComparisonOperator.Empty,
                DataFilterComparisonOperator.NotEmpty,
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

        return operators.Distinct();
    }
}
