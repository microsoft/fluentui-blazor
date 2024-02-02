using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.MainLayout;
public class FluentMainLayoutTests : TestBase
{
    public FluentMainLayoutTests()
    {
        TestContext.JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void FluentMainLayout_Default()
    {
        //Arrange
        var header = "<b>render me</b>";
        var subHeader = "<b>render me</b>";
        var navMenuContent = "<b>render me</b>";
        var body = "<b>render me</b>";
        int? headerHeight = default!;
        string navMenuTitle = default!;
        var navMenuWidth = 320;
        var cut = TestContext.RenderComponent<FluentMainLayout>(parameters => parameters
            .Add(p => p.Header, header)
            .Add(p => p.SubHeader, subHeader)
            .Add(p => p.HeaderHeight, headerHeight)
            .Add(p => p.NavMenuTitle, navMenuTitle)
            .Add(p => p.NavMenuContent, navMenuContent)
            .Add(p => p.NavMenuWidth, navMenuWidth)
            .Add(p => p.Body, body)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

