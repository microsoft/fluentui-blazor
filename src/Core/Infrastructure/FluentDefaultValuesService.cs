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
    private static volatile bool _initialized = false;
    private static readonly object _lock = new();

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
    /// </summary>
    /// <param name="component">The component to apply defaults to.</param>
    public static void ApplyDefaults(ComponentBase component)
    {
        if (!_initialized)
            Initialize();

        var componentType = component.GetType();
        var componentTypeName = componentType.Name;

        if (!_defaultValues.TryGetValue(componentTypeName, out var defaults))
            return;

        foreach (var (propertyName, defaultValue) in defaults)
        {
            var property = componentType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property == null || !property.CanWrite)
                continue;

            // Check if the property has the [Parameter] attribute
            if (!property.GetCustomAttributes<ParameterAttribute>().Any())
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
            _initialized = false;
        }
    }
}