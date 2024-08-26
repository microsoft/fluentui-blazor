using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components.Components.DataFilter.Infrastructure;

internal class TypeHelper
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
    public static bool IsDate(Type type) => IsType<System.DateTime>(type) || IsType<DateOnly>(type) || IsType<TimeOnly>(type);

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

    /// <summary>
    /// Get PropertyInfo from file path. 
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static PropertyInfo GetPropertyInfo<TItem>(string field)
    {
        if (string.IsNullOrEmpty(field))
        {
            throw new ArgumentException($"Field '{field}' is empty!.");
        }

        var type = typeof(TItem);
        PropertyInfo? prop = null;
        foreach (var item in field.Split('.'))
        {
            prop = type.GetProperty(item);
            if (prop == null)
            {
                throw new ArgumentException($"Field '{field}' not valid!.");
            }
            else
            {
                type = prop.PropertyType;
            }
        }

        if (prop == null || !prop.CanRead || !prop.CanWrite)
        {
            throw new ArgumentException($"Field '{field}' not valid!.");
        }

        return prop;
    }

    /// <summary>
    /// Get type from field,
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="field"></param>
    /// <returns></returns>
    public static Type? GetType<TItem>(string field) =>
          string.IsNullOrEmpty(field)
                      ? null
                      : GetPropertyInfo<TItem>(field).PropertyType;

    /// <summary>
    /// Get default value from type.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="isCollation"></param>
    /// <returns></returns>
    public static object GetDefaultValue(Type type, bool isCollation)
    {
        var realType = IsNullable(type)
                            ? type.GetGenericArguments()[0]
                            : type;

        object value;
        if (isCollation)
        {
            value = Activator.CreateInstance(typeof(List<>).MakeGenericType(realType))!;
        }
        else if (IsEnum(type))
        {
            value = Enum.GetValues(realType).GetValue(0)!;
        }
        else if (IsString(type))
        {
            value = string.Empty;
        }
        else
        {
            value = Activator.CreateInstance(realType)!;
        }

        return value;
    }
}
