using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.NavMenu;

public class FluentNavLinkTests : TestBase
{
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
    public void FluentNavLink_ExtendedTtitle()
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
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.Add(p => p.Icon, SampleIcons.Info);
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
        var cut = TestContext.RenderComponent<FluentNavLink>(parameters =>
        {
            parameters.Add(p => p.Owner, m);
            parameters.Add(p => p.Icon, SampleIcons.Info);
            parameters.Add(p => p.IconColor, Color.Neutral);
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

    //ActiveClass
    //Match
    //Target
}
