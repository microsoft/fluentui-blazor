using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentUI.Demo.Shared.Components.ApiComplex;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components;

public partial class ApiComplexClass
{
    [Parameter]
    [EditorRequired]
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public Type Item { get; set; } = default!;

    public IEnumerable<PropertyInfo>? Properties { get; set; } = null;

    protected override void OnInitialized()
    {
        var properties = new List<PropertyParentChild>();
        ClassPropertyFactory.HierarchicalProperties(properties, Item);
    }

    //private void GetProperties(List<PropertyLevel> properties, Type type, int level, string path)
    //{
    //    var items = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
    //                    .Where(m => m.MemberType == MemberTypes.Property)
    //                    .OrderBy(m => m.Name)
    //                    .Select(i => (PropertyInfo)i)
    //                    .ToArray();

    //    var parent = properties.FirstOrDefault(i => i.Property.PropertyType == type);

    //    foreach (var item in items)
    //    {
    //        properties.Add(new PropertyLevel($"{path}.{item.Name}", level + 1, parent, item));
    //        if (!IsSimpleType(item.PropertyType))
    //        {
    //            GetProperties(properties, item.PropertyType, level + 1, $"{path}.{item.Name}");
    //        }
    //    }
    //}

    //internal static bool IsSimpleType(Type type) => type.IsPrimitive ||
    //                                        type == typeof(string) ||
    //                                        type == typeof(decimal) ||
    //                                        type.IsNullable() && 
    //                                            (
    //                                             Nullable.GetUnderlyingType(type)?.IsPrimitive == true ||
    //                                             Nullable.GetUnderlyingType(type) == typeof(string) ||
    //                                             Nullable.GetUnderlyingType(type) == typeof(decimal)
    //                                            );

    //public class PropertyLevel
    //{
    //    public PropertyLevel(string key, int level, PropertyLevel? parent, PropertyInfo property)
    //    {
    //        Key = key;
    //        Level = level;
    //        Parent = parent;
    //        Property = property;
    //    }

    //    public string Key { get; }
    //    public int Level { get; }
    //    public PropertyLevel? Parent { get; }
    //    public PropertyInfo Property { get; set; }
    //    public PropertySummary Summary => new PropertySummary(this);
    //}

    //public class PropertySummary
    //{
    //    private PropertyLevel _property;
    //    public PropertySummary(PropertyLevel property)
    //    {
    //        _property = property;
    //    }
    //    public string FullName => _property.Key;
    //    public int Level => _property.Key.Count(i => i == '.');
    //    public string Name => _property.Key.Split(".").Last();
    //    public string Summary
    //    {
    //        get
    //        {
    //            var property = _property.Property;
    //            var ns = property.ReflectedType?.Namespace ?? string.Empty;
    //            var prefix = property.ReflectedType?.FullName?.Substring(ns.Length + 1).Replace("+", ".");
    //            var commentKey = $"{prefix}.{property.Name}";
    //            return CodeComments.GetSummary(commentKey);
    //        }
    //    }
    //    // bool IsEditable => IsSimpleType(Property.Property.PropertyType);
    //}
}
