using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Slider;
using Bunit;
using Newtonsoft.Json.Linq;
using Xunit;

public class FluentSliderLabelTests: TestBase
    {
    [Fact]
    public void FluentSliderLaber_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
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






