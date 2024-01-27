using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.HorizontalScroll;
public class FluentHorizontalScrollTests : TestBase
{
    public FluentHorizontalScrollTests()
    {
        TestContext.JSInterop.SetupModule("./_content/Microsoft.FluentUI.AspNetCore.Components/Components/HorizontalScroll/FluentHorizontalScroll.razor.js");
    }
    [Fact]
    public void FluentHorizontalScroll_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        var speed = 600;
        string duration = default!;
        ScrollEasing? easing = ScrollEasing.Linear;
        bool? flippersHiddenFromAt = false;
        HorizontalScrollView? view = HorizontalScrollView.Default;
        var cut = TestContext.RenderComponent<FluentHorizontalScroll>(parameters => parameters
            .Add(p => p.Speed, speed)
            .Add(p => p.Duration, duration)
            .Add(p => p.Easing, easing)
            .Add(p => p.FlippersHiddenFromAt, flippersHiddenFromAt)
            .Add(p => p.View, view)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

