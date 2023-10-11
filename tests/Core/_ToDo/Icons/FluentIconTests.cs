using Bunit;
using Microsoft.AspNetCore.Components.Web;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Icons;

public class FluentIconTests : TestBase
{
    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentIcon_Default()
    {
        //Arrange
        string slot = default!;
        string title = default!;
        Color? color = default!;
        string customColor = default!;
        string width = default!;
        Icon value = default!;
        Action<MouseEventArgs> onClick = _ => { };
        var cut = TestContext.RenderComponent<FluentIcon<Icon>>(parameters => parameters
            .Add(p => p.Slot, slot)
            .Add(p => p.Title, title)
            .Add(p => p.Color, color)
            .Add(p => p.CustomColor, customColor)
            .Add(p => p.Width, width)
            .Add(p => p.Value, value)
            .Add(p => p.OnClick, onClick)
        );
        //Act

        //Assert
        cut.Verify();
    }
}