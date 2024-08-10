// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Reflection;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Base;

public class ComponentBaseTests : TestContext
{
    /// <summary>
    /// List of components to exclude from the test.
    /// </summary>
    private static readonly Type[] Excluded = new[]
    {
        typeof(AspNetCore.Components._Imports)
    };

    /// <summary>
    /// List of customized actions to initialize the component with a specific type.
    /// </summary>
    private static readonly Dictionary<Type, Func<Type, Type>> ComponentInitializer = new()
    {
        { typeof(FluentIcon<>), type => type.MakeGenericType(typeof(Samples.Icons.Samples.Info)) }
    };

    [Fact]
    public void ComponentBase_Id()
    {
        using var ctx = new TestContext();
        
        foreach (var componentType in GetFluentComponents())
        {
            // Convert to generic type if needed
            var type = componentType;
            if (ComponentInitializer.ContainsKey(componentType))
            {
                type = ComponentInitializer[componentType](componentType);
            }

            // Arrange and Act
            var renderedComponent = ctx.RenderComponent<DynamicComponent>(parameters =>
            {
                parameters.Add(p => p.Type, type);
                parameters.Add(p => p.Parameters, new Dictionary<string, object>
                {
                    { "Id", "My-Specific-ID" }
                });
            });

            // Assert
            Assert.True(renderedComponent.Markup.Contains("My-Specific-ID", StringComparison.Ordinal), $"{componentType.Name} does not implement the 'Id' property.");
        }
    }

    private IEnumerable<Type> GetFluentComponents()
    {
        var componentBaseType = typeof(FluentComponentBase);
        var assembly = typeof(AspNetCore.Components._Imports).Assembly;

        return assembly.GetTypes()
                       .Where(t => componentBaseType.IsAssignableFrom(t) && !t.IsAbstract && !Excluded.Contains(t));
    }
}
