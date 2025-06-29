// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides functionality to configure default values for component parameters of type <typeparamref name="TComponent" />.
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Test will be added later")]
public class DefaultValuesComponentBuilder<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] TComponent>
{
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
    private static readonly Type TComponentType = typeof(TComponent);
    private readonly ConcurrentDictionary<string, object?> _values;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultValuesComponentBuilder{TComponent}"/> class with the specified default
    /// values.
    /// </summary>
    /// <param name="values">A thread-safe dictionary containing the default values to be used by the component.  Keys represent the names of
    /// the values, and values represent their corresponding default values.  Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is <see langword="null"/>.</exception>
    internal DefaultValuesComponentBuilder(ConcurrentDictionary<string, object?> values)
    {
        _values = values ?? throw new ArgumentNullException(nameof(values));
    }

    /// <summary />
    public void Set<TValue>(Expression<Func<TComponent, TValue>> parameterSelector, [AllowNull] TValue value)
    {
        // Inspiration from bUnit code
        ArgumentNullException.ThrowIfNull(parameterSelector);

        if (parameterSelector.Body is not MemberExpression { Member: PropertyInfo propInfoCandidate })
        {
            throw new ArgumentException($"The parameter selector '{parameterSelector}' does not resolve to a public property on the component '{typeof(TComponent)}'.", nameof(parameterSelector));
        }

        // Property name
        var propertyInfo = propInfoCandidate.DeclaringType != TComponentType
                         ? TComponentType.GetProperty(propInfoCandidate.Name, propInfoCandidate.PropertyType)
                         : propInfoCandidate;

        var propertyName = propertyInfo?.Name
                        ?? throw new ArgumentException($"The parameter selector '{parameterSelector}' does not resolve to a public property on the component '{typeof(TComponent)}'.", nameof(parameterSelector));

        // Add the value to the dictionary, using the property name as the key.
        _values.AddOrUpdate(propertyName, AddFunction, UpdateFunction);

        object? AddFunction(string key)
        {
            return value;
        }

        object? UpdateFunction(string key, object? oldValue)
        {
            if (oldValue is TValue existingValue && EqualityComparer<TValue>.Default.Equals(existingValue, value))
            {
                return oldValue; // No change
            }

            return value;
        }
    }
}
