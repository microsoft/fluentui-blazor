// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentUI.Demo.DocApiGen.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.DocApiGen.Models;

/// <summary>
/// Represents a class with properties, methods, and events.
/// </summary>
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class ApiClass
{
    private static readonly string[] MEMBERS_TO_EXCLUDE =
    [
        "AdditionalAttributes",
        "ParentReference",
        "Element",
        "Equals",
        "GetHashCode",
        "GetType",
        "SetParametersAsync",
        "ToString",
        "Dispose",
        "DisposeAsync",
        "ValueExpression",
    ];

    private readonly Type _component;
    private IEnumerable<ApiMember>? _allMembers;
    private readonly bool _allProperties;

    /// <summary>
    /// Gets the <see cref="ApiClass"/> for the specified component.
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="typeName"></param>
    /// <param name="allProperties">False to returns only [Parameter] properties</param>
    /// <returns></returns>
    public static ApiClass? FromTypeName(Assembly assembly, string? typeName, bool allProperties = false)
    {
        var type = assembly.GetTypes().FirstOrDefault(i => i.Name == typeName || i.Name.StartsWith($"{typeName}`1"));
        return type is null ? null : new ApiClass(type, allProperties);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClass"/> class.
    /// </summary>
    /// <param name="component"></param>
    /// <param name="allProperties">False to returns only [Parameter] properties</param>
    public ApiClass(Type component, bool allProperties = false)
    {
        _component = component;
        _allProperties = allProperties;
    }

    /// <summary>
    /// It Component is a generic type, a generic type argument needs to be provided
    /// so an instance of the type can be created. 
    /// This is needed to get and display any default values
    /// Default for this parameter is 'typeof(string)'
    /// </summary>
    public Type[] InstanceTypes { get; set; } = [typeof(string)];

    /// <summary>
    /// Gets the class summary for the specified component.
    /// </summary>
    public string Summary => GetSummary(_component, null);

    /// <summary>
    /// Gets the list of properties for the specified component.
    /// </summary>
    public IEnumerable<ApiMember> Properties => GetMembers(MemberTypes.Property).Where(i => _allProperties ? true : i.IsParameter);

    /// <summary>
    /// Gets the list of Events for the specified component.
    /// </summary>

    public IEnumerable<ApiMember> Events => GetMembers(MemberTypes.Event);

    /// <summary>
    /// Gets the list of Methods for the specified component.
    /// </summary>

    public IEnumerable<ApiMember> Methods => GetMembers(MemberTypes.Method);

    /// <summary />
    private IEnumerable<ApiMember> GetMembers(MemberTypes type)
    {
        if (_allMembers == null)
        {
            List<ApiMember>? members = [];

            object? obj = null;
            var created = false;

            object? GetObjectValue(string propertyName)
            {
                if (!created)
                {
                    if (_component.IsGenericType)
                    {
                        if (InstanceTypes is null)
                        {
                            throw new InvalidCastException("InstanceTypes must be specified when Component is a generic type");
                        }

                        // Supply the type to create the generic instance with (needs to be an array)
                        obj = Activator.CreateInstance(_component.MakeGenericType(InstanceTypes));
                    }
                    else
                    {
                        obj = Activator.CreateInstance(_component);
                    }

                    created = true;
                }

                return obj?.GetType().GetProperty(propertyName)?.GetValue(obj);
            }

            var allProperties = _component.GetProperties().Select(i => (MemberInfo)i);
            var allMethods = _component.GetMethods().Where(i => !i.IsSpecialName).Select(i => (MemberInfo)i);

            foreach (var memberInfo in allProperties.Union(allMethods).OrderBy(m => m.Name))
            {
                try
                {
                    if (!MEMBERS_TO_EXCLUDE.Contains(memberInfo.Name) || _component.Name == "FluentComponentBase")
                    {
                        var propertyInfo = memberInfo as PropertyInfo;
                        var methodInfo = memberInfo as MethodInfo;

                        var isObsolete = memberInfo.GetCustomAttribute<ObsoleteAttribute>() != null;
                        if (isObsolete)
                        {
                            continue;
                        }

                        if (propertyInfo != null)
                        {
                            var isParameter = memberInfo.GetCustomAttribute<ParameterAttribute>() != null;

                            var t = propertyInfo.PropertyType;
                            var isEvent = t == typeof(EventCallback) || t.IsGenericType && t.GetGenericTypeDefinition() == typeof(EventCallback<>);

                            // Parameters/properties
                            if (!isEvent)
                            {
                                // Icon? icon = null;
                                var defaultValue = "";
                                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string))
                                {
                                    defaultValue = GetObjectValue(propertyInfo.Name)?.ToString();
                                }
                                //else if (propertyInfo.PropertyType == typeof(Icon))
                                //{
                                //    if (GetObjectValue(propertyInfo.Name) is Icon value)
                                //    {
                                //        icon = value;
                                //        defaultValue = $"{value.Variant}.{value.Size}.{value.Name}";
                                //    }
                                //}

                                members.Add(new ApiMember()
                                {
                                    MemberType = MemberTypes.Property,
                                    Name = propertyInfo.Name,
                                    Type = propertyInfo.ToTypeNameString(),
                                    EnumValues = propertyInfo.GetEnumValues(),
                                    Default = defaultValue,
                                    Description = GetSummary(_component, propertyInfo),
                                    IsParameter = isParameter,
                                    //Icon = icon
                                });
                            }

                            // Events
                            if (isEvent)
                            {
                                var eventTypes = string.Join(", ", propertyInfo.PropertyType.GenericTypeArguments.Select(i => i.Name));
                                members.Add(new ApiMember()
                                {
                                    MemberType = MemberTypes.Event,
                                    Name = propertyInfo.Name,
                                    Type = propertyInfo.ToTypeNameString(),
                                    Description = GetSummary(_component, propertyInfo),
                                });
                            }
                        }

                        // Methods
                        if (methodInfo != null)
                        {
                            var isJSInvokable = memberInfo.GetCustomAttribute<JSInvokableAttribute>() != null;
                            if (isJSInvokable)
                            {
                                continue;
                            }

                            var genericArguments = "";
                            if (methodInfo.IsGenericMethod)
                            {
                                genericArguments = "<" + string.Join(", ", methodInfo.GetGenericArguments().Select(i => i.Name)) + ">";
                            }

                            members.Add(new ApiMember()
                            {
                                MemberType = MemberTypes.Method,
                                Name = methodInfo.Name + genericArguments,
                                Parameters = methodInfo.GetParameters().Select(i => $"{i.ToTypeNameString()} {i.Name}").ToArray(),
                                Type = methodInfo.ToTypeNameString(),
                                Description = GetSummary(_component, methodInfo),
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"[ApiDocumentation] ERROR: Cannot found {_component.FullName} -> {memberInfo.Name}");
                    throw;
                }
            }

            _allMembers = [.. members.OrderBy(i => i.Name)];
        }

        return _allMembers.Where(i => i.MemberType == type);
    }

    /// <summary />
    private string GetSummary(Type component, MemberInfo? member)
    {
        // TODO
        return $"{_component}.{component}.{member}";
    }
}
