using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Label;
public class FluentLabelTests : TestBase
{
    [Fact]
    public void FluentLabel_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        Microsoft.Fast.Components.FluentUI.Typography typo = default!;
        bool disabled = default!;
        Microsoft.Fast.Components.FluentUI.HorizontalAlignment? alignment = default!;
        Microsoft.Fast.Components.FluentUI.Color? color = default!;
        Microsoft.Fast.Components.FluentUI.FontWeight weight = default!;
        string marginBlock = default!;
        var cut = TestContext.RenderComponent<FluentLabel>(parameters => parameters
            .Add(p => p.Typo, typo)
            .Add(p => p.Disabled, disabled)
            .Add(p => p.Alignment, alignment)
            .Add(p => p.Color, color)
            .Add(p => p.Weight, weight)
            .Add(p => p.MarginBlock, marginBlock)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






