using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Stack;
public class FluentStackTests : TestBase
{
    [Fact]
    public void FluentStack_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        Microsoft.Fast.Components.FluentUI.HorizontalAlignment horizontalAlignment = default!;
        Microsoft.Fast.Components.FluentUI.VerticalAlignment verticalAlignment = default!;
        Microsoft.Fast.Components.FluentUI.Orientation orientation = default!;
        string width = default!;
        bool wrap = default!;
        int? horizontalGap = default!;
        int? verticalGap = default!;
        var cut = TestContext.RenderComponent<FluentStack>(parameters => parameters
            .Add(p => p.HorizontalAlignment, horizontalAlignment)
            .Add(p => p.VerticalAlignment, verticalAlignment)
            .Add(p => p.Orientation, orientation)
            .Add(p => p.Width, width)
            .Add(p => p.Wrap, wrap)
            .Add(p => p.HorizontalGap, horizontalGap)
            .Add(p => p.VerticalGap, verticalGap)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






