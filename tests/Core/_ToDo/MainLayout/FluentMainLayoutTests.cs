// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.MainLayout;
public class FluentMainLayoutTests : TestBase
{
    public FluentMainLayoutTests()
    {
        TestContext.JSInterop.Mode = JSRuntimeMode.Loose;
        TestContext.Services.AddSingleton(LibraryConfiguration.ForUnitTests);
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

    [Fact]
    public void FluentMainLayout_MainContent_Should_HaveCorrectHeight()
    {
        //Arrange
        int? headerHeight = 60;
        var expectedHeightStyle = $"height: 100%;";
        var header = "<b>render me</b>";
        var subHeader = "<b>render me</b>";
        var navMenuContent = "<b>render me</b>";
        var body = "<b>render me</b>";
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
        var fullContentStack = cut.FindComponent<FluentStack>();
        var mainContentStack = fullContentStack.FindComponent<FluentStack>();

        //Assert
        Assert.Equal(expectedHeightStyle, fullContentStack.Instance.Style, StringComparer.Ordinal);
        Assert.Equal(expectedHeightStyle, mainContentStack.Instance.Style, StringComparer.Ordinal);
    }
}

