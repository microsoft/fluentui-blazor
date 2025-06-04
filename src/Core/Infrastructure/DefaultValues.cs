// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
[SuppressMessage("Design", "MA0048:File name must match type name", Justification = "<Pending>")]
public class DefaultValuesComponentBuilder<TComponent>
{
    private static readonly Type TComponentType = typeof(TComponent);
    private readonly ConcurrentDictionary<string, object> _values;

    /// <summary />
    public DefaultValuesComponentBuilder(ConcurrentDictionary<string, object> values)
    {
        _values = values ?? throw new ArgumentNullException(nameof(values));
    }

    /// <summary />
    [SuppressMessage("Trimming", "IL2080:'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The source field does not have matching annotations.", Justification = "<Pending>")]
    public void Set<TValue>(Expression<Func<TComponent, TValue>> parameterSelector, [AllowNull] TValue value)
    {
        // Inspiration from bUnit code
        ArgumentNullException.ThrowIfNull(parameterSelector);

        if (parameterSelector.Body is not MemberExpression { Member: PropertyInfo propInfoCandidate })
        {
            throw new ArgumentException($"The parameter selector '{parameterSelector}' does not resolve to a public property on the component '{typeof(TComponent)}'.", nameof(parameterSelector));
        }

        var propertyInfo = propInfoCandidate.DeclaringType != TComponentType
                         ? TComponentType.GetProperty(propInfoCandidate.Name, propInfoCandidate.PropertyType)
                         : propInfoCandidate;

        var propertyName = propertyInfo?.Name ?? throw new ArgumentException($"The parameter selector '{parameterSelector}' does not resolve to a public property on the component '{typeof(TComponent)}'.", nameof(parameterSelector));

        // Add the value to the dictionary, using the property name as the key.
        _values.AddOrUpdate(propertyName,
                            _ => value!,
                            (_, existing) =>
                            {
                                if (existing is TValue existingValue && EqualityComparer<TValue>.Default.Equals(existingValue, value!))
                                {
                                    return existing; // No change
                                }

                                return value!;
                            });
    }
}

/// <summary />
public class DefaultValues
{
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> _componentCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, object>>();

    /// <summary />
    public DefaultValuesComponentBuilder<TComponent> For<TComponent>()
    {
        var values = _componentCache.GetOrAdd(typeof(TComponent), _ => new ConcurrentDictionary<string, object>(StringComparer.Ordinal));

        return new DefaultValuesComponentBuilder<TComponent>(values);
    }

    /*
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <returns></returns>
    public void Add<TComponent>(string property, object value) where TComponent : IFluentComponentBase
    {
        _componentCache.AddOrUpdate(
            typeof(TComponent),
            _ => new ConcurrentDictionary<string, object>(StringComparer.Ordinal) { [property] = value },
            (_, existing) =>
            {
                existing[property] = value;
                return existing;
            });
    }
    */

    /// <summary>
    /// 
    /// </summary>
    /// <param name="component"></param>
    [SuppressMessage("Trimming", "IL2075:'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.", Justification = "<Pending>")]
    internal void ApplyDefaults(IFluentComponentBase component)
    {
        var componentType = component.GetType();

        if (_componentCache.TryGetValue(componentType, out var properties))
        {
            foreach (var property in properties)
            {
                var propInfo = componentType.GetProperty(property.Key, bindingAttr: BindingFlags.Public | BindingFlags.Instance);
                if (propInfo != null && propInfo.CanWrite)
                {
                    propInfo.SetValue(component, property.Value);
                }
            }
        }
    }
}
