// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

/* ********************************************************
 *  ⚠️ DO NOT CHANGE THE FOLLOWING TEST.
 * ********************************************************
 */

using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Base;

public class AutofocusTests : BunitContext
{
    public AutofocusTests(ITestOutputHelper testOutputHelper)
    {
        Output = testOutputHelper;
        Services.AddFluentUIComponents();
    }

    public ITestOutputHelper Output { get; }

    [Theory]
    [MemberData(nameof(AutofocusComponents))]
    public void OnAfterRenderAsync_AutofocusComponents_AppliesAutofocusOnlyOnFirstRender(Type componentType, bool autofocus)
    {
        using var context = new DateTimeProviderContext(new DateTime(2022, 2, 15));
        JSInterop.Mode = JSRuntimeMode.Loose;
        Output.WriteLine(componentType.Name);

        var attributes = new Dictionary<string, object>
        {
            { nameof(FluentComponentBase.Id), "autofocus-component" },
            { nameof(IFluentComponentAutofocus.Autofocus), autofocus },
        };

        var cut = Render<DynamicComponent>(parameters => parameters
            .Add(p => p.Type, componentType)
            .Add(p => p.Parameters, attributes));

        var invocations = JSInterop.Invocations["Microsoft.FluentUI.Blazor.Utilities.DomFunctions.focus"];

        // If this component has autofocus enabled, it should have been focused once on the first render.
        // => Check if your `OnAfterRenderAsync` method contains `await this.ApplyAutofocusAsync(firstRender);`
        Assert.Equal(autofocus, invocations.Count > 0);
        Assert.All(invocations, invocation =>
        {
            Assert.Equal("autofocus-component", invocation.Arguments[0]);
            Assert.Equal(false, invocation.Arguments[1]);
        });

        cut.Render(parameters => parameters.Add(p => p.Parameters, new Dictionary<string, object>
        {
            { nameof(FluentComponentBase.Id), "autofocus-component" },
            { nameof(IFluentComponentAutofocus.Autofocus), true },
        }));

        Assert.Equal(autofocus, JSInterop.Invocations["Microsoft.FluentUI.Blazor.Utilities.DomFunctions.focus"].Count > 0);
    }

    [Theory]
    [MemberData(nameof(AutofocusComponents))]
    public async Task FocusAsync_AutofocusComponents_FocusesElementWithPreventScroll(Type componentType, bool preventScroll)
    {
        using var context = new DateTimeProviderContext(new DateTime(2022, 2, 15));
        JSInterop.Mode = JSRuntimeMode.Loose;
        Output.WriteLine(componentType.Name);

        var cut = Render<DynamicComponent>(parameters => parameters
            .Add(p => p.Type, componentType)
            .Add(p => p.Parameters, new Dictionary<string, object>
            {
                { nameof(FluentComponentBase.Id), "focus-component" },
                { nameof(IFluentComponentAutofocus.Autofocus), false },
            }));
        var component = Assert.IsAssignableFrom<IFluentComponentAutofocus>(cut.Instance.Instance);
        Assert.Empty(JSInterop.Invocations["Microsoft.FluentUI.Blazor.Utilities.DomFunctions.focus"]);

        await cut.InvokeAsync(() => component.FocusAsync(preventScroll));

        var invocation = Assert.Single(JSInterop.Invocations["Microsoft.FluentUI.Blazor.Utilities.DomFunctions.focus"]);
        Assert.Equal("focus-component", invocation.Arguments[0]);
        Assert.Equal(preventScroll, invocation.Arguments[1]);
    }

    public static TheoryData<Type, bool> AutofocusComponents()
    {
        var data = new TheoryData<Type, bool>();

        foreach (var componentType in BaseHelpers.GetDerivedTypes(typeof(IFluentComponentAutofocus), except: [])
                                                 .OrderBy(type => type.FullName, StringComparer.Ordinal))
        {
            var type = BaseHelpers.ComponentInitializer.TryGetValue(componentType, out var initializer)
                     ? initializer(componentType)
                     : componentType;

            data.Add(type, false);
            data.Add(type, true);
        }

        return data;
    }
}
