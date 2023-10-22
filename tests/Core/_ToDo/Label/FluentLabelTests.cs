using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Label;
public class FluentLabelTests : TestBase
{
    [Fact]
    public void FluentLabel_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        Microsoft.FluentUI.AspNetCore.Components.Typography typo = default!;
        bool disabled = default!;
        Microsoft.FluentUI.AspNetCore.Components.HorizontalAlignment? alignment = default!;
        Microsoft.FluentUI.AspNetCore.Components.Color? color = default!;
        Microsoft.FluentUI.AspNetCore.Components.FontWeight weight = default!;
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






