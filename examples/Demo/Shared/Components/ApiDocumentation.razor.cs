using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Components;

/// <summary />
public partial class ApiDocumentation
{
    private class MemberDescription
    {
        public MemberTypes MemberType { get; set; } = MemberTypes.Property;
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string[] EnumValues { get; set; } = [];
        public string[] Parameters { get; set; } = [];
        public string? Default { get; set; } = null;
        public string Description { get; set; } = "";
        public bool IsParameter { get; set; }
        public Icon? Icon { get; set; }
    }

    private IEnumerable<MemberDescription>? _allMembers = null;
    private string? _displayName, _id;

    /// <summary>
    /// Gets or sets the Component for which the Parameters, Methods and Events should be displayed.
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
    public Type[] InstanceTypes { get; set; } = [typeof(string)];

    /// <summary>
    /// Gets or sets the label used for displaying the type parameter.
    /// </summary>
    [Parameter]
    public string? GenericLabel { get; set; } = null;

    [Parameter]
    public RenderFragment? Description { get; set; }

    protected override void OnParametersSet()
    {
        _displayName = Component.Name.Replace("`1", $"<{GenericLabel}>").Replace("`2", $"<{GenericLabel}>") + " Class";
        _id = _displayName.Replace(' ', '-').ToLowerInvariant();
    }
    private IEnumerable<MemberDescription> Properties => GetMembers(MemberTypes.Property);

    private IEnumerable<MemberDescription> Events => GetMembers(MemberTypes.Event);

    private IEnumerable<MemberDescription> Methods => GetMembers(MemberTypes.Method);

