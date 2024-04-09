using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Main;
public class FluentMainTests : TestBase
{
    [Fact]
    public void FluentMain_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
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

