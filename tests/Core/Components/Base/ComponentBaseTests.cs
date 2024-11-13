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
using Xunit;
using Xunit.Abstractions;

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

    /// <summary />
    public ComponentBaseTests(ITestOutputHelper testOutputHelper)
    {
        Output = testOutputHelper;
    }

    /// <summary>
    /// Gets the test output helper.
    /// </summary>
    public ITestOutputHelper Output { get; }

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
    public void ComponentBase_DefaultProperties(string propName, string htmlName, object value)
    {
        var errors = new StringBuilder();

        JSInterop.Mode = JSRuntimeMode.Loose;

        foreach (var componentType in BaseHelpers.GetDerivedTypes<FluentComponentBase>(except: Excluded))
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
            var isMatch = renderedComponent.Markup.ContainsAttribute(htmlName, value);

            Output.WriteLine($"{(isMatch ? "✅" : "❌")} {componentType.Name}");

            if (!isMatch)
            {
                var error = $"\"{componentType.Name}\" does not use the \"{propName}\" property/attribute (missing HTML attribute {htmlName}=\"{value}\").";
                errors.AppendLine(error);
            }
        }

        Assert.True(errors.Length == 0, errors.ToString());
    }

    [Fact]
    public void ComponentBase_JsModule()
    {
        // Arrange
        JSInterop.Mode = JSRuntimeMode.Strict;
        Services.AddSingleton<LibraryConfiguration>();

        var module = JSInterop.SetupModule(matcher => matcher.Arguments.Any(i => i?.ToString()?.EndsWith(MyComponent.JAVASCRIPT_FILENAME) == true));
        module.Mode = JSRuntimeMode.Loose;

        // Act
        var cut = RenderComponent<MyComponent>(parameter =>
        {
            parameter.Add(p => p.OnBreakpointEnter, EventCallback.Factory.Create<GridItemSize>(this, e => { }));
        });

        // Assert
        Assert.NotNull(cut.Instance.GetJSModule());
    }

    [Fact]
    public void ComponentBase_JsModule_Undefined()
    {
        // Arrange
        JSInterop.Mode = JSRuntimeMode.Strict;
        Services.AddSingleton<LibraryConfiguration>();

        var module = JSInterop.SetupModule(matcher => matcher.Arguments.Any(i => i?.ToString()?.EndsWith(MyComponent.JAVASCRIPT_FILENAME) == true));
        module.Mode = JSRuntimeMode.Loose;

        // Assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            // Act: no OnBreakpointEnter
            var cut = RenderComponent<MyComponent>(parameter =>
            {
            });

            var module = cut.Instance.GetJSModule();
        });
    }

    // Class used by the "ComponentBase_JsModule" test
    private class MyComponent : FluentGrid
    {
        public const string JAVASCRIPT_FILENAME = "FluentGrid.razor.js";
        public IJSObjectReference GetJSModule() => base.JSModule;
    }
}
