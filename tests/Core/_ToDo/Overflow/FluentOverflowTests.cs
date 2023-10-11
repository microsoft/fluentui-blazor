using Bunit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Overflow;
public class FluentOverflowTests : TestBase
{
    public FluentOverflowTests()
    {
        TestContext.JSInterop.SetupModule("./_content/Microsoft.Fast.Components.FluentUI/Components/Overflow/FluentOverflow.razor.js");
    }

    [Fact(Skip = "Need to figure out how to do this test")]
    public void FluentOverflow_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        RenderFragment<FluentOverflow> overflowTemplate = default!;
        RenderFragment<FluentOverflow> moreButtonTemplate = default!;
        Orientation orientation = default!;
        Action<IEnumerable<FluentOverflowItem >> onOverflowRaised = _ => { };
        var cut = TestContext.RenderComponent<FluentOverflow>(parameters => parameters
            .AddChildContent(childContent)
            .Add(p => p.OverflowTemplate, overflowTemplate)
            .Add(p => p.MoreButtonTemplate, moreButtonTemplate)
            .Add(p => p.Orientation, orientation)
            .Add(p => p.OnOverflowRaised, onOverflowRaised)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






