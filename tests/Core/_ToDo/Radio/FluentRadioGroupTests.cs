using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Radio;
using Bunit;
using Xunit;

public class FluentRadioGroupTests: TestBase
    {
    [Fact]
    public void FluentRadioGroup_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        Microsoft.Fast.Components.FluentUI.Orientation? orientation = default!;
        
        var cut = TestContext.RenderComponent<FluentRadioGroup<bool>>(parameters => parameters
            .Add(p => p.Orientation, orientation)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






