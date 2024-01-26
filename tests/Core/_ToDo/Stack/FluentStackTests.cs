using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Stack;
public class FluentStackTests : TestBase
{
    [Fact]
    public void FluentStack_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        HorizontalAlignment horizontalAlignment = default!;
        VerticalAlignment verticalAlignment = default!;
        Orientation orientation = default!;
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

