// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Infrastructure;

/// <summary>
/// Service for managing and applying externalized default values for component parameters.
/// </summary>
public sealed class FluentDefaultValuesService
{
    private static readonly ConcurrentDictionary<string, Dictionary<string, object?>> _defaultValues = new();
    private static readonly ConcurrentDictionary<string, ComponentDefaultsInfo> _componentCache = new();
    private static readonly HashSet<string> _componentTypesWithoutDefaults = new();
    private static volatile bool _initialized = false;
    private static readonly object _lock = new();

    /// <summary>
    /// Cached information about a component's default-applicable properties.
    /// </summary>
    private sealed class ComponentDefaultsInfo
    {
        public required Dictionary<string, PropertyInfo> ParameterProperties { get; init; }
        public required Dictionary<string, object?> DefaultValues { get; init; }
    }

    /// <summary>
    /// Initializes the default values service by scanning for FluentDefault attributes.
    /// </summary>
    public static void Initialize()
    {
        if (_initialized)
            return;

        lock (_lock)
        {
            if (_initialized)
                return;

            ScanForDefaultValues();
            _initialized = true;
        }
    }

    /// <summary>
    /// Applies default values to the specified component if any matching defaults are found.
    /// Optimized to minimize reflection overhead.
    /// </summary>
    /// <param name="component">The component to apply defaults to.</param>
    public static void ApplyDefaults(ComponentBase component)
    {
        if (!_initialized)
            Initialize();

        var componentTypeName = component.GetType().Name;

        // Fast path: if we know this component type has no defaults, skip entirely
        if (_componentTypesWithoutDefaults.Contains(componentTypeName))
            return;

        // Check if we have cached info for this component type
        if (!_componentCache.TryGetValue(componentTypeName, out var componentInfo))
        {
            // Build cache for this component type
            componentInfo = BuildComponentCache(component.GetType(), componentTypeName);
            if (componentInfo == null)
            {
                // No defaults for this component type
                _componentTypesWithoutDefaults.Add(componentTypeName);
                return;
            }
        }

        // Apply defaults using cached reflection info
        foreach (var (propertyName, defaultValue) in componentInfo.DefaultValues)
        {
            if (!componentInfo.ParameterProperties.TryGetValue(propertyName, out var property))
                continue;

            // Only set default if the current value is null or the default value for the type
            var currentValue = property.GetValue(component);
            if (IsDefaultValue(currentValue, property.PropertyType))
            {
                try
                {
                    property.SetValue(component, defaultValue);
                }
                catch
                {
                    // Silently ignore type conversion errors
                }
            }
        }
    }

    private static ComponentDefaultsInfo? BuildComponentCache(Type componentType, string componentTypeName)
    {
        // Check if we have any defaults for this component type
        if (!_defaultValues.TryGetValue(componentTypeName, out var defaults) || defaults.Count == 0)
            return null;

        // Build cache of parameter properties for this component type
        var parameterProperties = new Dictionary<string, PropertyInfo>();
        var properties = componentType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var property in properties)
        {
            if (property.CanWrite && property.GetCustomAttributes<ParameterAttribute>().Any())
            {
                parameterProperties[property.Name] = property;
            }
        }

        var componentInfo = new ComponentDefaultsInfo
        {
            ParameterProperties = parameterProperties,
            DefaultValues = defaults
        };

        _componentCache.TryAdd(componentTypeName, componentInfo);
        return componentInfo;
    }

    private static void ScanForDefaultValues()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        foreach (var assembly in assemblies)
        {
            try
            {
                ScanAssemblyForDefaults(assembly);
            }
            catch
            {
                // Silently ignore assemblies that can't be scanned
            }
        }
    }

    private static void ScanAssemblyForDefaults(Assembly assembly)
    {
        var types = assembly.GetTypes().Where(t => t.IsClass && t.IsAbstract && t.IsSealed); // static classes

        foreach (var type in types)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Static);
            
            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<FluentDefaultAttribute>();
                if (attribute == null)
                    continue;

                var componentTypeName = attribute.ComponentTypeName;
                var propertyName = attribute.ParameterName ?? property.Name;
                var defaultValue = property.GetValue(null);

                _defaultValues.AddOrUpdate(
                    componentTypeName,
                    _ => new Dictionary<string, object?> { { propertyName, defaultValue } },
                    (_, existing) =>
                    {
                        existing[propertyName] = defaultValue;
                        return existing;
                    }
                );
            }
        }
    }

    private static bool IsDefaultValue(object? value, Type propertyType)
    {
        if (value == null)
            return true;

        // Handle nullable value types
        var underlyingType = Nullable.GetUnderlyingType(propertyType);
        if (underlyingType != null)
        {
            // For nullable types, null is considered the default
            return false; // If we got here, value is not null, so it's been explicitly set
        }

        if (!propertyType.IsValueType)
            return false; // Reference types (non-null) are considered explicitly set

        var defaultValue = Activator.CreateInstance(propertyType);
        return Equals(value, defaultValue);
    }

    /// <summary>
    /// Gets all registered default values for debugging purposes.
    /// </summary>
    internal static IReadOnlyDictionary<string, Dictionary<string, object?>> GetAllDefaults()
    {
        if (!_initialized)
            Initialize();

        return _defaultValues.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    /// <summary>
    /// Clears all cached default values. Primarily for testing purposes.
    /// </summary>
    internal static void Reset()
    {
        lock (_lock)
        {
            _defaultValues.Clear();
            _componentCache.Clear();
            _componentTypesWithoutDefaults.Clear();
            _initialized = false;
        }
    }
}