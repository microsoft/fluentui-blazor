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
        bool expanded = default!;
        string maxHeight = default!;
        Action<System.Boolean> expandedChanged = _ => { };
        var cut = TestContext.RenderComponent<FluentCollapsibleRegion>(parameters => parameters
            .Add(p => p.Expanded, expanded)
            .Add(p => p.MaxHeight, maxHeight)
            .AddChildContent(childContent)
            .Add(p => p.ExpandedChanged, expandedChanged)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






