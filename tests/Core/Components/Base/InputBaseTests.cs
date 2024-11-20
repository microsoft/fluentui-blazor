// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Base;

public class InputBaseTests : TestContext
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
        // { typeof(FluentIcon<>), type => type.MakeGenericType(typeof(Samples.Icons.Samples.Info)) }
        { typeof(FluentSelect<>), type => type.MakeGenericType(typeof(string)) }    // FluentSelect<string>
    };

    /// <summary />
    public InputBaseTests(ITestOutputHelper testOutputHelper)
    {
        Output = testOutputHelper;
    }

    /// <summary>
    /// Gets the test output helper.
    /// </summary>
    public ITestOutputHelper Output { get; }

    /// <summary>
    /// Test to verify that all FluentUI components implement the default properties (Label, Disabled, ReadOnly, ...)
    /// from <see cref="FluentInputBase{TValue}"/>.
    ///
    /// ⚠️ DO NOT CHANGE THE FOLLOWING TEST.
    /// </summary>
    /// <param name="propName">Blazor Component property name</param>
    /// <param name="htmlName">HTML attribute name</param>
    /// <param name="value">Value</param>
    [Theory]
    [InlineData("Name", "name", "my-name")]
    [InlineData("Disabled", "disabled", true)]
    [InlineData("ReadOnly", "readonly", true)]
    [InlineData("AriaLabel", "aria-label", "my-aria-label")]
    [InlineData("Autofocus", "autofocus", true)]
    [InlineData("Required", "required", true)]
    [InlineData("Label", "", "my-label")]
    public void InputBase_DefaultProperties(string propName, string htmlName, object value)
    {
        var errors = new StringBuilder();

        JSInterop.Mode = JSRuntimeMode.Loose;

        foreach (var componentType in BaseHelpers.GetDerivedTypes(baseType: typeof(FluentInputBase<>), except: Excluded))
        {
            // Convert to generic type if needed
            var type = ComponentInitializer.ContainsKey(componentType)
                     ? ComponentInitializer[componentType](componentType)
                     : componentType;

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
            var isMatch = string.IsNullOrEmpty(htmlName)
                        ? renderedComponent.Markup.Contains(value.ToString() ?? "")
                        : renderedComponent.Markup.ContainsAttribute(htmlName, value);

            Output.WriteLine($"{(isMatch ? "✅" : "❌")} {componentType.Name}");

            if (!isMatch)
            {
                var error = $"\"{componentType.Name}\" does not use the \"{propName}\" property/attribute (missing HTML attribute {htmlName}=\"{value}\").";
                errors.AppendLine(error);
            }
        }

        Assert.True(errors.Length == 0, errors.ToString());
    }
}
