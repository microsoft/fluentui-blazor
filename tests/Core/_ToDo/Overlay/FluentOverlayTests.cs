// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Overlay;
public class FluentOverlayTests : TestBase
{
    [Inject]
    public GlobalState GlobalState { get; set; } = new GlobalState();
    public FluentOverlayTests()
    {
        TestContext.JSInterop.Mode = JSRuntimeMode.Loose;
        TestContext.Services.AddSingleton(LibraryConfiguration.ForUnitTests);
        TestContext.Services.AddSingleton(GlobalState);

    }

    [Fact]
    public void FluentOverlay_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        bool visible = default!;
        Action<bool> visibleChanged = _ => { };
        Action<Microsoft.AspNetCore.Components.Web.MouseEventArgs> onClose = _ => { };
        bool transparent = default!;
        double? opacity = default!;
        Align alignment = default!;
        JustifyContent justification = default!;
        bool fullScreen = default!;
        bool dismissable = default!;
        string backgroundColor = default!;
        bool preventScroll = default!;
        var cut = TestContext.RenderComponent<FluentOverlay>(parameters => parameters
            .Add(p => p.Visible, visible)
            .Add(p => p.VisibleChanged, visibleChanged)
            .Add(p => p.OnClose, onClose)
            .Add(p => p.Transparent, transparent)
            .Add(p => p.Opacity, opacity)
            .Add(p => p.Alignment, alignment)
            .Add(p => p.Justification, justification)
            .Add(p => p.FullScreen, fullScreen)
            .Add(p => p.Dismissable, dismissable)
            .Add(p => p.BackgroundColor, backgroundColor)
            .Add(p => p.PreventScroll, preventScroll)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

