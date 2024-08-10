// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

    /// <summary>
    /// Test to verify that all FluentUI components implement the default properties (Id, Class, Style)
    /// from <see cref="FluentComponentBase"/>.
    ///
    /// ⚠️ DO NOT CHANGE THE FOLLOWING TEST.
    /// </summary>
    /// <param name="propName">Blazor Component property name</param>
    /// <param name="htmlName">HTML attribute name</param>
    /// <param name="value">Value</param>
    [Theory]
    [InlineData("Id", "id", "My-Specific-ID")]
    [InlineData("Class", "class", "My-Specific-Class")]
    [InlineData("Style", "style", "My-Specific-Style")]
    [InlineData("extra-attribute", "extra-attribute", "My-Specific-Attribute")]  // AdditionalAttributes
    public void ComponentBase_DefaultProperties(string propName, string? htmlName, string value)
    {
        var errors = new StringBuilder();

        foreach (var componentType in GetFluentComponents())
        {
            // Convert to generic type if needed
            var type = componentType;
            if (ComponentInitializer.ContainsKey(componentType))
            {
                type = ComponentInitializer[componentType](componentType);
            }

            // Arrange and Act
            var renderedComponent = RenderComponent<DynamicComponent>(parameters =>
            {
                parameters.Add(p => p.Type, type);
                parameters.Add(p => p.Parameters, new Dictionary<string, object>
                {
                    { propName, value }
                });
            });

            // Assert
            var pattern = @$"\b{htmlName}\s*=\s*[""'][^""']*\b{value}\b[^""']*[""']";
            var isMatch = Regex.IsMatch(renderedComponent.Markup, pattern);

            if (!isMatch)
            {
                var error = $"\"{componentType.Name}\" does not use the \"{propName}\" property/attribute (missing HTML attribute {htmlName}=\"{value}\").";
                errors.AppendLine(error);
            }
        }

        Assert.True(errors.Length == 0, errors.ToString());
    }

    /// <summary>
    /// Returns all FluentUI components, which inherit from <see cref="FluentComponentBase"/>.
    /// </summary>
    /// <returns></returns>
    private IEnumerable<Type> GetFluentComponents()
    {
        var componentBaseType = typeof(FluentComponentBase);
        var assembly = typeof(AspNetCore.Components._Imports).Assembly;

        return assembly.GetTypes()
                       .Where(t => componentBaseType.IsAssignableFrom(t) && !t.IsAbstract && !Excluded.Contains(t));
    }
}
