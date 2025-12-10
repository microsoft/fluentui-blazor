// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

/// <summary>
/// Factory for creating component information from types.
/// </summary>
internal sealed class ComponentInfoFactory
{
    private readonly XmlDocumentationReader _xmlReader;

    /// <summary>
    /// Members to exclude from the documentation.
    /// </summary>
    private static readonly HashSet<string> MembersToExclude =
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

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentInfoFactory"/> class.
    /// </summary>
    /// <param name="xmlReader">The XML documentation reader.</param>
    public ComponentInfoFactory(XmlDocumentationReader xmlReader)
    {
        _xmlReader = xmlReader;
    }

    /// <summary>
    /// Creates a ComponentInfo from a type.
    /// </summary>
    public ComponentInfo CreateComponentInfo(Type type)
    {
        var summary = _xmlReader.GetTypeSummary(type);
        var category = ComponentCategoryHelper.DetermineCategory(type);

        return new ComponentInfo
        {
            Name = type.Name,
            FullName = type.FullName ?? type.Name,
            Summary = summary,
            Category = category,
            IsGeneric = type.IsGenericType,
            BaseClass = type.BaseType?.Name
        };
    }

    /// <summary>
    /// Creates detailed component information.
    /// </summary>
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072", Justification = "Type is validated before use")]
    public ComponentDetails CreateComponentDetails(Type type, ComponentInfo componentInfo)
    {
        var properties = new List<Models.PropertyInfo>();
        var events = new List<Models.EventInfo>();
        var methods = new List<Models.MethodInfo>();

        ExtractProperties(type, properties, events);
        ExtractMethods(type, methods);

        return new ComponentDetails
        {
            Component = componentInfo,
            Parameters = properties.Where(p => p.IsParameter).OrderBy(p => p.Name).ToList(),
            Properties = properties.OrderBy(p => p.Name).ToList(),
            Events = events.OrderBy(e => e.Name).ToList(),
            Methods = methods.OrderBy(m => m.Name).ToList()
        };
    }

    /// <summary>
    /// Extracts properties and events from a type.
    /// </summary>
    private void ExtractProperties(Type type, List<Models.PropertyInfo> properties, List<Models.EventInfo> events)
    {
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (MembersToExclude.Contains(prop.Name))
            {
                continue;
            }

            var isObsolete = prop.GetCustomAttribute<ObsoleteAttribute>() != null;
            if (isObsolete)
            {
                continue;
            }

            var isParameter = prop.GetCustomAttribute<ParameterAttribute>() != null;
            var isEvent = TypeHelper.IsEventCallback(prop.PropertyType);

            if (isEvent)
            {
                events.Add(new Models.EventInfo
                {
                    Name = prop.Name,
                    Type = TypeHelper.GetTypeName(prop.PropertyType),
                    Description = _xmlReader.GetMemberSummary(prop),
                    IsInherited = IsInherited(prop, type)
                });
            }
            else
            {
                properties.Add(new Models.PropertyInfo
                {
                    Name = prop.Name,
                    Type = TypeHelper.GetTypeName(prop.PropertyType),
                    Description = _xmlReader.GetMemberSummary(prop),
                    IsParameter = isParameter,
                    IsInherited = IsInherited(prop, type),
                    DefaultValue = GetDefaultValue(type, prop),
                    EnumValues = TypeHelper.GetEnumValues(prop.PropertyType)
                });
            }
        }
    }

    /// <summary>
    /// Extracts methods from a type.
    /// </summary>
    private void ExtractMethods(Type type, List<Models.MethodInfo> methods)
    {
        foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            if (method.IsSpecialName || MembersToExclude.Contains(method.Name))
            {
                continue;
            }

            var isObsolete = method.GetCustomAttribute<ObsoleteAttribute>() != null;
            var isJSInvokable = method.GetCustomAttribute<JSInvokableAttribute>() != null;
            if (isObsolete || isJSInvokable)
            {
                continue;
            }

            var genericArgs = method.IsGenericMethod
                ? "<" + string.Join(", ", method.GetGenericArguments().Select(a => a.Name)) + ">"
                : "";

            methods.Add(new Models.MethodInfo
            {
                Name = method.Name + genericArgs,
                ReturnType = TypeHelper.GetTypeName(method.ReturnType),
                Description = _xmlReader.GetMemberSummary(method),
                Parameters = method.GetParameters().Select(p => $"{TypeHelper.GetTypeName(p.ParameterType)} {p.Name}").ToArray(),
                IsInherited = false
            });
        }
    }

    /// <summary>
    /// Checks if a property is inherited.
    /// </summary>
    private static bool IsInherited(System.Reflection.PropertyInfo property, Type declaringType)
    {
        return property.DeclaringType != declaringType;
    }

    /// <summary>
    /// Gets the default value for a property.
    /// </summary>
    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072", Justification = "Type is validated before use")]
    private static string? GetDefaultValue(Type componentType, System.Reflection.PropertyInfo property)
    {
        try
        {
            // Only get default values for value types and strings
            if (!property.PropertyType.IsValueType && property.PropertyType != typeof(string))
            {
                return null;
            }

            // Try to create an instance to get default values
            object? instance;
            if (componentType.IsGenericType)
            {
                var genericType = componentType.MakeGenericType(typeof(string));
                instance = Activator.CreateInstance(genericType);
            }
            else
            {
                instance = Activator.CreateInstance(componentType);
            }

            if (instance == null)
            {
                return null;
            }

            var value = property.GetValue(instance);
            return value?.ToString();
        }
        catch
        {
            return null;
        }
    }
}
