namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Slider;

using Bunit;
using Xunit;

public class FluentSliderLabelTests : TestBase
{
    public FluentSliderLabelTests()
    {
        TestContext.JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void FluentSliderLaber_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        int position = default!;
        bool? hideMark = default!;
        bool? disabled = default!;
        var cut = TestContext.RenderComponent<FluentSliderLabel<int>>(parameters => parameters
            .Add(p => p.Position, position)
            .Add(p => p.HideMark, hideMark)
            .Add(p => p.Disabled, disabled)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

