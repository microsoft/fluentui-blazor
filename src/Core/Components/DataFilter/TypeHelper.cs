using System.Linq.Expressions;
using System.Numerics;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Type helper
/// </summary>
public class TypeHelper
{
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

    private static bool IsType<TType>(Type type) => type == typeof(TType) || Nullable.GetUnderlyingType(type) == typeof(TType);

    /// <summary>
    /// Property is enum.
    /// </summary>
    public static bool IsEnum(Type type) => type.IsEnum || Nullable.GetUnderlyingType(type) is { IsEnum: true };

    /// <summary>
    /// Property is bool.
    /// </summary>
    public static bool IsBool(Type type) => IsType<bool>(type);

    /// <summary>
    /// Property is date.
    /// </summary>
    public static bool IsDate(Type type) => IsType<DateTime>(type) || IsType<DateOnly>(type) || IsType<TimeOnly>(type);

    /// <summary>
    /// Property is string.
    /// </summary>
    public static bool IsString(Type type) => type == typeof(string);

    /// <summary>
    /// Property is number.
    /// </summary>
    public static bool IsNumber(Type type) => _numericTypes.Contains(type);

    /// <summary>
    /// Property is nullable.
    /// </summary>
    public static bool IsNullable(Type type) => Nullable.GetUnderlyingType(type) != null;

    /// <summary>
    /// Create expression from filed.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="filed"></param>
    /// <returns></returns>
    public static LambdaExpression CreateExpression(Type type, string filed)
    {
        var param = Expression.Parameter(type, "a");
        Expression body = param;
        foreach (var member in filed.Split('.'))
        {
            body = Expression.PropertyOrField(body, member);
        }
        return Expression.Lambda(body, param);
    }

}
