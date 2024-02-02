using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Label;
public class FluentLabelTests : TestBase
{
    [Fact]
    public void FluentLabel_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        Typography typo = default!;
        bool disabled = default!;
        HorizontalAlignment? alignment = default!;
        Color? color = default!;
        FontWeight weight = default!;
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

