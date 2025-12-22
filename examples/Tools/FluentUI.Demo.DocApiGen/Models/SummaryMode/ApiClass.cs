// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentUI.Demo.DocApiGen.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.DocApiGen.Models.SummaryMode;

/// <summary>
/// Represents a class with properties, methods, and events.
/// </summary>
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class ApiClass
{
    private static readonly string[] MEMBERS_TO_EXCLUDE = Constants.MEMBERS_TO_EXCLUDE;
    private readonly Type _component;
    private IEnumerable<ApiMember>? _allMembers;
    private readonly ApiClassOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClass"/> class.
    /// </summary>
    /// <param name="component"></param>
    /// <param name="options"></param>
    public ApiClass(Type component, ApiClassOptions options)
    {
        _component = component;
        _options = options;
    }

    /// <summary>
    /// It Component is a generic type, a generic type argument needs to be provided
    /// so an instance of the type can be created. 
    /// This is needed to get and display any default values
    /// Default for this parameter is 'typeof(string)'
    /// </summary>
    public Type[] InstanceTypes
    {
        get
        {
            return _component.GetType() switch
            {
                //Type t when t == typeof(int) => new[] { typeof(int) },
                //Type t when t == typeof(bool) => new[] { typeof(bool) },
                _ => [typeof(string)]
            };
        }
    }

    /// <summary>
    /// Gets the name of the specified component.
    /// </summary>
    public string Name => _component.Name;

    /// <summary>
    /// Gets the class summary for the specified component.
    /// </summary>
    public string Summary => GetSummary(_component, null);

    /// <summary>
    /// Gets the list of properties for the specified component.
    /// </summary>
    public IEnumerable<ApiMember> Properties
    {
        get
        {
            var properties = GetMembers(MemberTypes.Property);
            
            // For enums, include all values regardless of PropertyParameterOnly setting
            if (_component.IsEnum)
            {
                return properties;
            }
            
            // For classes, apply PropertyParameterOnly filter
            return properties.Where(i => _options.PropertyParameterOnly == false || i.IsParameter);
        }
    }

    /// <summary>
    /// Gets the list of Events for the specified component.
    /// </summary>

    public IEnumerable<ApiMember> Events => GetMembers(MemberTypes.Event);

    /// <summary>
    /// Gets the list of Methods for the specified component.
    /// </summary>

    public IEnumerable<ApiMember> Methods => GetMembers(MemberTypes.Method);

    /// <summary>
    /// Returns a dictionary of the properties, methods, and events.
    /// </summary>
    /// <returns></returns>
    public IDictionary<string, string> ToDictionary()
    {
        var result = new Dictionary<string, string>();
        var members = Properties.Union(Methods).Union(Events).OrderBy(i => i.Name);

        foreach (var member in members)
        {
            result.Add(member.GetSignature(), member.Description);
        }

        return result;
    }

    /// <summary />
    private IEnumerable<ApiMember> GetMembers(MemberTypes type)
    {
        if (_allMembers == null)
        {
            List<ApiMember>? members = [];
            object? obj = null;
            var created = false;
            var canCreateInstance = CanCreateInstance(_component);

            var ctorArguments = HasCtorWithArguments(_component, ["LibraryConfiguration"])
                  ? new object?[] { null }
                  : Array.Empty<object>();

            // Create an instance of the component to get the default values
            object? GetObjectValue(string propertyName)
            {
                // Skip instance creation if we know it will fail
                if (!canCreateInstance || _component.IsAbstract || _component.IsInterface)
                {
                    return null;
                }

                try
                {
                    if (!created)
                    {
                        if (_component.IsGenericType)
                        {
                            if (InstanceTypes is null)
                            {
                                return null;
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
                catch (Exception)
                {
                    // Mark as unable to create to avoid future attempts
                    canCreateInstance = false;
                    return null;
                }
            }

            var allEnums = _component.IsEnum ? _component.GetFields(BindingFlags.Public | BindingFlags.Static).Select(i => (MemberInfo)i) : [];
            var allProperties = _component.GetProperties().Select(i => (MemberInfo)i);
            var allMethods = _component.GetMethods().Where(i => !i.IsSpecialName).Select(i => (MemberInfo)i);

            foreach (var memberInfo in allProperties.Union(allMethods).Union(allEnums).OrderBy(m => m.Name))
            {
                try
                {
                    if (!MEMBERS_TO_EXCLUDE.Contains(memberInfo.Name) || _component.Name == "FluentComponentBase")
                    {
                        var enumInfo = memberInfo as FieldInfo;
                        var propertyInfo = memberInfo as PropertyInfo;
                        var methodInfo = memberInfo as MethodInfo;

                        var isObsolete = memberInfo.GetCustomAttribute<ObsoleteAttribute>() != null;
                        if (isObsolete)
                        {
                            continue;
                        }

                        if (enumInfo != null && enumInfo.FieldType.IsEnum)
                        {
                            members.Add(new ApiMember()
                            {
                                MemberInfo = memberInfo,
                                MemberType = MemberTypes.Property,
                                Name = enumInfo.Name,
                                Type = "",
                                EnumValues = [],
                                Default = "",
                                Description = GetSummary(_component, enumInfo),
                                IsParameter = false,
                            });
                        }

                        if (!_component.IsEnum && propertyInfo != null)
                        {
                            var isParameter = memberInfo.GetCustomAttribute<ParameterAttribute>() != null;

                            var t = propertyInfo.PropertyType;
                            var isEvent = t == typeof(EventCallback) || t.IsGenericType && t.GetGenericTypeDefinition() == typeof(EventCallback<>);

                            // Parameters/properties
                            if (!isEvent)
                            {
                                // Only try to get default value if we can create an instance and the property type is simple
                                var defaultValue = "";
                                if (canCreateInstance && (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string)))
                                {
                                    defaultValue = GetObjectValue(propertyInfo.Name)?.ToString();
                                }

                                members.Add(new ApiMember()
                                {
                                    MemberInfo = memberInfo,
                                    MemberType = MemberTypes.Property,
                                    Name = propertyInfo.Name,
                                    Type = propertyInfo.ToTypeNameString(),
                                    EnumValues = propertyInfo.GetEnumValues(),
                                    Default = defaultValue,
                                    Description = GetSummary(_component, propertyInfo),
                                    IsParameter = isParameter,
                                });
                            }

                            // Events
                            if (isEvent)
                            {
                                var eventTypes = string.Join(", ", propertyInfo.PropertyType.GenericTypeArguments.Select(i => i.Name));
                                members.Add(new ApiMember()
                                {
                                    MemberInfo = memberInfo,
                                    MemberType = MemberTypes.Event,
                                    Name = propertyInfo.Name,
                                    Type = propertyInfo.ToTypeNameString(),
                                    Description = GetSummary(_component, propertyInfo),
                                });
                            }
                        }

                        // Methods
                        if (!_component.IsEnum && methodInfo != null)
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
                                MemberInfo = memberInfo,
                                MemberType = MemberTypes.Method,
                                Name = methodInfo.Name + genericArguments,
                                Parameters = methodInfo.GetParameters().Select(i => $"{i.ToTypeNameString()} {i.Name}").ToArray(),
                                Type = methodInfo.ToTypeNameString(),
                                Description = GetSummary(_component, methodInfo),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ApiDocumentation] ERROR: Cannot process {_component.FullName} -> {memberInfo.Name}: {ex.Message}");
                    // Don't rethrow, continue with next member
                }
            }

            _allMembers = [.. members.OrderBy(i => i.Name)];
        }

        return _allMembers.Where(i => i.MemberType == type);
    }

    /// <summary>
    /// Checks if a type can be instantiated.
    /// </summary>
    private static bool CanCreateInstance(Type type)
    {
        if (type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition)
        {
            return false;
        }

        // Check if type has a parameterless constructor or a constructor with nullable LibraryConfiguration
        var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        if (constructors.Length == 0)
        {
            return false;
        }

        foreach (var ctor in constructors)
        {
            var parameters = ctor.GetParameters();
            
            // Parameterless constructor
            if (parameters.Length == 0)
            {
                return true;
            }
            
            // Constructor with single nullable LibraryConfiguration parameter
            if (parameters.Length == 1 && 
                parameters[0].ParameterType.Name == "LibraryConfiguration" &&
                parameters[0].IsOptional == false)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary />
    private string GetSummary(Type component, MemberInfo? member)
    {
        return member == null
             ? _options.DocXmlReader.GetComponentSummary(component)
             : _options.DocXmlReader.GetMemberSummary(member);
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
