using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Overlay;
public class FluentOverlayTests : TestBase
{
    [Fact]
    public void FluentOverlay_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        bool visible = default!;
        Action<System.Boolean> visibleChanged = _ => { };
        Action<Microsoft.AspNetCore.Components.Web.MouseEventArgs> onClose = _ => { };
        bool transparent = default!;
        double? opacity = default!;
        Microsoft.Fast.Components.FluentUI.Align alignment = default!;
        Microsoft.Fast.Components.FluentUI.JustifyContent justification = default!;
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






