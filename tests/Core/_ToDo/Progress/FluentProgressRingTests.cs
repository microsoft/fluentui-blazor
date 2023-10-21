using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Progress;
public class FluentProgressRingTests : TestBase
{
    [Fact]
    public void FluentProgressRing_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        int? min = default!;
        int? max = default!;
        int? value = default!;
        bool visible = default!;
        bool? paused = default!;
        var cut = TestContext.RenderComponent<FluentProgressRing>(parameters => parameters
            .Add(p => p.Min, min)
            .Add(p => p.Max, max)
            .Add(p => p.Value, value)
            .Add(p => p.Visible, visible)
            .Add(p => p.Paused, paused)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






