using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Grid;
public class FluentGridTests : TestBase
{
    [Fact]
    public void FluentGrid_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        int spacing = default!;
        Microsoft.Fast.Components.FluentUI.JustifyContent justify = default!;
        var cut = TestContext.RenderComponent<FluentGrid>(parameters => parameters
            .Add(p => p.Spacing, spacing)
            .Add(p => p.Justify, justify)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






