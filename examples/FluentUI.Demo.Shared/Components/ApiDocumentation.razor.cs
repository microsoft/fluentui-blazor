using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentUI.Demo.Generators;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components;

/// <summary />
public partial class ApiDocumentation
{
    private class MemberDescription
    {
        public MemberTypes MemberType { get; set; } = MemberTypes.Property;
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string[] EnumValues { get; set; } = Array.Empty<string>();
        public string[] Parameters { get; set; } = Array.Empty<string>();
        public string? Default { get; set; } = null;
        public string Description { get; set; } = "";
    }

    private IEnumerable<MemberDescription>? _allMembers = null;
    private string? _displayName, _id;

    /// <summary>
    /// The Component for which the Parameters, Methods and Events should be displayed
    /// </summary>

    [Parameter, EditorRequired]
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public Type Component { get; set; } = default!;

    /// <summary>
    /// It Component is a generic type, a generic type argument needs to be provided
    /// so an instance of the type can be created. 
    /// This is needed to get and display any default values
    /// Default for this parameter is 'typeof(string)'
    /// </summary>
    [Parameter]
    public Type InstanceType { get; set; } = typeof(string);

    /// <summary>
    /// The label used for displaying the type parameter
    /// </summary>
    [Parameter]
    public string? GenericLabel { get; set; } = null;

    protected override void OnParametersSet()
    {
        _displayName = Component.Name.Replace("`1", $"<{GenericLabel}>") + " Class";
        _id = _displayName.Replace(' ', '-').ToLowerInvariant();
    }

    private IEnumerable<MemberDescription> Properties => GetMembers(MemberTypes.Property);

    private IEnumerable<MemberDescription> Events => GetMembers(MemberTypes.Event);

    private IEnumerable<MemberDescription> Methods => GetMembers(MemberTypes.Method);

    [SuppressMessage("Trimming", "IL2055:Either the type on which the MakeGenericType is called can't be statically determined, or the type parameters to be used for generic arguments can't be statically determined.", Justification = "Just for demo/documentation purposes")]
    private IEnumerable<MemberDescription> GetMembers(MemberTypes type)
    {
        string[] MEMBERS_TO_EXCLUDE = new[] { "AdditionalAttributes", "BackReference", "Class", "Style", "Equals", "GetHashCode", "GetType", "SetParametersAsync", "ToString", "Dispose" };

        if (_allMembers == null)
        {
            List<MemberDescription>? members = new();

            object? obj;
            if (Component.IsGenericType)
            {
                if (InstanceType is null)
                    throw new ArgumentNullException(nameof(InstanceType), "InstanceType must be specified when Component is a generic type");

                // Supply the type to create the generic instance with (needs to be an array)
                Type[] typeArgs = { InstanceType };
                Type constructed = Component.MakeGenericType(typeArgs);

                obj = Activator.CreateInstance(constructed);
            }
            else
                obj = Activator.CreateInstance(Component);


            IEnumerable<MemberInfo>? allProperties = Component.GetProperties(BindingFlags.Public).Select(i => (MemberInfo)i);
            IEnumerable<MemberInfo>? allMethods = Component.GetMethods().Where(i => !i.IsSpecialName).Select(i => (MemberInfo)i);

            foreach (var memberInfo in allProperties.Union(allMethods).OrderBy(m => m.Name))
            {
                if (!MEMBERS_TO_EXCLUDE.Contains(memberInfo.Name) || Component.Name == "FluentComponentBase")
                {
                    PropertyInfo? propertyInfo = memberInfo as PropertyInfo;
                    MethodInfo? methodInfo = memberInfo as MethodInfo;

                    if (propertyInfo != null)
                    {
                        bool isParameter = memberInfo.GetCustomAttribute<ParameterAttribute>() != null;
                       

                        Type t = memberInfo.GetType(); 
                        bool isEvent = t == typeof(EventCallback) || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(EventCallback<>));

                        // Parameters
                        if (isParameter && !isEvent)
                        {
                            members.Add(new MemberDescription()
                            {
                                MemberType = MemberTypes.Property,
                                Name = propertyInfo.Name,
                                Type = propertyInfo.ToTypeNameString(),
                                EnumValues = GetEnumValues(propertyInfo),
                                Default = propertyInfo.PropertyType.IsValueType ? obj?.GetType().GetProperty(propertyInfo.Name)?.GetValue(obj)?.ToString() : "",
                                Description = CodeComments.GetSummary(Component.Name + "." + propertyInfo.Name) ?? CodeComments.GetSummary(Component.BaseType?.Name + "." + propertyInfo.Name)
                            }); ;
                        }

                        // Events
                        if (isEvent)
                        {
                            string eventTypes = string.Join(", ", propertyInfo.PropertyType.GenericTypeArguments.Select(i => i.Name));
                            members.Add(new MemberDescription()
                            {
                                MemberType = MemberTypes.Event,
                                Name = propertyInfo.Name,
                                Type = propertyInfo.ToTypeNameString(),
                                Description = CodeComments.GetSummary(Component.Name + "." + propertyInfo.Name) ?? CodeComments.GetSummary(Component.BaseType?.Name + "." + propertyInfo.Name)
                            });
                        }
                    }

                    // Methods
                    if (methodInfo != null)
                    {
                        members.Add(new MemberDescription()
                        {
                            MemberType = MemberTypes.Method,
                            Name = methodInfo.Name,
                            Parameters = methodInfo.GetParameters().Select(i => $"{i.ToTypeNameString()} {i.Name}").ToArray(),
                            Type = methodInfo.ToTypeNameString(),
                            Description = CodeComments.GetSummary(Component.Name + "." + methodInfo.Name) ?? CodeComments.GetSummary(Component.BaseType?.Name + "." + methodInfo.Name)
                        });
                    }
                }
            }

            _allMembers = members.OrderBy(i => i.Name).ToArray();
        }

        return _allMembers.Where(i => i.MemberType == type);
    }

    private static string[] GetEnumValues(PropertyInfo? propertyInfo)
    {
        if (propertyInfo != null)
        {
            if (propertyInfo.PropertyType.IsEnum)
            {
                return propertyInfo.PropertyType.GetEnumNames();
            }
            else if (propertyInfo.PropertyType.GenericTypeArguments?.Length > 0 &&
                     propertyInfo.PropertyType.GenericTypeArguments[0].IsEnum)
            {
                return propertyInfo.PropertyType.GenericTypeArguments[0].GetEnumNames();
            }
        }

        return Array.Empty<string>();
    }
}
