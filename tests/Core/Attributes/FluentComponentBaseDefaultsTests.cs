using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Infrastructure;
using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Attributes;

public class FluentComponentBaseDefaultsTests : TestBase
{
    public static class IntegrationTestDefaults
    {
        [FluentDefault("TestIntegrationComponent")]
        public static string? DefaultStyle => "color: blue;";

        [FluentDefault("TestIntegrationComponent")]
        public static string? DefaultClass => "integration-test-class";
    }

    public class TestIntegrationComponent : FluentComponentBase
    {
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", Class);
            builder.AddAttribute(2, "style", Style);
            builder.AddContent(3, "Test Content");
            builder.CloseElement();
        }
    }

    [Fact]
    public void FluentComponentBase_AppliesDefaults_OnInitialization()
    {
        // Arrange
        FluentDefaultValuesService.Reset(); // Ensure clean state

        // Act
        var component = TestContext.RenderComponent<TestIntegrationComponent>();

        // Assert
        var markup = component.Markup;
        Assert.Contains("integration-test-class", markup);
        Assert.Contains("color: blue;", markup);
    }

    [Fact]
    public void FluentComponentBase_PreservesExplicitValues_OverDefaults()
    {
        // Arrange
        FluentDefaultValuesService.Reset(); // Ensure clean state

        // Act
        var component = TestContext.RenderComponent<TestIntegrationComponent>(parameters => parameters
            .Add(p => p.Class, "explicit-class")
            .Add(p => p.Style, "color: red;"));

        // Assert
        var markup = component.Markup;
        Assert.Contains("explicit-class", markup);
        Assert.Contains("color: red;", markup);
        Assert.DoesNotContain("integration-test-class", markup);
        Assert.DoesNotContain("color: blue;", markup);
    }
}