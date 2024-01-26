using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.CollapsibleRegion;
public class FluentCollapsibleRegionTests : TestBase
{
    [Fact]
    public void FluentCollapsibleRegion_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";

        var cut = TestContext.RenderComponent<FluentCollapsibleRegion>(parameters => parameters
            .Add(p => p.Expanded, true)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCollapsibleRegion_NotExpanded()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        Action<bool> expandedChanged = _ => { };
        var cut = TestContext.RenderComponent<FluentCollapsibleRegion>(parameters => parameters
            .Add(p => p.Expanded, false)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCollapsibleRegion_MaxHeight()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        var cut = TestContext.RenderComponent<FluentCollapsibleRegion>(parameters => parameters
            .Add(p => p.Expanded, true)
            .Add(p => p.MaxHeight, "666px")
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }

    [Fact]
    public void FluentCollapsibleRegion_ExpandedChanged()
    {
        //Arrange
        var value = false;
        var childContent = "<b>render me</b>";
        Action<bool> expandedChanged = (e) => { value = e; };
        var cut = TestContext.RenderComponent<FluentCollapsibleRegion>(parameters => parameters
            .Add(p => p.Expanded, true)
            .Add(p => p.ExpandedChanged, expandedChanged)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }
}






