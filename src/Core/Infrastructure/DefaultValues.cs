// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class DefaultValues
{
    // List of components and their Property/Default values.
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object?>> _componentCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, object?>>();

    // Per-type cache of the properties merged across the inheritance chain, with PropertyInfo resolved once and reused
    // across every ApplyDefaults/SetInitialValues call, avoiding reflection lookups on every component instantiation.
    private readonly ConcurrentDictionary<Type, CachedDefault[]?> _mergedCache = new ConcurrentDictionary<Type, CachedDefault[]?>();

    private bool _isInitialized;

    /// <summary>
    /// A default value with its <see cref="PropertyInfo"/> already resolved, so <see cref="ApplyDefaults{TComponent}(TComponent)"/>
    /// never needs to perform a reflection lookup by name.
    /// </summary>
    private readonly record struct CachedDefault(PropertyInfo Property, object? Value);

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
                property.Property.SetValue(component, property.Value);
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
                if (!ContainsProperty(properties, kvp.Name))
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
    private CachedDefault[]? GetCachedProperties(Type componentType)
    {
        if (!_isInitialized || componentType is null)
        {
            return null;
        }

        // Get the merged properties for the component type, computing them if they haven't been cached yet.
        return _mergedCache.GetOrAdd(componentType, BuildMergedProperties);
    }

    /// <summary>
    /// Walks up the type hierarchy, merging defaults from base classes so that values registered on a more derived type
    /// take precedence, then resolves each property's <see cref="PropertyInfo"/> once. The result is cached per exact
    /// component type by <see cref="GetCachedProperties"/> so no reflection lookup is repeated on later calls.
    /// </summary>
    private CachedDefault[]? BuildMergedProperties(Type componentType)
    {
        Dictionary<string, object?>? merged = null;

        for (var currentType = componentType; currentType is not null && currentType != typeof(object); currentType = currentType.BaseType)
        {
            if (!TryGetRegisteredProperties(currentType, out var properties))
            {
                continue;
            }

            if (merged is null)
            {
                merged = new Dictionary<string, object?>(properties, StringComparer.Ordinal);
                continue;
            }

            // A base class also has defaults: only add names not already set by a more derived type.
            foreach (var property in properties)
            {
                merged.TryAdd(property.Key, property.Value);
            }
        }

        if (merged is null)
        {
            return null;
        }

        // Resolve PropertyInfo for each property name once, so ApplyDefaults never needs to perform a reflection lookup by name.
        var resolved = new CachedDefault[merged.Count];
        var count = 0;

        foreach (var (name, value) in merged)
        {
            var propInfo = componentType.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
            if (propInfo != null && propInfo.CanWrite)
            {
                resolved[count++] = new CachedDefault(propInfo, value);
            }
        }

        return count == resolved.Length ? resolved : resolved[..count];
    }

    /// <summary>
    /// Tries to retrieve the registered properties for a given component type, checking both the exact type and its open generic definition if applicable.
    /// </summary>
    private bool TryGetRegisteredProperties(Type componentType, [NotNullWhen(true)] out ConcurrentDictionary<string, object?>? properties)
    {
        if (_componentCache.TryGetValue(componentType, out properties))
        {
            return true;
        }

        if (componentType.IsGenericType)
        {
            return _componentCache.TryGetValue(componentType.GetGenericTypeDefinition(), out properties);
        }

        properties = null;
        return false;
    }

    /// <summary>
    /// Checks if the provided property name exists in the cached properties array.
    /// Using a foreach loop instead of LINQ to avoid allocations and improve performance.
    /// </summary>
    private static bool ContainsProperty(CachedDefault[] properties, string name)
    {
        foreach (var property in properties)
        {
            if (string.Equals(property.Property.Name, name, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }
}
