using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.Grid;
public class FluentGridItemTests : TestBase
{
    [Fact]
    public void FluentGridItem_Default()
    {
        //Arrange
        string childContent = "<b>render me</b>";
        int? xs = default!;
        int? sm = default!;
        int? md = default!;
        int? lg = default!;
        int? xl = default!;
        int? xxl = default!;
        var cut = TestContext.RenderComponent<FluentGridItem>(parameters => parameters
            .Add(p => p.xs, xs)
            .Add(p => p.sm, sm)
            .Add(p => p.md, md)
            .Add(p => p.lg, lg)
            .Add(p => p.xl, xl)
            .Add(p => p.xxl, xxl)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
		cut.Verify();
    }
}






