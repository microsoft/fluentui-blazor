// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

/* ********************************************************
 *  ⚠️ DO NOT CHANGE THE FOLLOWING TEST.
 * ********************************************************
 */

using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
        Services.AddFluentUIComponents();
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
    /// <param name="attributeName">Blazor Component property name</param>
    /// <param name="attributeValue">Blazor Component property htmlValue</param>
    /// <param name="htmlAttribute">HTML attribute name (null to verify only if the `htmlValue` is included in the Markup</param>
    /// <param name="htmlValue">HTML attribute Value (null to use the same `attributeValue` content</param>
    /// <param name="extraCondition">Add a custom extra rules, hardcoded in the test</param>
    [Theory]
    [InlineData("Name", "my-name", "name")]
    [InlineData("Disabled", true, "disabled")]
    [InlineData("ReadOnly", true, "readonly")]
    [InlineData("AriaLabel", "my-aria-label", "aria-label")]
    [InlineData("Autofocus", true, "autofocus")]
    [InlineData("Required", true, "required")]
    [InlineData("Label", "my-label")]
    [InlineData("LabelWidth", "150px", null, "width: 150px;", "Set_LabelPosition_Before")]
    [InlineData("LabelPosition", LabelPosition.Before, "label-position", "before")]
    [InlineData("Message", "my-message", null, null, "Add_MessageCondition_AlwaysTrue")]
    [InlineData("MessageState", MessageState.Success, null, "color: var(--success);", "Add_MessageCondition_AlwaysTrue")]
    [InlineData("InputSlot", "input", null, "slot=\"input\"")]
    [InlineData("LostFocus", "input", null, null, "Check_LostFocus")]
    public void InputBase_DefaultProperties(string attributeName, object attributeValue, string? htmlAttribute = null, object? htmlValue = null, string? extraCondition = null)
    {
        var errors = new StringBuilder();
        var localizer = Services.GetRequiredService<IFluentLocalizer>();

        if (htmlValue == null)
        {
            htmlValue = attributeValue;
        }

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
                var attributes = new Dictionary<string, object>
                {
                    { attributeName, attributeValue },
                };

                // Extra conditions
                switch (extraCondition)
                {
                    case "Set_LabelPosition_Before":
                        attributes.Add("Label", "my-label");
                        attributes.Add("LabelPosition", LabelPosition.Before);
                        break;

                    case "Add_MessageCondition_AlwaysTrue":
                        attributes.Add("MessageCondition", (IFluentField e) => true);
                        break;
                }

                parameters.Add(p => p.Type, type);
                parameters.Add(p => p.Parameters, attributes);
            });

            // Assert
            var isMatch = string.IsNullOrEmpty(htmlAttribute)
                        ? renderedComponent.Markup.Contains(htmlValue.ToString() ?? "")
                        : renderedComponent.Markup.ContainsAttribute(htmlAttribute, htmlValue);

            // LostFocus
            if (extraCondition == "Check_LostFocus")
            {
                isMatch = VerifyLostFocus(renderedComponent);
            }

            Output.WriteLine($"{(isMatch ? "✅" : "❌")} {componentType.Name}");

            if (!isMatch)
            {
                var error = $"\"{componentType.Name}\" does not use the \"{attributeName}\" attribute (missing HTML attribute {htmlAttribute}=\"{htmlValue}\").";
                errors.AppendLine(error);
            }
        }

        Assert.True(errors.Length == 0, errors.ToString());
    }

    private bool VerifyLostFocus(IRenderedComponent<DynamicComponent> component)
    {
        try
        {
            var input = component.Find("[slot='input']");
            input.FocusOut();

            var field = component.FindComponent<FluentField>();
            return field.Instance.InputComponent?.FocusLost ?? false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
