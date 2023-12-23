using static FluentUI.Demo.Shared.Components.ApiComplexClass;
using System.Reflection;

namespace FluentUI.Demo.Shared.Components.ApiComplex;

internal static class ClassPropertyFactory
{
    public static void HierarchicalProperties(List<PropertyParentChild> properties, Type type)
    {
        var items = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .Where(m => m.MemberType == MemberTypes.Property)
                        .OrderBy(m => m.Name)
                        .Select(i => (PropertyInfo)i)
                        .ToArray();

        var parent = properties.FirstOrDefault(i => i.Value.PropertyType == type)?.Value;

        foreach (var item in items)
        {
            properties.Add(new PropertyParentChild(parent, item));
            if (!IsSimpleType(item.PropertyType))
            {
                HierarchicalProperties(properties, item.PropertyType);
            }
        }
    }

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
}
