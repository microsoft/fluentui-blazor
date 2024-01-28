using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Header;
public class FluentHeaderTests : TestBase
{
    [Fact]
    public void FluentHeader_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        int? height = default!;
        var cut = TestContext.RenderComponent<FluentHeader>(parameters => parameters
            .Add(p => p.Height, height)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

