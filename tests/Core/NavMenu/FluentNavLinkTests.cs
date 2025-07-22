// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
using Bunit;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.NavMenu;

public class FluentNavLinkTests : TestBase
{
    public FluentNavLinkTests()
    {
        TestContext.JSInterop.Mode = JSRuntimeMode.Loose;
        TestContext.Services.AddSingleton(LibraryConfiguration.ForUnitTests);
    }
    [Fact]
    public void FluentNavLink_Default()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.AddChildContent("NavLink text");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavLink_ExtendedTitle()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.AddChildContent("<h3>NavLink text</h3>");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavLink_Href()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.Add(p => p.Href, "/NavMenu");
            parameters.AddChildContent("NavLink text");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavLink_Id()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.Add(p => p.Id, "nav-link-id");
            parameters.Add(p => p.Href, "/NavMenu");
            parameters.AddChildContent("NavLink text");
        });

        // Assert
        cut.Verify();
    }
    [Fact]
    public void FluentNavLink_Target()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.Add(p => p.Href, "/NavMenu");
            parameters.Add(p => p.Target, "_blank");
            parameters.AddChildContent("NavLink text");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavLink_Match()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.Add(p => p.Href, "/NavMenu");
            parameters.Add(p => p.Match, NavLinkMatch.All);
            parameters.AddChildContent("NavLink text");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavLink_ForceLoad()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.Add(p => p.Href, "/NavMenu");
            parameters.Add(p => p.ForceLoad, true);
            parameters.AddChildContent("NavLink text");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavLink_Icon()
    {
        // Arrange & Act
        var icon = new SampleIcons.Samples.Info();
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.Add(p => p.Icon, icon);
            parameters.AddChildContent("NavLink text");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavLink_IconAndIconColor()
    {
        // Arrange & Act
        var m = new FluentNavMenu();
        var icon = new SampleIcons.Samples.Info();
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.Add(p => p.Owner, m);
            parameters.Add(p => p.Icon, icon);
            parameters.Add(p => p.IconColor, Color.Neutral);
            parameters.AddChildContent("NavLink text");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavLink_IconWithCustomColor()
    {
        // Arrange & Act
        var m = new FluentNavMenu();
        var icon = new SampleIcons.Samples.Info();
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.Add(p => p.Owner, m);
            parameters.Add(p => p.Icon, icon);
            parameters.Add(p => p.IconColor, Color.Custom);
            parameters.Add(p => p.CustomColor, "red");
            parameters.AddChildContent("NavLink text");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavLink_OnClick()
    {
        Action<MouseEventArgs> onClickHandler = _ => { };

        // Arrange & Act
        var m = new FluentNavMenu();
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.Add(p => p.Owner, m);
            parameters.Add(p => p.OnClick, onClickHandler);
            parameters.AddChildContent("NavLink text");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentNavLink_InsideNavGroup_WithHref()
    {
        // Arrange & Act
        var m = new FluentNavMenu
        {
            Expanded = true
        };
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.Add(p => p.Owner, m);
            parameters.Add(p => p.Href, "/example-page");
            parameters.AddChildContent("NavLink text");
        });

        // Assert
        cut.Verify();

        // Verify that FluentKeyCode is added when Owner is not null and Href is set
        var fluentKeyCode = cut.FindComponent<FluentKeyCode>();
        Assert.NotNull(fluentKeyCode);

        // Verify the anchor points to the NavLink element
        var navLinkId = cut.Find("a[id]").GetAttribute("id");
        Assert.Contains("navlink", navLinkId);
    }

    [Fact]
    public void FluentNavLink_KeyboardNavigation_InsideNavGroup()
    {
        // Arrange & Act - Test NavLink inside a NavGroup scenario
        var m = new FluentNavMenu
        {
            Expanded = true
        };
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.Add(p => p.Owner, m);
            parameters.Add(p => p.Href, "/example-page");
            parameters.AddChildContent("Example page");
        });

        // Assert - Verify that keyboard handling is added for NavLink inside group
        cut.Verify();

        // Verify FluentKeyCode component is present
        var fluentKeyCode = cut.FindComponent<FluentKeyCode>();
        Assert.NotNull(fluentKeyCode);

        // Verify the NavLink has the expected id
        var navLink = cut.Find("a[id]");
        var navLinkId = navLink.GetAttribute("id");
        Assert.Contains("-navlink", navLinkId);
    }

    [Fact]
    public void FluentNavLink_KeyboardNavigation_Standalone()
    {
        // Arrange & Act - Test standalone NavLink (no Owner)
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.Add(p => p.Href, "/example-page");
            parameters.AddChildContent("Example page");
        });

        // Assert - Verify that no keyboard handling is added for standalone NavLink
        cut.Verify();

        // Verify no FluentKeyCode component is present
        Assert.Throws<ComponentNotFoundException>(() => cut.FindComponent<FluentKeyCode>());
    }

    //ActiveClass
    //Match
    //Target
}
