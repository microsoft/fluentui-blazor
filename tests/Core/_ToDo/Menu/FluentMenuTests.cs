using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Menu;
public class FluentMenuTests : TestBase
{
    public FluentMenuTests()
    {
        TestContext.JSInterop.SetupModule("./_content/Microsoft.Fast.Components.FluentUI/Components/Menu/FluentMenu.razor.js");
    }

    [Fact]
    public void FluentMenu_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        string anchor = default!;
        Microsoft.FluentUI.AspNetCore.Components.MouseButton trigger = default!;
        bool open = default!;
        Microsoft.FluentUI.AspNetCore.Components.HorizontalPosition horizontalPosition = default!;
        string width = default!;
        Action<System.Boolean> openChanged = _ => { };
        bool anchored = default!;
        var cut = TestContext.RenderComponent<FluentMenu>(parameters => parameters
            .Add(p => p.Anchor, anchor)
            .Add(p => p.Trigger, trigger)
            .Add(p => p.Open, open)
            .AddChildContent(childContent)
            .Add(p => p.HorizontalPosition, horizontalPosition)
            .Add(p => p.Width, width)
            .Add(p => p.OpenChanged, openChanged)
            .Add(p => p.Anchored, anchored)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






