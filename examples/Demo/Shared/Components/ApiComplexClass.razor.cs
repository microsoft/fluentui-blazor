using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components;

public partial class ApiComplexClass
{
    [Parameter]
    [EditorRequired]
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public Type Item { get; set; } = default!;

    public IDictionary<string, MemberInfo>? Members { get; set; } = null;

    protected override void OnInitialized()
    {
        var members = new Dictionary<string, MemberInfo>();
        GetMembers(ref members, Item, Item.Name);

        Members = members;
    }

    private void GetMembers(ref Dictionary<string, MemberInfo> members, Type type, string path)
    {
        var items = type.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                        .Where(m => m.MemberType == MemberTypes.Property)
                        .OrderBy(m => m.Name)
                        .Select(i => (PropertyInfo)i)
                        .ToArray();

        foreach (var item in items)
        {
            members.Add($"{path}.{item.Name}", item);
            if (!IsSimpleType(item.PropertyType))
            {
                GetMembers(ref members, item.PropertyType, $"{path}.{item.Name}");
            }
        }
    }

    private PropertySummary GetPropertySummary(KeyValuePair<string, MemberInfo> value)
    {
        // Summary documentation
        var property = (PropertyInfo)value.Value;
        var ns = property.ReflectedType?.Namespace ?? string.Empty;
        var prefix = property.ReflectedType?.FullName?.Substring(ns.Length + 1).Replace("+", ".");
        var commentKey = $"{prefix}.{property.Name}";
        var summary = CodeComments.GetSummary(commentKey);

        // Editable if the last leaf
        bool isEditable = IsSimpleType(property.PropertyType); 

        // Returns
        return new PropertySummary(
            value.Key,
            value.Key.Count(i => i == '.'),
            value.Key.Split(".").Last(),
            summary,
            isEditable
            );
    }

    private bool IsSimpleType(Type type) => type.IsPrimitive ||
                                            type == typeof(string) ||
                                            type == typeof(decimal) ||
                                            type.IsNullable() && 
                                                (
                                                 Nullable.GetUnderlyingType(type)?.IsPrimitive == true ||
                                                 Nullable.GetUnderlyingType(type) == typeof(string) ||
                                                 Nullable.GetUnderlyingType(type) == typeof(decimal)
                                                );


    record PropertySummary(string FullName, int Level, string Name, string Summary, bool IsEditable);
}
