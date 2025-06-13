// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Extensions;
using FluentUI.Demo.DocViewer.Services;
using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace FluentUI.Demo.DocViewer.Models;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
internal class ApiClass
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
    private readonly DocViewerService _docViewerService;
    private IEnumerable<ApiClassMember>? _allMembers;
    private readonly bool _allProperties;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClass"/> class.
    /// </summary>
    /// <param name="docViewerService"></param>
    /// <param name="component"></param>
    /// <param name="allProperties">False to returns only [Parameter] properties</param>
    public ApiClass(DocViewerService docViewerService, Type component, bool allProperties = false)
    {
        _docViewerService = docViewerService;
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
    public IEnumerable<ApiClassMember> Properties => GetMembers(MemberTypes.Property).Where(i => _allProperties ? true : i.IsParameter);

    /// <summary>
    /// Gets the list of Events for the specified component.
    /// </summary>

    public IEnumerable<ApiClassMember> Events => GetMembers(MemberTypes.Event);

    /// <summary>
    /// Gets the list of Methods for the specified component.
    /// </summary>

    public IEnumerable<ApiClassMember> Methods => GetMembers(MemberTypes.Method);

    /// <summary />
    private IEnumerable<ApiClassMember> GetMembers(MemberTypes type)
    {
        if (_allMembers == null)
        {
            List<ApiClassMember>? members = [];

            object? obj = null;
            var created = false;

            var ctorArguments = HasCtorWithArguments(_component, ["LibraryConfiguration"])
                              ? new object?[] { null }
                              : Array.Empty<object>();

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
                        obj = Activator.CreateInstance(_component.MakeGenericType(InstanceTypes), args: ctorArguments);
                    }
                    else
                    {
                        obj = Activator.CreateInstance(_component, args: ctorArguments);
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
                                var defaultValue = "";
                                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string))
                                {
                                    defaultValue = GetObjectValue(propertyInfo.Name)?.ToString();
                                }
                                
                                members.Add(new ApiClassMember()
                                {
                                    MemberType = MemberTypes.Property,
                                    Name = propertyInfo.Name,
                                    Type = propertyInfo.ToTypeNameString(),
                                    EnumValues = propertyInfo.GetEnumValues(),
                                    Default = defaultValue,
                                    Description = GetSummary(_component, propertyInfo),
                                    IsParameter = isParameter,
                                    IsInherited = propertyInfo.IsInherited(_component),
                                });
                            }

                            // Events
                            if (isEvent)
                            {
                                var eventTypes = string.Join(", ", propertyInfo.PropertyType.GenericTypeArguments.Select(i => i.Name));
                                members.Add(new ApiClassMember()
                                {
                                    MemberType = MemberTypes.Event,
                                    Name = propertyInfo.Name,
                                    Type = propertyInfo.ToTypeNameString(),
                                    Description = GetSummary(_component, propertyInfo),
                                    IsInherited = propertyInfo.IsInherited(_component),
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

                            members.Add(new ApiClassMember()
                            {
                                MemberType = MemberTypes.Method,
                                Name = methodInfo.Name + genericArguments,
                                Parameters = methodInfo.GetParameters().Select(i => $"{i.ToTypeNameString()} {i.Name}").ToArray(),
                                Type = methodInfo.ToTypeNameString(),
                                Description = GetSummary(_component, methodInfo),
                                IsInherited = methodInfo.IsInherited(_component),
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
        var summary = _docViewerService.ApiCommentSummary(ApiDocSummary.Cached, component, member);

        if (string.IsNullOrWhiteSpace(summary))
        {
            return string.Empty;
        }

        return Markdown.ToHtml(summary, DocViewerService.MarkdownPipeline);
    }

    /// <summary>
    /// Determines whether the specified type has a constructor that matches the given argument types.
    /// </summary>
    public static bool HasCtorWithArguments(Type component, string[] arguments)
    {
        var constructors = component.GetConstructors();
        foreach (var ctor in constructors)
        {
            var parameters = ctor.GetParameters();
            var ctorArguments = parameters.Select(p => p.ParameterType.Name).ToArray();

            if (ctorArguments.Length == arguments.Length)
            {
                for (var i = 0; i < ctorArguments.Length; i++)
                {
                    if (ctorArguments[i] != arguments[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        return false;
    }
}
