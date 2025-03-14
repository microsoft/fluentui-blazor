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
        typeof(AspNetCore.Components._Imports),
        typeof(DialogOptions),
    };

    /// <summary>
    /// List of customized actions to initialize the component with a specific type.
    /// </summary>
    private static readonly Dictionary<Type, Func<Type, Type>> ComponentInitializer = new()
    {
        { typeof(FluentIcon<>), type => type.MakeGenericType(typeof(Samples.Icons.Samples.Info)) },
        { typeof(FluentSelect<>), type => type.MakeGenericType(typeof(int)) },
        { typeof(FluentSlider<>), type => type.MakeGenericType(typeof(int)) },
    };

    /// <summary />
    public ComponentBaseTests(ITestOutputHelper testOutputHelper)
    {
        Output = testOutputHelper;
        Services.AddFluentUIComponents();
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
    /// <param name="blazor">Blazor Component property name/value</param>
    /// <param name="html">Expected HTML attribute name/value</param>
    [Theory]
    [InlineData("Id='id='My-Specific-ID'", "id='My-Specific-ID'")]
    [InlineData("Class='My-Specific-Item'", "class='My-Specific-Item'")]
    [InlineData("Style='My-Specific-Style'", "style='My-Specific-Style'")]
    [InlineData("Margin='10px'", "style='margin: 10px;*'")]                                                 // `*` is required to accept `;` in the Regex
    [InlineData("Padding='10px'", "style='padding: 10px;*'")]                                               // `*` is required to accept `;` in the Regex
    [InlineData("Margin='my-margin'", "class='my-margin'")]
    [InlineData("Padding='my-padding'", "class='my-padding'")]
    [InlineData("extra-attribute='My-Specific-Attribute'", "extra-attribute='My-Specific-Attribute'")]      // AdditionalAttributes
    public void ComponentBase_DefaultProperties(string blazor, string html)
    {
        var errors = new StringBuilder();
        var blazorAttribute = ParseHtmlAttribute(blazor);
        var htmlAttribute = ParseHtmlAttribute(html);

        JSInterop.Mode = JSRuntimeMode.Loose;

        foreach (var componentType in BaseHelpers.GetDerivedTypes<IFluentComponentBase>(except: Excluded))
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
                    { blazorAttribute.Name, blazorAttribute.Value }
                });
            });

            // Assert
            var isMatch = renderedComponent.Markup.ContainsAttribute(htmlAttribute.Name, htmlAttribute.Value);

            Output.WriteLine($"{(isMatch ? "✅" : "❌")} {componentType.Name}");

            if (!isMatch)
            {
                var error = $"\"{componentType.Name}\" does not use the \"{blazorAttribute.Name}\" property/attribute (missing HTML attribute {htmlAttribute.Name}=\"{htmlAttribute.Value}\").";
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

    // Helper method to parse HTML attributes
    private static (string Name, string Value) ParseHtmlAttribute(string attributeString)
    {
        var parts = attributeString.Split(['='], 2);
        if (parts.Length != 2)
        {
            throw new ArgumentException("Invalid attribute string format.", nameof(attributeString));
        }

        var name = parts[0].Trim();
        var value = parts[1].Trim(' ', '"', '\'');

        return (name, value);
    }

    // Class used by the "ComponentBase_JsModule" test
    private class MyComponent : FluentGrid
    {
        public const string JAVASCRIPT_FILENAME = "FluentGrid.razor.js";
        public IJSObjectReference GetJSModule() => base.JSModule.ObjectReference;
    }
}
