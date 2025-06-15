using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Infrastructure;
using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Attributes;

public class FluentDefaultIntegrationTests : TestBase
{
    // External defaults class that would typically be in user code
    public static class AppWideDefaults
    {
        [FluentDefault("SampleFluentButton")]
        public static Appearance? Appearance => Components.Appearance.Outline;

        [FluentDefault("SampleFluentButton")]
        public static string? Class => "app-button";

        [FluentDefault("SampleFluentButton")]
        public static bool Disabled => false;

        [FluentDefault("SampleDesignProvider")]
        public static LocalizationDirection? Direction => LocalizationDirection.RightToLeft;
    }

    // Sample component that mimics FluentButton structure
    public class SampleFluentButton : FluentComponentBase
    {
        [Parameter]
        public Appearance? Appearance { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "button");
            builder.AddAttribute(1, "class", Class);
            builder.AddAttribute(2, "disabled", Disabled);
            builder.AddAttribute(3, "data-appearance", Appearance?.ToString().ToLowerInvariant());
            builder.AddContent(4, "Button Text");
            builder.CloseElement();
        }
    }

    // Sample component that mimics design provider
    public class SampleDesignProvider : FluentComponentBase
    {
        [Parameter]
        public LocalizationDirection? Direction { get; set; }

        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", Class);
            builder.AddAttribute(2, "dir", Direction?.ToString().ToLowerInvariant());
            builder.AddContent(3, "Provider Content");
            builder.CloseElement();
        }
    }

    [Fact]
    public void FluentDefault_IntegrationTest_AppliesDefaultsCorrectly()
    {
        // Arrange
        FluentDefaultValuesService.Reset(); // Ensure clean state

        // Act - Render component without any parameters
        var component = TestContext.RenderComponent<SampleFluentButton>();

        // Assert - Defaults should be applied
        var markup = component.Markup;
        Assert.Contains("app-button", markup);
        Assert.Contains("data-appearance=\"outline\"", markup);
        Assert.DoesNotContain("disabled", markup); // disabled=false means no disabled attribute
    }

    [Fact]
    public void FluentDefault_IntegrationTest_ExplicitParametersOverrideDefaults()
    {
        // Arrange
        FluentDefaultValuesService.Reset(); // Ensure clean state

        // Act - Render component with explicit parameters
        var component = TestContext.RenderComponent<SampleFluentButton>(parameters => parameters
            .Add(p => p.Appearance, Components.Appearance.Accent)
            .Add(p => p.Class, "custom-button")
            .Add(p => p.Disabled, true));

        // Assert - Explicit values should be used
        var markup = component.Markup;
        Assert.Contains("custom-button", markup);
        Assert.Contains("data-appearance=\"accent\"", markup);
        Assert.Contains("disabled", markup);
        Assert.DoesNotContain("app-button", markup);
        Assert.DoesNotContain("outline", markup);
    }

    [Fact]
    public void FluentDefault_IntegrationTest_MixedExplicitAndDefaultParameters()
    {
        // Arrange
        FluentDefaultValuesService.Reset(); // Ensure clean state

        // Act - Render component with some explicit parameters
        var component = TestContext.RenderComponent<SampleFluentButton>(parameters => parameters
            .Add(p => p.Appearance, Components.Appearance.Stealth));

        // Assert - Mix of explicit and default values
        var markup = component.Markup;
        Assert.Contains("app-button", markup); // Default class
        Assert.Contains("data-appearance=\"stealth\"", markup); // Explicit appearance
        Assert.DoesNotContain("disabled", markup); // Default disabled=false
    }

    [Fact]
    public void FluentDefault_IntegrationTest_WorksForDifferentComponentTypes()
    {
        // Arrange
        FluentDefaultValuesService.Reset(); // Ensure clean state

        // Act - Render design provider component
        var component = TestContext.RenderComponent<SampleDesignProvider>();

        // Assert - Different component gets its own defaults
        var markup = component.Markup;
        Assert.Contains("dir=\"righttoleft\"", markup);
    }
}