// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
[ExcludeFromCodeCoverage(Justification = "Test will be added later")]
public class DefaultValues
{
    // List of components and their Property/Default values.
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object?>> _componentCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, object?>>();

    private bool _isInitialized;

    /// <summary>
    /// Registers default values for a specific component type.
    /// E.g. FluentButton or FluentAutocomplete&lt;string, string&gt;).
    /// For generic components, use ForAny with a closed generic type (e.g., FluentAutocomplete&lt;object, object&gt;).
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <returns></returns>
    public DefaultValuesComponentBuilder<TComponent> For<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TComponent>()
    {
        _isInitialized = true;

        var values = _componentCache.GetOrAdd(typeof(TComponent), _ => new ConcurrentDictionary<string, object?>(StringComparer.Ordinal));

        return new DefaultValuesComponentBuilder<TComponent>(values);
    }

    /// <summary>
    /// Registers default values for all generic instantiations of a component.
    /// Use any closed generic as TComponent (e.g., FluentAutocomplete&lt;object, object&gt;).
    /// </summary>
    public DefaultValuesComponentBuilder<TComponent> ForAny<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TComponent>()
    {
        var type = typeof(TComponent);
        var key = type.IsGenericType
            ? type.GetGenericTypeDefinition()
            : type;

        _isInitialized = true;
        var values = _componentCache.GetOrAdd(key, _ => new ConcurrentDictionary<string, object?>(StringComparer.Ordinal));

        return new DefaultValuesComponentBuilder<TComponent>(values);
    }

    /// <summary />
    [SuppressMessage("Trimming", "IL2075:'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.",
                     Justification = "TComponent properties are preserved via DynamicDependency attributes. The usage of TComponent.GetType() generates this IL2075 warning.")]
    internal void ApplyDefaults<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TComponent>(TComponent component)
        where TComponent : IFluentComponentBase
    {
        var componentType = component.GetType();
        var properties = GetCachedProperties(componentType);

        if (properties is not null)
        {
            foreach (var property in properties)
            {
                var propInfo = componentType.GetProperty(property.Key, BindingFlags.Public | BindingFlags.Instance);
                if (propInfo != null && propInfo.CanWrite)
                {
                    propInfo.SetValue(component, property.Value);
                }
            }
        }
    }

    [SuppressMessage("Trimming", "IL2075:'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.",
                     Justification = "TComponent properties are preserved via DynamicDependency attributes. The usage of TComponent.GetType() generates this IL2075 warning.")]
    internal void SetInitialValues<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TComponent>(TComponent component, ReadOnlySpan<(string Name, object? Value)> initialValues)
        where TComponent : IFluentComponentBase
    {
        var componentType = component.GetType();
        var properties = GetCachedProperties(componentType);

        // Check if the propertyName already has a default value set
        if (properties is not null)
        {
            foreach (var kvp in initialValues)
            {
                if (!properties.ContainsKey(kvp.Name))
                {
                    var propInfo = componentType.GetProperty(kvp.Name, BindingFlags.Public | BindingFlags.Instance);
                    if (propInfo != null && propInfo.CanWrite)
                    {
                        propInfo.SetValue(component, kvp.Value);
                    }
                }
            }
        }
    }

    /// <summary />
    private ConcurrentDictionary<string, object?>? GetCachedProperties(Type componentType)
    {
        if (!_isInitialized || componentType is null)
        {
            return null;
        }

        // Try exact type first
        if (!_componentCache.TryGetValue(componentType, out var properties))
        {
            // Fallback: check open generic type definition
            if (componentType.IsGenericType)
            {
                _componentCache.TryGetValue(componentType.GetGenericTypeDefinition(), out properties);
            }
        }

        return properties;
    }
}
