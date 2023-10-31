using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.MainLayout;
public class FluentMainLayoutTests : TestBase
{
    [Fact]
    public void FluentMainLayout_Default()
    {
        //Arrange
        string header = "<b>render me</b>";
        string subHeader = "<b>render me</b>";
        string navMenuContent = "<b>render me</b>";
        string body = "<b>render me</b>";
        int? headerHeight = default!;
        string navMenuTitle = default!;
        var cut = TestContext.RenderComponent<FluentMainLayout>(parameters => parameters
            .Add(p => p.Header, header)
            .Add(p => p.SubHeader, subHeader)
            .Add(p => p.HeaderHeight, headerHeight)
            .Add(p => p.NavMenuTitle, navMenuTitle)
            .Add(p => p.NavMenuContent, navMenuContent)
            .Add(p => p.Body, body)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