    [SuppressMessage("Trimming", "IL2055:Either the type on which the MakeGenericType is called can't be statically determined, or the type parameters to be used for generic arguments can't be statically determined.", Justification = "Just for demo/documentation purposes")]
    private IEnumerable<MemberDescription> GetMembers(MemberTypes type)
    {
        var MEMBERS_TO_EXCLUDE = new[] { "Id", "AdditionalAttributes", "ParentReference", "Element", "Class", "Style", "Data", "Equals", "GetHashCode", "GetType", "SetParametersAsync", "ToString", "Dispose" };

        if (_allMembers == null)
        {
            List<MemberDescription>? members = [];

            object? obj = null;
            var created = false;

            object? GetObjectValue(string propertyName)
            {
                if (!created)
                {
                    if (Component.IsGenericType)
                    {
                        if (InstanceTypes is null)
                        {
                            throw new ArgumentNullException(nameof(InstanceTypes), "InstanceTypes must be specified when Component is a generic type");
                        }

                        // Supply the type to create the generic instance with (needs to be an array)
                        obj = Activator.CreateInstance(Component.MakeGenericType(InstanceTypes));
                    }
                    else
                    {
                        obj = Activator.CreateInstance(Component);
                    }
                    created = true;
                }

                return obj?.GetType().GetProperty(propertyName)?.GetValue(obj);
            };

            var allProperties = Component.GetProperties().Select(i => (MemberInfo)i);
            var allMethods = Component.GetMethods().Where(i => !i.IsSpecialName).Select(i => (MemberInfo)i);

            foreach (var memberInfo in allProperties.Union(allMethods).OrderBy(m => m.Name))
            {
                try
                {
                    if (!MEMBERS_TO_EXCLUDE.Contains(memberInfo.Name) || Component.Name == "FluentComponentBase")
                    {
                        var propertyInfo = memberInfo as PropertyInfo;
                        var methodInfo = memberInfo as MethodInfo;

                        if (propertyInfo != null)
                        {
                            var isParameter = memberInfo.GetCustomAttribute<ParameterAttribute>() != null;

                            var t = propertyInfo.PropertyType;
                            var isEvent = t == typeof(EventCallback) || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(EventCallback<>));

                            // Parameters/properties
                            if (!isEvent)
                            {
                                Icon? icon = null;
                                var defaultVaue = "";
                                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string))
                                {
                                    defaultVaue = GetObjectValue(propertyInfo.Name)?.ToString();
                                }
                                else if (propertyInfo.PropertyType == typeof(Icon))
                                {
                                    if (GetObjectValue(propertyInfo.Name) is Icon value)
                                    {
                                        icon = value;
                                        defaultVaue = $"{value.Variant}.{value.Size}.{value.Name}";
                                    }
                                }

                                members.Add(new MemberDescription()
                                {
                                    MemberType = MemberTypes.Property,
                                    Name = propertyInfo.Name,
                                    Type = propertyInfo.ToTypeNameString(),
                                    EnumValues = GetEnumValues(propertyInfo),
                                    Default = defaultVaue,
                                    Description = GetDescription(Component, propertyInfo),
                                    IsParameter = isParameter,
                                    Icon = icon
                                });
                            }

                            // Events
                            if (isEvent)
                            {
                                var eventTypes = string.Join(", ", propertyInfo.PropertyType.GenericTypeArguments.Select(i => i.Name));
                                members.Add(new MemberDescription()
                                {
                                    MemberType = MemberTypes.Event,
                                    Name = propertyInfo.Name,
                                    Type = propertyInfo.ToTypeNameString(),
                                    Description = GetDescription(Component, propertyInfo)
                                });
                            }
                        }

                        // Methods
                        if (methodInfo != null)
                        {
                            var genericArguments = "";
                            if (methodInfo.IsGenericMethod)
                            {
                                genericArguments = "<" + string.Join(", ", methodInfo.GetGenericArguments().Select(i => i.Name)) + ">";
                            }

                            members.Add(new MemberDescription()
                            {
                                MemberType = MemberTypes.Method,
                                Name = methodInfo.Name + genericArguments,
                                Parameters = methodInfo.GetParameters().Select(i => $"{i.ToTypeNameString()} {i.Name}").ToArray(),
                                Type = methodInfo.ToTypeNameString(),
                                Description = GetDescription(Component, methodInfo)
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"[ApiDocumentation] ERROR: Cannot found {Component.FullName} -> {memberInfo.Name}");
                    throw;
                }
            }

            _allMembers = members.OrderBy(i => i.Name).ToArray();
        }

        return _allMembers.Where(i => i.MemberType == type);
    }

    /// <summary>
    /// Gets member description for generic MemberInfo.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="component"></param>
    /// <param name="memberInfo"></param>
    /// <returns>member description</returns>
    private static string GetDescription<T>(Type component, T memberInfo) where T : MemberInfo
    {
        return DescriptionFromCodeComments(component, memberInfo.Name);
    }

    /// <summary>
    /// Gets description
    /// </summary>
    /// <param name="component"></param>
    /// <param name="methodInfo"></param>
    /// <returns></returns>
    /// <remarks>
    /// see the following about name mangling when dealing with generics
    /// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/#id-strings
    /// </remarks>
    private static string GetDescription(Type component, MethodInfo methodInfo)
    {
        var genericArgumentCount = methodInfo.GetGenericArguments().Length;
        var mangledName = methodInfo.Name + (genericArgumentCount == 0 ? "" : $"``{genericArgumentCount}");

        var description = DescriptionFromCodeComments(component, mangledName);

        return description;
    }

    /// <summary>
    /// Gets member description from source generated class of component
    /// descriptions. If none found, component base member description
    /// is returned.
    /// </summary>
    /// <param name="component"></param>
    /// <param name="name">name of property, method, or event</param>
    /// <returns></returns>
    private static string DescriptionFromCodeComments(Type component, string name)
    {
        var description = CodeComments.GetSummary(component.Name + "." + name);

        if (description == null && component.BaseType != null)
        {
            description = DescriptionFromCodeComments(component.BaseType, name);
        }

        return description ?? string.Empty;
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

        return [];
    }
}
