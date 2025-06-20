// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using Microsoft.JSInterop;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Base;

public class ComponentBaseTests : Bunit.TestContext
{
    /// <summary>
    /// List of components to exclude from the test.
    /// </summary>
    private static readonly Type[] Excluded =
    [
        typeof(AspNetCore.Components._Imports),
        typeof(DialogOptions),
        typeof(FluentRadio<>),  // TODO: To update
        typeof(FluentTab),      // Excluded because the Tab content in rendered in the parent FluentTabs component
    ];

    /// <summary>
    /// List of customized actions to initialize the component with a specific type and optional required parameters.
    /// </summary>
    private static readonly Dictionary<Type, Loader> ComponentInitializer = new()
    {
        { typeof(FluentIcon<>), Loader.MakeGenericType(typeof(Samples.Icons.Samples.Info))},
        { typeof(FluentSelect<>), Loader.MakeGenericType(typeof(int))},
        { typeof(FluentCombobox<>), Loader.MakeGenericType(typeof(int))},
        { typeof(FluentSlider<>), Loader.MakeGenericType(typeof(int))},
        { typeof(FluentRadioGroup<>), Loader.MakeGenericType(typeof(string)) },
        { typeof(FluentTooltip), Loader.Default.WithRequiredParameter("Anchor", "MyButton").WithRequiredParameter("UseTooltipService", false)},
        { typeof(FluentHighlighter), Loader.Default.WithRequiredParameter("HighlightedText", "AB").WithRequiredParameter("Text", "ABCDEF")},
        { typeof(FluentKeyCode), Loader.Default.WithRequiredParameter("ChildContent", (RenderFragment)(builder => builder.AddContent(0, "MyContent"))) },
        { typeof(FluentPaginator), Loader.Default.WithRequiredParameter("State", new PaginationState()) },
        { typeof(FluentDataGrid<>), Loader.MakeGenericType(typeof(string)) },
        { typeof(FluentDataGridRow<>), Loader.MakeGenericType(typeof(string)).WithCascadingValue(new InternalGridContext<string>(new FluentDataGrid<string>(new LibraryConfiguration()))) },
        { typeof(FluentDataGridCell<>), Loader.MakeGenericType(typeof(string))
                                       .WithCascadingValue(new InternalGridContext<string>(new FluentDataGrid<string>(new LibraryConfiguration())))
                                       .WithCascadingValue("OwningRow", new FluentDataGridRow<string>(new LibraryConfiguration()) { InternalGridContext = new InternalGridContext<string>(new FluentDataGrid<string>(new LibraryConfiguration())) }) },
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
            var type = ComponentInitializer.TryGetValue(componentType, out var value)
                     ? value.ComponentType(componentType)
                     : componentType;

            // Arrange and Act
            try
            {
                var renderedComponent = RenderComponent<DynamicComponent>(parameters =>
                {
                    parameters.Add(p => p.Type, type);

                    // Required parameters
                    parameters.Add(p => p.Parameters, DictionaryExtensions.Union(
                        new Dictionary<string, object>
                        {
                            { blazorAttribute.Name, blazorAttribute.Value }
                        },
                        ComponentInitializer.TryGetValue(componentType, out var valueRequired) ? valueRequired.RequiredParameters : null
                    ));

                    // Cascading values
                    if (ComponentInitializer.TryGetValue(componentType, out var valueCascading))
                    {
                        foreach (var (Name, Value) in valueCascading.CascadingValues)
                        {
                            if (string.IsNullOrEmpty(Name))
                            {
                                parameters.AddCascadingValue(Value);
                            }
                            else
                            {
                                parameters.AddCascadingValue(Name, Value);
                            }
                        }
                    }
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
            catch (Exception ex)
            {
                var error = $"Error rendering component {componentType?.Name}. Update the `ComponentInitializer` dictionary: {Environment.NewLine}{Environment.NewLine}{ex.Message}{Environment.NewLine}{Environment.NewLine}{ex.InnerException?.Message}";
                errors.AppendLine(error);
            }
        }

        Assert.True(errors.Length == 0, errors.ToString());
    }

    [Fact]
    public void ComponentBase_TooltipInterface_CorrectRendering()
    {
        var errors = new StringBuilder();

        JSInterop.Mode = JSRuntimeMode.Loose;

        foreach (var componentType in BaseHelpers.GetDerivedTypes<ITooltipComponent>(except: Excluded))
        {
            // Convert to generic type if needed
            var type = ComponentInitializer.TryGetValue(componentType, out var value)
                     ? value.ComponentType(componentType)
                     : componentType;

            // Arrange and Act
            var renderedComponent = RenderComponent<FluentStack>(stack =>
            {
                stack.AddChildContent<DynamicComponent>(parameters =>
                {
                    parameters.Add(p => p.Type, type);
                    parameters.Add(p => p.Parameters, DictionaryExtensions.Union(
                        new Dictionary<string, object>
                        {
                            { "Id", $"id-{type.Name}" },
                            { "Tooltip", $"My tooltip {type.Name}" },
                        },
                        ComponentInitializer.TryGetValue(componentType, out var valueRequired) ? valueRequired.RequiredParameters : null
                    ));
                });
                stack.AddChildContent<FluentTooltipProvider>();
            });

            // Assert

            var isMatch = Regex.IsMatch(renderedComponent.Markup, $"<fluent-tooltip .+><text>My tooltip {type.Name}<\\/text><\\/fluent-tooltip>");

            Output.WriteLine($"{(isMatch ? "✅" : "❌")} {componentType.Name}");

            if (!isMatch)
            {
                var error = $"\"{componentType.Name}\" does not correctly implement the \"Tooltip\" parameter.";
                errors.AppendLine(error);
            }
        }

        Assert.True(errors.Length == 0, errors.ToString());
    }

    [Fact]
    public void ComponentBase_TooltipInterface_NotImplemented()
    {
        var errors = new StringBuilder();

        JSInterop.Mode = JSRuntimeMode.Loose;

        foreach (var componentType in BaseHelpers.GetDerivedTypes<IFluentComponentBase>(except: Excluded))
        {
            // Check if the component contains a Tooltip property but without implementing the ITooltipComponent interface
            var hasTooltipProperty = componentType.GetProperty("Tooltip", BindingFlags.Public | BindingFlags.Instance) != null;
            var isImplementingTooltipComponent = typeof(ITooltipComponent).IsAssignableFrom(componentType);
            var hasTooltipParameterAttribute = componentType.GetProperty("Tooltip", BindingFlags.Public | BindingFlags.Instance)?.GetCustomAttribute<ParameterAttribute>() != null;

            if (hasTooltipProperty && !isImplementingTooltipComponent)
            {
                Output.WriteLine($"❌ {componentType.Name}");

                var error = $"\"{componentType.Name}\" contains the \"Tooltip\" property but does not implement the \"ITooltipComponent\" interface.";
                errors.AppendLine(error);
            }

            else if (hasTooltipProperty && !hasTooltipParameterAttribute)
            {
                Output.WriteLine($"❌ {componentType.Name}");

                var error = $"\"{componentType.Name}.Tooltip\" property is not a Blazor [Parameter].";
                errors.AppendLine(error);
            }

            else if (hasTooltipProperty)
            {
                Output.WriteLine($"✅ {componentType.Name}");
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
        public MyComponent() : base(LibraryConfiguration.Empty) { }

        public const string JAVASCRIPT_FILENAME = "FluentGrid.razor.js";
        public IJSObjectReference GetJSModule() => base.JSModule.ObjectReference;
    }

    private class Loader
    {
        public static Loader MakeGenericType(Type type) => new()
        {
            ComponentType = t => t.MakeGenericType(type)
        };

        public static Loader Default => new();

        private Loader() { }

        public Func<Type, Type> ComponentType { get; private set; } = t => t;

        public Dictionary<string, object> RequiredParameters { get; } = [];

        public List<(string? Name, object Value)> CascadingValues { get; } = [];

        public Loader WithRequiredParameter(string key, object value)
        {
            RequiredParameters.Add(key, value);
            return this;
        }

        public Loader WithCascadingValue<TValue>(string? name, TValue cascadingValue) where TValue : notnull
        {
            CascadingValues.Add((name, cascadingValue));
            return this;
        }

        public Loader WithCascadingValue<TValue>(TValue cascadingValue) where TValue : notnull
        {
            return WithCascadingValue(null, cascadingValue);
        }
    }

    private static class DictionaryExtensions
    {
        public static Dictionary<string, object> Union(Dictionary<string, object> first, Dictionary<string, object>? second)
        {
            if (second == null)
            {
                return first;
            }

            return first.Concat(second)
                .GroupBy(kvp => kvp.Key)
                .ToDictionary(g => g.Key, g => g.Last().Value);
        }
    }
}
