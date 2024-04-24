using Bunit;
using Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.NavMenu;

public class FluentNavGroupTests : TestBase
{
    public FluentNavGroupTests()
    {
        TestContext.JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void FluentNavGroup_Default()
    {
        // Arrange & Act
        var m = new FluentNavMenu
        {
            Expanded = true
        };
        var cut = TestContext.RenderComponent<FluentNavGroup>(parameters =>
        {
            parameters.Add(p => p.Owner, m);
            //parameters.Add(p => p.Owner.Expanded, true);
            parameters.Add(p => p.Title, "Group title");
            parameters.AddChildContent("NavGroups and NavLinks here");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavGroup_Empty()
    {
        // Arrange & Act
        var m = new FluentNavMenu();
        var cut = TestContext.RenderComponent<FluentNavGroup>(parameters =>
        {
            parameters.Add(p => p.Owner, m);
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavGroup_HideExpander()
    {
        // Arrange & Act
        var m = new FluentNavMenu();
        var cut = TestContext.RenderComponent<FluentNavGroup>(parameters =>
        {
            parameters.Add(p => p.Owner, m);
            parameters.Add(p => p.HideExpander, true);
            parameters.AddChildContent("NavGroups and NavLinks here");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavGroup_Disabled()
    {
        // Arrange & Act
        var m = new FluentNavMenu();
        var cut = TestContext.RenderComponent<FluentNavGroup>(parameters =>
        {
            parameters.Add(p => p.Owner, m);
            parameters.Add(p => p.Disabled, true);
            parameters.Add(p => p.Title, "Group title");
            parameters.AddChildContent("NavGroups and NavLinks here");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavGroup_TitleTemplate()
    {
        // Arrange & Act
        var m = new FluentNavMenu();
        var cut = TestContext.RenderComponent<FluentNavGroup>(parameters =>
        {
            parameters.Add(p => p.Owner, m);
            parameters.Add(p => p.TitleTemplate, "<h2>Group title</h2>");
            parameters.AddChildContent("NavGroups and NavLinks here");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavGroup_Gap()
    {
        // Arrange & Act
        var m = new FluentNavMenu();
        var cut = TestContext.RenderComponent<FluentNavGroup>(parameters =>
        {
            parameters.Add(p => p.Owner, m);
            parameters.Add(p => p.Title, "Group title");
            parameters.Add(p => p.Gap, "20px");
            parameters.AddChildContent("NavGroups and NavLinks here");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavGroup_Href()
    {
        // Arrange & Act
        var m = new FluentNavMenu();
        var cut = TestContext.RenderComponent<FluentNavGroup>(parameters =>
        {
            parameters.Add(p => p.Owner, m);
            parameters.Add(p => p.Title, "Group title");
            parameters.Add(p => p.Href, "/NavMenu");
            parameters.AddChildContent("NavGroups and NavLinks here");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavGroup_Icon()
    {
        // Arrange & Act
        var m = new FluentNavMenu();
        var cut = TestContext.RenderComponent<FluentNavGroup>(parameters =>
        {
            parameters.Add(p => p.Owner, m);
            parameters.Add(p => p.Title, "Group title");
            parameters.Add(p => p.Icon, SampleIcons.Info);
            parameters.AddChildContent("NavGroups and NavLinks here");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavGroup_IconAndIconColor()
    {
        // Arrange & Act
        var m = new FluentNavMenu();
        var cut = TestContext.RenderComponent<FluentNavGroup>(parameters =>
        {
            parameters.Add(p => p.Owner, m);
            parameters.Add(p => p.Title, "Group title");
            parameters.Add(p => p.Icon, SampleIcons.Info);
            parameters.Add(p => p.IconColor, Color.Neutral);
            parameters.AddChildContent("NavGroups and NavLinks here");
        });

        // Assert
        cut.Verify();
    }

}
