using System.Diagnostics;
using System.Reflection;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components;

public static class PropertyInfoExtensions
{
    public static IEnumerable<PropertyChildren> GetPropertyChildren(this Type type)
    {
        return type.GetSubProperties()
                   .Select(i => new PropertyChildren(null, i, 0))
                   .ToArray();
    }

    public static IEnumerable<PropertyInfo> GetSubProperties(this PropertyInfo property)
    {
        return property.PropertyType.GetSubProperties();
    }

    public static IEnumerable<PropertyInfo> GetSubProperties(this Type type)
    {
        var items = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .Where(m => m.MemberType == MemberTypes.Property)
                        .OrderBy(m => m.Name)
                        .Select(i => (PropertyInfo)i)
                        .ToArray();

        return items;
    }

    /// <summary>
    /// Return True if the specified property type is a Simple Type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsSimpleType(this PropertyInfo property)
    {
        return property.PropertyType.IsSimpleType();
    }

    /// <summary>
    /// Return True if the specified type is a Simple Type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsSimpleType(this Type type)
    {
        return type.IsPrimitive ||
               type == typeof(string) ||
               type == typeof(decimal) ||
               type.IsNullable() &&
                   (
                       Nullable.GetUnderlyingType(type)?.IsPrimitive == true ||
                       Nullable.GetUnderlyingType(type) == typeof(string) ||
                       Nullable.GetUnderlyingType(type) == typeof(decimal)
                   );
    }

    /// <summary>
    /// Returns the property value, for the nested property (MyItem.MyProperty.MyValue)
    /// </summary>
    /// <param name="src"></param>
    /// <param name="propName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static object? GetPropertyValue(object? src, string propName)
    {
        if (string.IsNullOrEmpty(propName))
        {
            throw new ArgumentException("Value cannot be null.", nameof(propName));
        }

        if (src == null)
        {
            return null;
        }

        if (propName.Contains(".")) //complex type nested
        {
            var temp = propName.Split(new char[] { '.' }, 2);
            return GetPropertyValue(GetPropertyValue(src, temp[0]), temp[1]);
        }
        else
        {
            var prop = src.GetType().GetProperty(propName);
            return prop != null ? prop.GetValue(src, null) : null;
        }
    }

    /// <summary>
    /// Updates the property value, for the nested property (MyItem.MyProperty.MyValue)
    /// </summary>
    /// <param name="src"></param>
    /// <param name="propName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static void SetPropertyValue(object? src, string propName, object? value)
    {
        if (string.IsNullOrEmpty(propName))
        {
            throw new ArgumentException("Value cannot be null.", nameof(propName));
        }

        if (propName.Contains(".")) //complex type nested
        {
            var temp = propName.Split(new char[] { '.' }, 2);
            SetPropertyValue(GetPropertyValue(src, temp[0]), temp[1], value);
        }
        else
        {
            var prop = src?.GetType().GetProperty(propName);
            prop?.SetValue(src, value);
        }
    }
}

[DebuggerDisplay("{Item.Name}")]
public class PropertyChildren
{
    public PropertyChildren(PropertyChildren? parent, PropertyInfo item, int level)
    {
        Parent = parent;
        Id = Identifier.NewId();
        Level = level;
        Item = item;
        Children = item.IsSimpleType()
                 ? null
                 : item.GetSubProperties()
                       .Select(i => new PropertyChildren(this, i, level + 1))
                       .ToArray();
    }

    public PropertyChildren? Parent { get; }

    public string Id { get; }

    public int Level { get; }

    public string Summary => GetSummary();

    public PropertyInfo Item { get; }

    public IEnumerable<PropertyChildren>? Children { get; }

    public string FullName => GetFullName(this);

    public TokenType TokenType => Item.GetCustomAttribute<ThemeTokenTypeAttribute>()?.Value ?? TokenType.Text;

    private string GetSummary()
    {
        var property = Item;
        var ns = property.ReflectedType?.Namespace ?? string.Empty;
        var prefix = property.ReflectedType?.FullName?.Substring(ns.Length + 1).Replace("+", ".");
        var commentKey = $"{prefix}.{property.Name}";
        return CodeComments.GetSummary(commentKey);
    }

    private string GetFullName(PropertyChildren property)
    {
        return property.Parent == null
             ? property.Item.Name
             : $"{GetFullName(property.Parent)}.{property.Item.Name}";
    }
}