using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Menu;
public class FluentMenuTests : TestBase
{
    public FluentMenuTests()
    {
        TestContext.JSInterop.SetupModule("./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Menu/FluentMenu.razor.js");
    }

    [Fact]
    public void FluentMenu_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        string anchor = default!;
        MouseButton trigger = default!;
        bool open = default!;
        HorizontalPosition horizontalPosition = default!;
        string width = default!;
        Action<bool> openChanged = _ => { };
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

