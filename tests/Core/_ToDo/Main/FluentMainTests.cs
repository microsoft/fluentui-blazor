using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Main;
public class FluentMainTests : TestBase
{
    [Fact]
    public void FluentMain_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        int? height = default!;
        var cut = TestContext.RenderComponent<FluentMain>(parameters => parameters
            .Add(p => p.Height, height)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






