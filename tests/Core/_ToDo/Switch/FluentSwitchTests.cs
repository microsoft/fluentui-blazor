using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Switch;
public class FluentSwitchTests : TestBase
{
    [Fact]
    public void FluentSwitch_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        string checkedMessage = default!;
        string uncheckedMessage = default!;
        var cut = TestContext.RenderComponent<FluentSwitch>(parameters => parameters
            .AddChildContent(childContent)
            .Add(p => p.CheckedMessage, checkedMessage)
            .Add(p => p.UncheckedMessage, uncheckedMessage)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






