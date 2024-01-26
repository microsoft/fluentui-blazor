using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Skeleton;
public class FluentSkeletonTests : TestBase
{
    [Fact]
    public void FluentSkeleton_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        string fill = default!;
        SkeletonShape? shape = default!;
        string pattern = default!;
        bool? shimmer = default!;
        string width = default!;
        string height = default!;
        bool visible = default!;
        var cut = TestContext.RenderComponent<FluentSkeleton>(parameters => parameters
            .Add(p => p.Fill, fill)
            .Add(p => p.Shape, shape)
            .Add(p => p.Pattern, pattern)
            .Add(p => p.Shimmer, shimmer)
            .Add(p => p.Width, width)
            .Add(p => p.Height, height)
            .Add(p => p.Visible, visible)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

