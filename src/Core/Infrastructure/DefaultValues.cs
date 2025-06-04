// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class DefaultValues
{
    // List of components and their Property/Default values.
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> _componentCache = new ConcurrentDictionary<Type, ConcurrentDictionary<string, object>>();

    /// <summary />
    public DefaultValuesComponentBuilder<TComponent> For<TComponent>()
    {
        var values = _componentCache.GetOrAdd(typeof(TComponent), _ => new ConcurrentDictionary<string, object>(StringComparer.Ordinal));

        return new DefaultValuesComponentBuilder<TComponent>(values);
    }

    /// <summary />
    [SuppressMessage("Trimming", "IL2075:'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.", Justification = "<Pending>")]
    internal void ApplyDefaults<TComponent>(TComponent component) where TComponent : IFluentComponentBase
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
