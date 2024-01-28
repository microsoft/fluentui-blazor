using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Tooltip;

public class FluentTooltipTests : TestBase
{
    [Fact]
    public void FluentTooltip_Default()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentTooltip>(parameters =>
        {
            parameters.Add(p => p.Anchor, "button-id");
            parameters.AddChildContent("My help text");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentTooltip_AllAttributes()
    {
        // Arrange & Act
        var cut = TestContext.RenderComponent<FluentTooltip>(parameters =>
        {
            parameters.Add(p => p.Anchor, "button-id");
            parameters.Add(p => p.UseTooltipService, false);
            parameters.Add(p => p.Visible, true);
            parameters.Add(p => p.Delay, 200);
            parameters.Add(p => p.Position, TooltipPosition.Bottom);
            parameters.Add(p => p.MaxWidth, "300px");
            parameters.Add(p => p.AutoUpdateMode, AutoUpdateMode.Auto);
            parameters.Add(p => p.HorizontalViewportLock, true);
            parameters.Add(p => p.VerticalViewportLock, true);
            parameters.Add(p => p.OnDismissed, (e) => { });
            parameters.AddChildContent("My help text");
        });

        // Assert
        cut.Verify();
    }
}
