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
