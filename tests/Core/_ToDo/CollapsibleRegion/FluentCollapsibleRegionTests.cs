using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.CollapsibleRegion;
public class FluentCollapsibleRegionTests : TestBase
{
    [Fact]
    public void FluentCollapsibleRegion_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";

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
        string childContent = "<b>render me</b>";
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
        string childContent = "<b>render me</b>";
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
        bool value = false;
        string childContent = "<b>render me</b>";
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






