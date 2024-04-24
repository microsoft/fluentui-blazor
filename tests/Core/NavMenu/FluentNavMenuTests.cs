using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.NavMenu;

public class FluentNavMenuTests : TestBase
{
    public FluentNavMenuTests()
    {
        TestContext.JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void FluentNavMenu_Default()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentNavMenu>(parameters =>
        {
            parameters.AddChildContent("NavGroups and NavLinks here");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavMenu_Collapsible()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentNavMenu>(parameters =>
        {
            parameters.Add(p => p.Collapsible, true);
            parameters.AddChildContent("NavGroups and NavLinks here");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavMenu_NotExpanded()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentNavMenu>(parameters =>
        {
            parameters.Add(p => p.Expanded, false);
            parameters.AddChildContent("NavGroups and NavLinks here");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavMenu_CustomTitle()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentNavMenu>(parameters =>
        {
            parameters.Add(p => p.Title, "Custom title");
            parameters.AddChildContent("NavGroups and NavLinks here");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavMenu_CollapsibleCustomTitle()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentNavMenu>(parameters =>
        {
            parameters.Add(p => p.Collapsible, true);
            parameters.Add(p => p.Title, "Custom title");
            parameters.AddChildContent("NavGroups and NavLinks here");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavMenu_Width()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentNavMenu>(parameters =>
        {
            parameters.Add(p => p.Width, 300);
            parameters.AddChildContent("NavGroups and NavLinks here");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavMenu_Margin()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentNavMenu>(parameters =>
        {
            parameters.Add(p => p.Margin, "5px 15px");
            parameters.AddChildContent("NavGroups and NavLinks here");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavMenu_ExpanderContent()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentNavMenu>(parameters =>
        {
            parameters.Add(p => p.Collapsible, true);
            parameters.Add(p => p.ExpanderContent, "<div>custom expander</div>");
        });

        // Assert
        cut.Verify();
    }
}
