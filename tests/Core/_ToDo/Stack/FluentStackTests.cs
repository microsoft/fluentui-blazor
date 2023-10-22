using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Stack;
public class FluentStackTests : TestBase
{
    [Fact]
    public void FluentStack_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        Microsoft.FluentUI.AspNetCore.Components.HorizontalAlignment horizontalAlignment = default!;
        Microsoft.FluentUI.AspNetCore.Components.VerticalAlignment verticalAlignment = default!;
        Microsoft.FluentUI.AspNetCore.Components.Orientation orientation = default!;
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






